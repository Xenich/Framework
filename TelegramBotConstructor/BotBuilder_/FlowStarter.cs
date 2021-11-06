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
        public StatesBuilderStarter SetInternalHandlerWebHook(int interval)
        {
            bot.IsWebhook = false;
            bot.Interval = interval;
            StatesBuilderStarter statesBuilderStarter = new StatesBuilderStarter(bot);
            return statesBuilderStarter;
        }
    }

}
