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
using TelegramBotConstructor.MessageHandlers;
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
        internal readonly IStateResolver _userDefinedStateResolver;
        private readonly IncomingUpdatesQueueMultiThreadHandler incomingUpdatesQueueMultiThreadHandler;

        internal bool IsWebhook { private get;  set; }
        internal int Interval { private get; set; }

        internal bool IsCustomInlineStateResolver { get; set; }

        /// <summary>
        /// Нужно ли делать проверку на то, послан ли запрос из предыдущей inline-клавиатуры, чтоб отказаться от его обработки в будущем.
        /// Рекомендуется true
        /// </summary>
        internal bool isNeedToCheckPreviousInlineMessage = true;

        /// <summary>
        /// Словарь chatId -> Id последнего обработанного сообщения этого чата. Словарь ведется в случае isNeedToCheckPreviousInlineMessage = true
        /// </summary>
        private readonly Dictionary<int, int> chatIdToCurrentMessageId_dic = new Dictionary<int, int>();       

        internal void SetCurrentMessageIdToChatId(int chatId, int messageId)
        {
            if (chatIdToCurrentMessageId_dic.ContainsKey(chatId))
                chatIdToCurrentMessageId_dic[chatId] = messageId;
            else
                chatIdToCurrentMessageId_dic.Add(chatId, messageId);
        }

        internal int GetCurrentMessageIdByChatId(int chatId)
        {
            if (chatIdToCurrentMessageId_dic.ContainsKey(chatId))
                return chatIdToCurrentMessageId_dic[chatId];
            else
                return 0;
        }

        
        /// <summary>
        /// Делегат на функцию, обрабатывающую событие удаления юзером чата с ботом
        /// </summary>
        internal Action<Update> BotKickedEventHandler;

        /// <summary>
        /// Делегат на функцию, обрабатывающую событие добавления юзером бота
        /// </summary>
        internal Action<Update> BotAddedEventHandler;

        private readonly bool isNeedLogging;

        private int lastUpdate = 0;   // текущий номер сообщения

        internal string Token { get;  set; }

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
            _userDefinedStateResolver = stateResolver;
            incomingUpdatesQueueMultiThreadHandler = new IncomingUpdatesQueueMultiThreadHandler(this);
        }

        /// <summary>
        /// Проверка на то, является ли входящее сообщение нажатием на кнопку старой инлайн-клавиатуры и нужно ли такое сообщение обрабатывать
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="update"></param>
        /// <returns>true - старое сообщение, обрабатывать не нужно. false - нужно обработать</returns>
        internal bool CheckIfPreviousInlineMessage(int chatId, Update update)
        {
            if (isNeedToCheckPreviousInlineMessage)
            {
                if (!chatIdToCurrentMessageId_dic.ContainsKey(chatId))
                    chatIdToCurrentMessageId_dic.Add(chatId, update.GetMessageId());
                int currentMessageId = chatIdToCurrentMessageId_dic[chatId];
                int incomingMessageId = update.GetMessageId();

                if (incomingMessageId < currentMessageId)
                    return true;
                else
                    chatIdToCurrentMessageId_dic[chatId] = incomingMessageId;
                return false;
            }
            return false;
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
                            AddUdatesToQueue(resp);
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

        public void AddUdatesToQueue(Response incomingUpdates)
        {
            if (incomingUpdates.Ok && incomingUpdates.Updates.Count > 0)
            {
                for (int i = 0; i < incomingUpdates.Updates.Count; i++)
                {
                    lastUpdate = incomingUpdates.Updates[i].UpdateId;
                    HandleMessageFromTelegramAsync(incomingUpdates.Updates[i]);
                }
                lastUpdate++;
            }
        }

        /// <summary>
        /// Обработчик сообщения от юзера
        /// </summary>
        /// <param name="update">Объект сообщения</param>
        async Task HandleMessageFromTelegramAsync(Update update)
        {
            try
            {
                MessageHandler messageHandler = MessageHandlerFactory.CreateHandler(update, this);
                await messageHandler.Handle();
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




        //This object represents an incoming callback query from a callback button in an inline keyboard



        public State GetStateByName(string name)
        {
            if (nameToState_dic.ContainsKey(name))
                return nameToState_dic[name];
            else
                return null;
        }

        internal State GetStateByUid(Guid uid)
        {
            if (uidToState_dic.ContainsKey(uid))
                return uidToState_dic[uid];
            else
                return null;
        }


        internal void Log(string message)
        {
            if(isNeedLogging)
                _logger.Log(LogLevel.Information, message);
        }

        internal void LogError(string message)
        {
            if (isNeedLogging)
                _logger.Log(LogLevel.Error, message);
        }
    }
}
