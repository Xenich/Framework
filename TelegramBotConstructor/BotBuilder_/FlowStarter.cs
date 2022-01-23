using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Iterfaces;
using TelegramBotConstructor.StatesBuilders;

namespace TelegramBotConstructor.BotGenerator
{
    public class FlowStarter
    {
        readonly Bot bot;
        //private State currentState;

        internal FlowStarter(ILogger logger, IStateResolver stateResolver)
        {
            bot = new Bot(logger, stateResolver);
        }

        /// <summary>
        /// Установить токен, полученный от BotFather
        /// </summary>
        /// <param name="token">токен</param>
        /// <returns></returns>
        public WebhookSetter SetToken(string token)
        {
            bot.Token = token;
            WebhookSetter webhookSetter = new WebhookSetter(bot);
            return webhookSetter;
        }
    }

    public class WebhookSetter
    {
        Bot bot;
        internal WebhookSetter(Bot bot)
        {
            this.bot = bot;
        }

        /// <summary>
        /// Установить обработку входящих сообщений с помощью вебхука
        /// </summary>        
        public StatesBuilderStarter SetWebHook()
        {
            bot.IsWebhook = true;
            StatesBuilderStarter statesBuilderStarter = new StatesBuilderStarter(bot);
            return statesBuilderStarter;
        }

        /// <summary>
        /// Установить внутреннюю обработку входящих сообщений (вместо вебхука, рекомендуется только для тестирования)
        /// </summary>
        /// <param name="interval">Интервал в милисекундах, через который будет посылаться запрос на API телеграма для получения новых сообщений</param>
        /// <returns></returns>
        public OrderOfMessageHandlingSetter SetInternalHandlerWebHook(int interval)
        {
            bot.IsWebhook = false;
            bot.Interval = interval;
            OrderOfMessageHandlingSetter orderOfMessageHandlingSetter = new OrderOfMessageHandlingSetter(bot);
            return orderOfMessageHandlingSetter;
        }
    }


    public class OrderOfMessageHandlingSetter
    {
        Bot bot;
        internal OrderOfMessageHandlingSetter(Bot bot)
        {
            this.bot = bot;
        }

        /// <summary>
        /// Установить стратегию обработки сообщений.
        /// </summary>
        /// <param name="orderOfUpdatesHandling"></param>
        /// <returns></returns>
        public NeedToCheckPreviousInlineMessageSetter SetIsNeedHandleMessagesInApearenceOrder(OrderOfUpdatesHandling orderOfUpdatesHandling)
        {
            bot.orderOfUpdatesHandling = orderOfUpdatesHandling;
            NeedToCheckPreviousInlineMessageSetter needToCheckPreviousInlineMessageSetter = new NeedToCheckPreviousInlineMessageSetter(bot);
            return needToCheckPreviousInlineMessageSetter;
        }
    }

    public class NeedToCheckPreviousInlineMessageSetter
    {
        Bot bot;
        internal NeedToCheckPreviousInlineMessageSetter(Bot bot)
        {
            this.bot = bot;
        }

        /// <summary>
        /// Нужно ли делать проверку на то, послан ли запрос из предыдущей inline-клавиатуры, чтоб отказаться от его обработки в будущем.
        /// Рекомендуется true
        /// </summary>
        /// <param name="value">true - Каждое inline-сообщение будет проверяться на то, послано ли оно из устаревшей  inline-клавиатуры и если да, то оно обрабатываться не будет </param>
        /// <returns></returns>
        public InlineStateResolverSetter SetIsNeedToCheckPreviousInlineMessage(bool value)
        {
            bot.isNeedToCheckPreviousInlineMessage = value;
            InlineStateResolverSetter inlineStateResolverSetter = new InlineStateResolverSetter(bot);
            return inlineStateResolverSetter;
        }
    }

    public class InlineStateResolverSetter
    {
        Bot bot;
        internal InlineStateResolverSetter(Bot bot)
        {
            this.bot = bot;
        }

        /// <summary>
        /// Установить метод обработки входящих сообщений из inline-клавиатуры
        /// Standart - Стандартный метод предполагает наличие Guid состояния в первых 32 байтах объекта update.CallbackQuery.Data (при этом метод InlineMessageResolve реализации интерфейса IStateResolver (в случае его наличия) игнорируется).
        /// Custom - Вместо стандартного метода для определения текущего состояния будет вызываться метод InlineMessageResolve реализации интерфейса IStateResolver.
        /// </summary>
        /// <param name="inlineStateResolver"></param>
        /// <returns></returns>
        public BotAddedEventHandler SetInlineStateResolver(InlineStateResolver inlineStateResolver)
        {
            if(inlineStateResolver == InlineStateResolver.Custom)                           
                bot.IsCustomInlineStateResolver = true;
            else
                bot.IsCustomInlineStateResolver = false;

            BotAddedEventHandler botAddedEventHandler = new BotAddedEventHandler(bot);
            return botAddedEventHandler;
        }        
    }

    public class BotAddedEventHandler
    {
        Bot bot;
        internal BotAddedEventHandler(Bot bot)
        {
            this.bot = bot;
        }

        /// <summary>
        /// Установить обработчик события добавления юзером бота
        /// </summary>
        /// <param name="botAddedEventHandler">Делегат на обработчик</param>
        /// <returns></returns>
        public BotKickedEventHandlerSetter SetBotAddedEventHandler(Action<Update> botAddedEventHandler)
        {
            bot.BotAddedEventHandler = botAddedEventHandler;
            BotKickedEventHandlerSetter botKickedEventHandlerSetter = new BotKickedEventHandlerSetter(bot);
            return botKickedEventHandlerSetter;


        }
    }

    public class BotKickedEventHandlerSetter
    {
        Bot bot;
        internal BotKickedEventHandlerSetter(Bot bot)
        {
            this.bot = bot;
        }

        /// <summary>
        /// Установить обработчик события удаления чата с ботом
        /// </summary>
        /// <param name="botKickedHandler">Делегат на обработчик</param>
        /// <returns></returns>
        public StatesBuilderStarter SetBotKickedEventHandler(Action<Update> botKickedHandler)
        {
            bot.BotKickedEventHandler = botKickedHandler;
            StatesBuilderStarter statesBuilderStarter = new StatesBuilderStarter(bot);
            return statesBuilderStarter;
        }
    }


}




