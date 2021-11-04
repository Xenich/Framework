using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards.Buttons;

namespace TelegramBotConstructor.Keyboards
{

    internal class ReplyKeyboard : IKeyboard
    {
        List<ReplyKeyboardRow> rowList = new List<ReplyKeyboardRow>();

        /// <summary>
        /// Requests clients to hide the keyboard as soon as it's been used. The keyboard will still be available, but clients will automatically display the usual letter-keyboard in the chat – the user can press a special button in the input field to see the custom keyboard again.
        /// </summary>
        bool oneTimeKeyboard;

        /// <summary>
        /// Requests clients to resize the keyboard vertically for optimal fit. Defaults to false, in which case the custom keyboard is always of the same height as the app's standard keyboard.
        /// </summary>
        bool resizeKeyboard;

        /// <summary>
        /// The placeholder to be shown in the input field when the keyboard is active; 1-64 characters
        /// </summary>
        string inputFieldPlaceholder;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="inputFieldPlaceholder">The placeholder to be shown in the input field when the keyboard is active; 1-64 characters</param>
        /// <param name="oneTimeKeyboard">Requests clients to hide the keyboard as soon as it's been used. The keyboard will still be available, but clients will automatically display the usual letter-keyboard in the chat – the user can press a special button in the input field to see the custom keyboard again.</param>
        /// <param name="resizeKeyboard">Requests clients to resize the keyboard vertically for optimal fit. Defaults to false, in which case the custom keyboard is always of the same height as the app's standard keyboard.</param>
        internal ReplyKeyboard(string inputFieldPlaceholder, bool oneTimeKeyboard, bool resizeKeyboard)
        {            
            this.oneTimeKeyboard = oneTimeKeyboard;
            this.resizeKeyboard = resizeKeyboard;
            this.inputFieldPlaceholder = inputFieldPlaceholder;
        }

        internal ReplyKeyboard AddRow(ReplyKeyboardRow row)
        {
            rowList.Add(row);
            return this;
        }
        public string Generate()
        {
            List<object> _rowlist = new List<object>();
            Dictionary<string, object> maindic = new Dictionary<string, object>();
            maindic.Add("keyboard", _rowlist);
            foreach (ReplyKeyboardRow row in rowList)
            {
                List<object> buttonlist = new List<object>();
                foreach (ReplyKeyboardButton btn in row.buttonList)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("text", btn.Text);
                    if (btn.RequestContact)
                        dic.Add("request_contact", true);                    
                    if(btn.RequestLocation)
                        dic.Add("request_location", true);
                    buttonlist.Add(dic);
                }
                _rowlist.Add(buttonlist);
            }

            if(resizeKeyboard)
                maindic.Add("resize_keyboard", true);
            if(oneTimeKeyboard)
                maindic.Add("one_time_keyboard", true);
            if(!String.IsNullOrEmpty(inputFieldPlaceholder))
                maindic.Add("input_field_placeholder", inputFieldPlaceholder);

            string s = Newtonsoft.Json.JsonConvert.SerializeObject(maindic);
            return s;
        }
    }

    internal class ReplyKeyboardRow
    {
        internal List<ReplyKeyboardButton> buttonList = new List<ReplyKeyboardButton>();
        internal void AddButton(ReplyKeyboardButton button)
        {
            buttonList.Add(button);
        }
    }
}
