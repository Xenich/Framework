using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.Keyboards.Buttons;

namespace TelegramBotConstructor.StatesBuilders
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="K">inlineKeyboardBuilder</typeparam>
    public class InlineButtonBuilder<K>
    {
        private readonly InlineKeyboardRow currentRow;
        private readonly K inlineKeyboardBuilder;

        internal InlineButtonBuilder(K inlineKeyboardBuilder, InlineKeyboardRow currentRow)
        {
            this.currentRow = currentRow;
            this.inlineKeyboardBuilder = inlineKeyboardBuilder;

        }

        /// <summary>
        /// Добавляет кнопку к inline-клавиатуре.
        /// Идентификатор состояния, вызываемого данной кнопкой, записывается в первые 32 байта callback_data и используется при стандартном определении состояния конечного автомата. 
        /// </summary>
        /// <param name="gotoStateUid">Идентификатор состояния конечного автомата, которое вызывается данной кнопкой. Записывается в первые 32 байта объекта callback_data</param>
        /// <param name="text">Label text on the button</param>
        /// <param name="callback_data_33_64">Coding ASCII. Последние 32 байта служебной информации объекта callback_data. 
        /// callback_data - data to be sent in a callback query to the bot when button is pressed, 1-64 bytes</param>
        public InlineButtonBuilder<K> AddButtonWithStateInCallbackData(Guid gotoStateUid, string text, string callback_data_33_64 = "")
        {
            string callback_1_32 = gotoStateUid.ToString("N");
            InlineKeyboardButton button = new InlineKeyboardButton(text, callback_1_32 + callback_data_33_64);
            currentRow.AddButton(button);
            return this;
        }

        /// <summary>
        /// Добавляет кнопку к inline-клавиатуре. 
        /// Вызываемое данной кнопкой состояние должно определяться в методе InlineMessageResolve реализации интерфейса IStateResolver 
        /// </summary>
        /// <param name="text">Label text on the button</param>
        /// <param name="callback_data">Coding ASCII. Data to be sent in a callback query to the bot when button is pressed, 1-64 bytes</param>
        public InlineButtonBuilder<K> AddButton(string text, string callback_data)
        {
            InlineKeyboardButton button = new InlineKeyboardButton(text, callback_data);
            currentRow.AddButton(button);
            return this;
        }

        public InlineButtonBuilder<K> AddURLButton(string url, string text)
        {
            InlineKeyboardButton button = new InlineKeyboardButton(url, text, true);
            currentRow.AddButton(button);
            return this;
        }

        /// <summary>
        /// Закончить строку
        /// </summary>
        public K FinishRow
        {
            get
            {
                return  inlineKeyboardBuilder;
            }
        }










    }
}
