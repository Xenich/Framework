using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards.Buttons;

namespace TelegramBotConstructor.Keyboards
{
    internal class InlineKeyboard : IKeyboard
    {
        List<InlineKeyboardRow> rowList = new List<InlineKeyboardRow>();

        internal InlineKeyboard AddRow(InlineKeyboardRow row)
        {
            rowList.Add(row);
            return this;
        }

        public string Generate()
        {
            /*
            StringBuilder sb = new StringBuilder("{\"inline_keyboard\":[");
            foreach (InlineKeyboardRow row in rowList)
            {
                sb.Append("[");
                foreach (InlineKeyboardButton btn in row.buttonList)
                {
                    sb.Append("{\"text\":\"" + btn.text + "\", \"callback_data\":\"" + btn.callback_data + "\"");
                    if (!string.IsNullOrEmpty(btn.url))
                        sb.Append(",\"url\":\"" + btn.url + "\"");
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);          // удаляем ненужную запятую
                sb.Append("],");
            }
            sb.Remove(sb.Length - 1, 1);              // удаляем ненужную запятую
            sb.Append("]}");
            return sb.ToString(); ;
            */

            List<object> _rowlist = new List<object>();
            Dictionary<string, List<object>> maindic = new Dictionary<string, List<object>>();
            maindic.Add("inline_keyboard", _rowlist);
            foreach (InlineKeyboardRow row in rowList)
            {
                List<object> buttonlist = new List<object>();
                foreach (InlineKeyboardButton btn in row.buttonList)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    if (btn.IsUrl)
                    {
                        if (!string.IsNullOrEmpty(btn.Url))
                        {
                            dic.Add("text", btn.Text);
                            dic.Add("url", btn.Url);
                        }
                    }
                    else
                    {
                        dic.Add("text", btn.Text);
                        dic.Add("callback_data", btn.CallbackData);
                    }
                    buttonlist.Add(dic);
                }
                _rowlist.Add(buttonlist);
            }

            string s = Newtonsoft.Json.JsonConvert.SerializeObject(maindic);
            return s;
        }
    }

    internal class InlineKeyboardRow
    {
        internal List<InlineKeyboardButton> buttonList = new List<InlineKeyboardButton>();
        internal void AddButton(InlineKeyboardButton button)
        {
            buttonList.Add(button);
        }
    }
}
