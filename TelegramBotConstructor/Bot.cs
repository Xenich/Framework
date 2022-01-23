using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
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
using Newtonsoft.Json;

namespace TelegramBotConstructor
{
    /// <summary>
    /// Бот представляет собой конечный автомат с множеством состояний - объекты типа State. 
    /// Каждое состояние имеет свой 32-битный GUID-идентификатор, наименование и обработчик 
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
        //private readonly IncomingUpdatesQueueMultiThreadHandler incomingUpdatesQueueMultiThreadHandler;

        private readonly bool isNeedLogging;

        /// <summary>
        /// словарь : имя состояния -> состояние конечного автомата
        /// </summary>
        private readonly Dictionary<string, State> nameToState_dic = new Dictionary<string, State>();

        /// <summary>
        /// словарь : идентификатор состояния -> состояние конечного автомата
        /// </summary>
        private readonly Dictionary<Guid, State> uidToState_dic = new Dictionary<Guid, State>();

        /// <summary>
        /// Словарь chatId -> Id последнего обработанного сообщения этого чата. Словарь ведется в случае isNeedToCheckPreviousInlineMessage = true
        /// </summary>
        private readonly Dictionary<int, int> chatIdToCurrentMessageId_dic = new Dictionary<int, int>();

        private readonly IncomingUpdatesInAppearanceOrderQueueHandler incomingUpdatesInAppearanceOrderQueueHandler;
        private readonly IncomingUpdatesQueueMultiThreadHandler incomingUpdatesQueueMultiThreadHandler;

        internal bool IsWebhook { private get;  set; }
        internal int Interval { private get; set; }

        [JsonProperty("IsCustomInlineStateResolver")]
        internal bool IsCustomInlineStateResolver { get; set; }

        /// <summary>
        /// Необходимость обрабатывать сообщения в порядке их поступления
        /// </summary>
        internal bool IsNeedHandleMessagesInApearenceOrder { get; set; } = false;

        /// <summary>
        /// Нужно ли делать проверку на то, послан ли запрос из предыдущей inline-клавиатуры, чтоб отказаться от его обработки в будущем.
        /// Рекомендуется true
        /// </summary>
        internal bool isNeedToCheckPreviousInlineMessage = true;

        [JsonProperty("BotKickedEventHandler")]
        /// <summary>
        /// Делегат на функцию, обрабатывающую событие удаления юзером чата с ботом
        /// </summary>
        internal Action<Update> BotKickedEventHandler;

        /// <summary>
        /// Делегат на функцию, обрабатывающую событие добавления юзером бота
        /// </summary>
        internal Action<Update> BotAddedEventHandler;

        private int lastUpdate = 0;   // текущий номер сообщения
        

        internal string Token { get;  set; }

        internal Bot()
        { }

        internal Bot(ILogger logger, IStateResolver stateResolver)
        {
            if (logger != null)
            {
                isNeedLogging = true;
                _logger = logger;
            }
            _userDefinedStateResolver = stateResolver;
            incomingUpdatesInAppearanceOrderQueueHandler = new IncomingUpdatesInAppearanceOrderQueueHandler(this);
            incomingUpdatesQueueMultiThreadHandler = new IncomingUpdatesQueueMultiThreadHandler(this);
        }

//********************************************************************************************************

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
        /// Проверка на то, является ли входящее сообщение нажатием на кнопку старой инлайн-клавиатуры и нужно ли такое сообщение обрабатывать
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="update"></param>
        /// <returns>true - старое сообщение, обрабатывать не нужно. false - нужно обработать</returns>
        private bool CheckIfPreviousInlineMessage(int chatId, Update update)
        {
            if (update.Type == UpdateTypes.CallbackQuery && isNeedToCheckPreviousInlineMessage)
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
        /// Строитель бота в flow-стиле
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
                    while (!cancellationToken.IsCancellationRequested )
                    {
                        await Task.Delay(Interval);
                        try
                        {
                            Uri url = new Uri(baseTelegramAddress + Token + @"/getUpdates?offset=" + lastUpdate.ToString());
                            byte[] bytes = await client.GetByteArrayAsync(url);
                            string respText = Encoding.UTF8.GetString(bytes);
                            Response incomingUpdates = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(respText);
                            if (incomingUpdates.Updates != null && incomingUpdates.Updates.Count > 0)
                            {
                                await HandleUpdates(incomingUpdates);
                                lastUpdate = ++incomingUpdates.Updates.Last().UpdateId;
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



        public async Task HandleUpdates(Response incomingUpdates)
        {
            for (int i = 0; i < incomingUpdates.Updates.Count; i++)
            {
                if (!CheckIfPreviousInlineMessage(incomingUpdates.Updates[i].GetChatId(), incomingUpdates.Updates[i]))      //Определяем сначала, нужно ли вообще обрабатывать сообщение(если оно -инлайн)
                {
                    if (IsNeedHandleMessagesInApearenceOrder)
                    {
                        /*
                        int countOfThreads = 20;
                        Thread[] threads = new Thread[countOfThreads];
                        for (int j = 0; j < countOfThreads; j++)
                        {
                            
                            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(incomingUpdatesQueueMultiThreadHandler.AddUpdateToQueue);
                            Thread thread = new Thread(parameterizedThreadStart);
                            threads[j] = thread;
                        }   
                        foreach(Thread thread1 in threads)
                            thread1.Start(incomingUpdates.Updates[i]);
                        */

                        incomingUpdatesInAppearanceOrderQueueHandler.AddUpdateToQueue(incomingUpdates.Updates[i]);

                        
                    }
                    else
                    {
                        incomingUpdatesQueueMultiThreadHandler.AddUpdateToQueue(incomingUpdates.Updates[i]);
                        //HandleMessageFromTelegramAsync(incomingUpdates.Updates[i]);
                        //incomingUpdatesInAppearanceOrderQueueHandler.AddUpdatesToQueue(incomingUpdates.Updates[i]);


                    }
                }
            }
        }

        

        public async Task AddUdatesToQueue(Response incomingUpdates)
        {
            if (incomingUpdates.Ok && incomingUpdates.Updates.Count > 0)
            {
                
                Queue<Update> updates = new Queue<Update>();

                IEnumerable<IGrouping<int, Update>> messageGroups = incomingUpdates.Updates.GroupBy(u => u.GetChatId());
                foreach (IGrouping<int, Update> group in messageGroups)
                {
                    int groupCounter = 0;
                    foreach (Update update in group)
                    {
                        if (update.Type == UpdateTypes.MyChatMember)
                        {
                            updates.Enqueue(update);
                            break;
                        }
                        groupCounter++;
                        if (groupCounter == group.Count())  // если мы просмотрели все сообщения в группе сообщений этого юзера, то обрабатываем только последнее сообщение
                            updates.Enqueue(update);
                    }
                }
                

                if (IsNeedHandleMessagesInApearenceOrder)
                {
                    for (int i = 0; i < incomingUpdates.Updates.Count; i++)
                    {
                        await HandleMessageFromTelegramAsync(incomingUpdates.Updates[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < incomingUpdates.Updates.Count; i++)
                    {
                        HandleMessageFromTelegramAsync(incomingUpdates.Updates[i]);
                    }
                }
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
                //await Task.Delay(2000);
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
