using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.StatesBuilders;

namespace TelegramBotConstructor.StatesBuilders
{
    public class StatesBuilderStarter
    {
        readonly Bot bot;
        internal StatesBuilderStarter(Bot bot)
        {
            this.bot = bot;
        }

        /// <summary>
        /// Начать добавлять состояния
        /// </summary>
        public StatesBuilder BeginAddStates
        {
            get
            {
                StatesBuilder statesBuilder = new StatesBuilder(bot);
                return statesBuilder;
            }
        }
    }
}
