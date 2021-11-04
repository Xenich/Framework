using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.Keyboards.Buttons
{
    /*
    internal class Button
    {
        Guid gotoStateUid;

        internal Button(Guid gotoStateUid)
        {
            this.gotoStateUid = gotoStateUid;
        }
    }
    */
    internal class InlineKeyboardButton //: Button
    {
        internal readonly bool IsUrl;
        internal readonly string Text;
        internal readonly string CallbackData;
        internal readonly string Url;

        /// <summary>
        /// This object represents one button of an inline keyboard.
        /// </summary>
        /// <param name="gotoStateUid">Идентификатор состояния конечного автомата, которое вызывается данной кнопкой</param>
        /// <param name="text">Label text on the button</param>
        /// <param name="callback_data">Coding ASCII. Data to be sent in a callback query to the bot when button is pressed, 1-64 bytes</param>
        internal InlineKeyboardButton(string text, string callback_data) //: base(gotoStateUid)
        {
            IsUrl = false;
            if (Encoding.ASCII.GetBytes(callback_data).Length > 64)
                throw new Exception(string.Format("Ошибка при создании кнопки {0}. Параметр callback_data - не более 64 байт", text));
            Text = text;
            CallbackData = callback_data;
        }
        
        internal InlineKeyboardButton(string url, string text, bool isUrl) //: base(Guid.Empty)
        {
            IsUrl = true;
            Text = text;
            CallbackData = "";
            Url = url;
        }
    }
}
