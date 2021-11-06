using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.Keyboards.Buttons;

namespace TelegramBotConstructor.StatesBuilders
{
    public class ReplyButtonBuilder<K>
    {
        private readonly ReplyKeyboardRow currentRow;
        readonly K keyboardBuilder;
        internal ReplyButtonBuilder(K replyKeyboardBuilder, ReplyKeyboardRow currentRow)
        {
            this.currentRow = currentRow;
            this.keyboardBuilder = replyKeyboardBuilder;
        }

        public ReplyButtonBuilder<K> AddButton(string text)
        {
            ReplyKeyboardButton button = new ReplyKeyboardButton(text, false, false);
            currentRow.AddButton(button);
            return this;
        }

        public ReplyButtonBuilder<K> AddRequestContactButton(string text)
        {
            ReplyKeyboardButton button = new ReplyKeyboardButton(text, true, false);
            currentRow.AddButton(button);
            return this;
        }

        public ReplyButtonBuilder<K> AddRequestLocationButton(string text)
        {
            ReplyKeyboardButton button = new ReplyKeyboardButton(text, false, true);
            currentRow.AddButton(button);
            return this;
        }

        public K FinishRow
        {
            get
            {
                return keyboardBuilder;
            }
        }
    }
}
