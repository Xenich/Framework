using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.States;
using TelegramBotConstructor.StatesBuilders;

namespace TelegramBotConstructor.StatesBuilders
{
    public class FixedInlineKeyboardBuilder
    {
        private readonly StatesBuilder statesBuilder;
        private readonly InlineKeyboard inlineKeyboard;
        private readonly FixedInlineState inlineState;

        internal FixedInlineKeyboardBuilder(StatesBuilder statesBuilder, FixedInlineState inlineState)
        {
            this.statesBuilder = statesBuilder;
            inlineKeyboard = new InlineKeyboard();
            this.inlineState = inlineState;
        }

        /// <summary>
        /// Добавить строку
        /// </summary>
        public InlineButtonBuilder<FixedInlineKeyboardBuilder> AddRow
        {
            get
            {
                InlineKeyboardRow row = new InlineKeyboardRow();
                inlineKeyboard.AddRow(row);
                InlineButtonBuilder<FixedInlineKeyboardBuilder> buttonBuilder = new InlineButtonBuilder<FixedInlineKeyboardBuilder>(this, row);
                return buttonBuilder;
            }
        }

        /// <summary>
        /// Закончить клавиатуру и перейти к следующему состоянию
        /// </summary>
        public StatesBuilder FinishKeyboard
        {
            get
            {
                inlineState.SetKeyboard(inlineKeyboard);
                return statesBuilder;
            }
        }
    }
}
