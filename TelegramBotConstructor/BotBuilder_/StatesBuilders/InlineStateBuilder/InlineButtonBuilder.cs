using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.Keyboards.Buttons;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.StatesBuilders
{  
    public class InlineButtonBuilder
    {
        private readonly InlineKeyboardRow currentRow;
        readonly FixedInlineKeyboardBuilder keyboardBuilder;
        internal InlineButtonBuilder(FixedInlineKeyboardBuilder inlineKeyboardBuilder, InlineKeyboardRow currentRow)
        {
            this.currentRow = currentRow;
            this.keyboardBuilder = inlineKeyboardBuilder;
        }

        /// <summary>
        /// Добавляет one button of an inline keyboard.
        /// </summary>
        /// <param name="gotoStateUid">Идентификатор состояния конечного автомата, которое вызывается данной кнопкой</param>
        /// <param name="text">Label text on the button</param>
        /// <param name="callback_data">Coding ASCII. Data to be sent in a callback query to the bot when button is pressed, 1-64 bytes</param>
        public InlineButtonBuilder AddButton(Guid gotoStateUid, string text, string callback_data)
        {
            InlineKeyboardButton button = new InlineKeyboardButton( text, callback_data);
            currentRow.AddButton(button);
            return this;
        }

        public InlineButtonBuilder AddURLButton(string url, string text)
        {
            InlineKeyboardButton button = new InlineKeyboardButton(url, text, true);
            currentRow.AddButton(button);
            return this;
        }

        public FixedInlineKeyboardBuilder FinishRow
        {
            get
            {
                return keyboardBuilder;
            }
        }
    }
}
