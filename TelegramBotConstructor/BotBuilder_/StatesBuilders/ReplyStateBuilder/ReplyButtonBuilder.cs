using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.Keyboards.Buttons;

namespace TelegramBotConstructor.StatesBuilders
{
    public class ReplyButtonBuilder
    {
        private readonly ReplyKeyboardRow currentRow;
        readonly ReplyKeyboardBuilder keyboardBuilder;
        internal ReplyButtonBuilder(ReplyKeyboardBuilder replyKeyboardBuilder, ReplyKeyboardRow currentRow)
        {
            this.currentRow = currentRow;
            this.keyboardBuilder = replyKeyboardBuilder;
        }

        public ReplyButtonBuilder AddButton(string text)
        {
            ReplyKeyboardButton button = new ReplyKeyboardButton(text, false, false);
            currentRow.AddButton(button);
            return this;
        }

        public ReplyButtonBuilder AddRequestContactButton(string text)
        {
            ReplyKeyboardButton button = new ReplyKeyboardButton(text, true, false);
            currentRow.AddButton(button);
            return this;
        }

        public ReplyButtonBuilder AddRequestLocationButton(string text)
        {
            ReplyKeyboardButton button = new ReplyKeyboardButton(text, false, true);
            currentRow.AddButton(button);
            return this;
        }

        public ReplyKeyboardBuilder FinishRow
        {
            get
            {
                return keyboardBuilder;
            }
        }
    }
}
