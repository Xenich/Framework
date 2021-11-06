using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.BotGenerator;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.StatesBuilders
{
    public class FixedInlineStateBuilder_Start : MainStateBuilderStart<FixedInlineStateBuilder_Start>
    {
        internal FixedInlineStateBuilder_Start(StatesBuilder statesBuilder, FixedInlineState state)
            : base(statesBuilder, state)
        {
            derivedStateBuilder = this;
        }

        /// <summary>
        /// Начать создавать клавиатуру
        /// </summary>
        public FixedInlineKeyboardBuilder CreateKeyboard
        {
            get
            {
                FixedInlineKeyboardBuilder fixedInlineKeyboardBuilder = new FixedInlineKeyboardBuilder(statesBuilder, state as FixedInlineState);
                return fixedInlineKeyboardBuilder;
            }
        }
    }


}
