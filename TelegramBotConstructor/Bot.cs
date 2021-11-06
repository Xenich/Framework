using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelegramBotConstructor.BotGenerator;
using TelegramBotConstructor.Iterfaces;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor
{
    /// <summary>
    /// Бот представляет собой конечный автомат с множеством состояний - объекты типа State. 
    /// Каждое состояние иеет свой 32-битный GUID-идентификатор, наименование и обработчик 
    /// </summary>
    public class Bot
    {
        private const string baseTelegramAddress = @"https://api.telegram.org/bot";
        /// <summary>
        /// Логгер
        /// </summary>
        private readonly ILogger _logger;
        /// <summary>
        /// Определение состояния конечного автомата
        /// </summary>
        private readonly IStateResolver _stateResolver;
                                                   
        internal bool IsWebhook { private get;  set; }
        internal int Interval { private get; set; }

        internal bool IsCustomInlineStateResolver { private get; set; }

        /// <summary>
        /// Нужно ли делать проверку на то, послан ли запрос из предыдущей inline-клавиатуры, чтоб отказаться от его обработки в будущем.
        /// Рекомендуется true
        /// </summary>
        internal bool isNeedToCheckPreviousInlineMessage = true;

        /// <summary>
        /// Словарь chatId -> Id последнего обработанного сообщения этого чата. Словарь ведется в случае isNeedToCheckPreviousInlineMessage = true
        /// </summary>
        private readonly Dictionary<int, int> chatIdToCurrentMessageId_dic = new Dictionary<int, int>();

        /// <summary>
        /// Делегат на функцию определения состояния
        /// </summary>
        //internal Func<Update, string> StateResolver;


        readonly bool isNeedLogging;
        int lastUpdate = 0;   // текущий номер сообщения

        public string Token { get; internal set; }

        /// <summary>
        /// словарь : имя состояния -> состояние конечного автомата
        /// </summary>
        readonly Dictionary<string, State> nameToState_dic = new Dictionary<string, State>(); 

        /// <summary>
        /// словарь : идентификатор состояния -> состояние конечного автомата
        /// </summary>
        readonly Dictionary<Guid, State> uidToState_dic = new Dictionary<Guid, State>();  


        internal Bot(ILogger logger, IStateResolver stateResolver)
        {
            if (logger != null)
            {
                isNeedLogging = true;
                _logger = logger;
            }
            _stateResolver = stateResolver;
        }

        /// <summary>
        /// Стротель бота в flow-стиле
        /// </summary>
        /// <param name="logger">Логгер</param>
        /// <param name="stateResolver">Интерфейс определения состояния конечного автомата в зависимости от типа пришедшего сообщения (inline, text, photo и т.д.)</param>
        /// <returns></returns>
        public static FlowStarter StartFlowBuilder(ILogger logger, IStateResolver stateResolver)
        {
            FlowStarter botBuilder = new FlowStarter(logger, stateResolver);
            return botBuilder;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (IsWebhook)
                {
                    LogError("Бот установлен в режим вебхука");
                    return;
                }
                Uri baseaddress = new Uri(baseTelegramAddress);
                using (HttpClient client = new HttpClient())
                {
                    Log("трулала");
                    client.BaseAddress = baseaddress;
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(Interval);
                        try
                        {
                            Uri url = new Uri(baseTelegramAddress + Token + @"/getUpdates?offset=" + lastUpdate.ToString());
                            byte[] bytes = await client.GetByteArrayAsync(url);
                            string respText = Encoding.UTF8.GetString(bytes);
                            Response resp = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(respText);
                            if (resp.Ok && resp.Updates.Count > 0)
                            {
                                for (int i = 0; i < resp.Updates.Count; i++)
                                {
                                    lastUpdate = resp.Updates[i].UpdateId;
                                    HandleMessageFromTelegramAsync(resp.Updates[i]);
                                }
                                lastUpdate++;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (isNeedLogging)
                                _logger.LogError(ex.Message);
                            lastUpdate++;
                        }
                    }
                }
                Log("Работа бота завершена");
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        /// <summary>
        /// Добавить состояние в словари состояний. Состояние должно иметь уникальный идентификатор и имя
        /// </summary>
        /// <param name="state">Состояние</param>
        /// <returns>Успех/неудача добавления состояния</returns>
        internal bool TryAddState(State state)
        {
            Log("TryAddState");

            if (nameToState_dic.ContainsKey(state.Name))
                return false;
            if (uidToState_dic.ContainsKey(state.Uid))
                return false;

            nameToState_dic.Add(state.Name, state);
            uidToState_dic.Add(state.Uid, state);

            return true;
        }

        /// <summary>
        /// Обработчик сообщения от юзера
        /// </summary>
        /// <param name="update">Объект сообщения</param>
        async Task HandleMessageFromTelegramAsync(Update update)
        {
            try
            {
                int chatId = update.GetChatId();

                // 0. Определяем сначала, нужно ли вообще обрабатывать сообщение
                if (isNeedToCheckPreviousInlineMessage)
                {
                    if (!chatIdToCurrentMessageId_dic.ContainsKey(chatId))
                        chatIdToCurrentMessageId_dic.Add(chatId, update.GetMessageId());
                    int currentMessageId = chatIdToCurrentMessageId_dic[chatId];
                    int incomingMessageId = update.GetMessageId();

                    if (incomingMessageId < currentMessageId)
                        return;
                    else
                        chatIdToCurrentMessageId_dic[chatId] = incomingMessageId;
                }
                // 1. Определяем текущее состояние
                Guid stateUid = ResolveStateUid(update);         // Определяем идентификатор текущего состояния
                State currentState = GetStateByUid(stateUid);

                if (currentState != null)
                {
                    // 2. Вызываем обработчик текущего состояния
                    await StateHandler(update, chatId, currentState);

                    // 3. Устанавливаем новое текущее состояние
                    _stateResolver.SetCurrentState(update, currentState.DefaultNextStateUid);

                    // 4. Конец обработки сообщения
                    Log(string.Format("HandleMessageFromTelegram: update_id = {0}", update.UpdateId));
                }
                else
                    LogError(string.Format("HandleMessageFromTelegram: update_id = {0} . Не удалось найти State с Guid = {1}", update.UpdateId, stateUid.ToString()));
            }
            catch (Exception ex)
            {
                LogError(ex.Message);
            }
        }

        /// <summary>
        /// Обработчик текущего состояния конечного автомата
        /// </summary>
        /// <param name="update"></param>
        /// <param name="chatId"></param>
        /// <param name="state"></param>
        private async Task StateHandler(Update update, int chatId, State state)
        {
            if (isNeedToCheckPreviousInlineMessage)
            {
                string response = await state.StateHandler.HandleWithResponceAsync(update);
                if (response == "BADSTATUS")
                {
                    LogError(string.Format("HandleMessageFromTelegram: update_id = {0}. BADSTATUS response", update.UpdateId));
                }
                else
                {
                    ResponceFromTelegram resp = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponceFromTelegram>(response);
                    if (resp.Ok)
                    {
                        int message_id = resp.Result.MessageId;
                        chatIdToCurrentMessageId_dic[chatId] = message_id;
                    }
                }
            }
            else
                state.StateHandler.HandleWithoutResponce(update);
        }

        private Guid ResolveStateUid(Update update)
        {
            Guid stateUid = Guid.Empty;
            if (update.IsPhotoMessage())            // пришло фото
            {
                stateUid = _stateResolver.PhotoMessageResolve(update);
            }
            if (update.IsSimpleMessage())           // пришло сообщение, введенное пользователем
            {
                stateUid = _stateResolver.SimpleMessageResolve(update);
            }
            if (update.IsCallbackQueryMessage())    // пришло сообщение от inline-клавиатуры
            {
                if (IsCustomInlineStateResolver)
                    stateUid = _stateResolver.InlineMessageResolve(update);
                else
                    stateUid = StandartResolveInlineState(update.CallbackQuery.Data);

            }
            return stateUid;
        }
        //This object represents an incoming callback query from a callback button in an inline keyboard

        /// <summary>
        /// Стандартный метод, определяющий состояние конечного автомата при входящем сообщении после нажатия кнопки inline-клавиатуры.
        /// Считывает первывые 32 байта callbackQuery и преобразовывает их в Guid
        /// </summary>
        /// <param name="callbackQueryData">объект Update.CallbackQuery.Data</param>
        /// <returns>Идентификатор состояния</returns>
        private Guid StandartResolveInlineState(string callbackQueryData)
        {
            if(callbackQueryData.Length<32)
                throw new Exception(string.Format("Не удаётся прочитать Guid состояния. callbackQueryData: {0}", callbackQueryData));
            string uid = callbackQueryData.Substring(0,32);
            Guid guid;
            bool succeess = Guid.TryParse(uid, out guid);
            if (!succeess)
                throw new Exception(string.Format("Не удаётся прочитать Guid состояния. callbackQueryData: {0}", callbackQueryData));
            else
                return guid;
        }

        public State GetStateByName(string name)
        {
            if (nameToState_dic.ContainsKey(name))
                return nameToState_dic[name];
            else
                return null;
        }

        public State GetStateByUid(Guid uid)
        {
            if (uidToState_dic.ContainsKey(uid))
                return uidToState_dic[uid];
            else
                return null;
        }


        void Log(string message)
        {
            if(isNeedLogging)
                _logger.Log(LogLevel.Information, message);
        }

        void LogError(string message)
        {
            if (isNeedLogging)
                _logger.Log(LogLevel.Error, message);
        }
    }
}
