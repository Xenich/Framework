using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.BotGenerator;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.StatesBuilders
{
    public class StateBuilderStart : MainStateBuilderStart<StateBuilderStart>
    {
        internal StateBuilderStart(StatesBuilder statesBuilder, State state)
            :base(statesBuilder, state)
        {
            derivedStateBuilder = this;
        }

        /// <summary>
        /// Закончить формирование состояния
        /// </summary>
        public StatesBuilder Next
        {            
            get { return statesBuilder; }
        }
    }
}
