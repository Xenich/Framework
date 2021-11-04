using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Iterfaces;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.States;
using TelegramBotConstructor.StatesBuilders;

namespace TelegramBotConstructor.BotGenerator
{
     public class BotBuilder
    {
        private readonly Bot bot;

        internal BotBuilder(Bot bot)
        {            
            this.bot = bot;
        }

        /// <summary>
        /// Окончательная проверка возможности билда
        /// </summary>
        /// <returns></returns>
        public bool IsReadyToBuild()
        {

            // TODO : reilize
            return true;
        }

        /// <summary>
        /// Сгенерировать бота
        /// </summary>
        /// <returns></returns>
        public Bot Build()
        {
            // TODO - установить вебхук, если надо
            return bot;
        }
    }
}
