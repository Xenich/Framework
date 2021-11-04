using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards;

namespace TelegramBotConstructor
{
    internal static class BotHelper
    {

        internal static void SendMessageWithKeyboard(string message, int chatID, IKeyboard keyboard, string botToken)
        {
            Dictionary<string, string> messageDict = new Dictionary<string, string>();
            messageDict.Add("text", message);
            messageDict.Add("chat_id", chatID.ToString());
            messageDict.Add("parse_mode", "Markdown");
            messageDict.Add("reply_markup", keyboard.Generate());           
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(messageDict);
            Send("sendMessage", json, botToken);
        }

        internal static async Task<string> SendMessageWithKeyboardWithResponceAsync(string message, int chatID, IKeyboard keyboard, string botToken)
        {
            Dictionary<string, string> messageDict = new Dictionary<string, string>();
            messageDict.Add("text", message);
            messageDict.Add("chat_id", chatID.ToString());
            messageDict.Add("parse_mode", "Markdown");
            messageDict.Add("reply_markup", keyboard.Generate());
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(messageDict);
            string responce = await SendWithResponseAsync("sendMessage", json, botToken);
            return responce;
        }

        /// <summary>
        /// Редактировать сообщение по его Id (по сути удалить сообщение у пользователя и подменить его другим - сообщением с inline-клавиатурой)
        /// </summary>
        /// <param name="message">Сообщение над клавиатурой</param>
        /// <param name="chatID"></param>
        /// <param name="message_id">Id сообщения, которое редактирутся</param>
        /// <param name="keyboard">Клавиатура</param>
        /// <param name="botToken">Токен бота</param>
        internal static void EditMessageText(string message, int chatID, int message_id, IKeyboard keyboard, string botToken)
        {
            Dictionary<string, string> messageDict = new Dictionary<string, string>()
            {
                { "text", message },
                { "chat_id", chatID.ToString() },
                { "message_id", message_id.ToString() },
                { "parse_mode", "Markdown" },
                { "reply_markup", keyboard.Generate() }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(messageDict);            
            Send("editMessageText", json, botToken);    
        }

        internal static async Task<string> EditMessageTextWithResponceAsync(string message, int chatID, int message_id, IKeyboard keyboard, string botToken)
        {
            Dictionary<string, string> messageDict = new Dictionary<string, string>()
            {
                { "text", message },
                { "chat_id", chatID.ToString() },
                { "message_id", message_id.ToString() },
                { "parse_mode", "Markdown" },
                { "reply_markup", keyboard.Generate() }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(messageDict);
            string responce = await SendWithResponseAsync("editMessageText", json, botToken);
            return responce;
        }

        /// <summary>
        /// Редактировать сообщение по его Id (т.е. удалить сообщение у пользователя и подменить его другим - текстовым)
        /// </summary>
        /// <param name="message">Сообщение над клавиатурой</param>
        /// <param name="chatID"></param>
        /// <param name="message_id">Id сообщения, которое редактирутся</param>
        /// <param name="botToken">Токен бота</param>
        internal static void EditMessageTextWithSimpleText(string message, int chatID, int message_id, string botToken)
        {
            Dictionary<string, string> messageDict = new Dictionary<string, string>()
            {
                { "text", message },
                { "chat_id", chatID.ToString() },
                { "message_id", message_id.ToString() }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(messageDict);
            Send("editMessageText", json, botToken);
        }

        /// <summary>
        /// Удаление сообщения по его message_id
        /// </summary>
        /// <param name="message_id"></param>
        /// <param name="chat_id"></param>
        /// <param name="botToken"></param>
        public static void DeleteMessage(int message_id, int chat_id, string botToken)
        {
            Dictionary<string, string> messageDict = new Dictionary<string, string>()
            {
                { "message_id", message_id.ToString() },
                { "chat_id", chat_id.ToString()  }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(messageDict);
            Send("deleteMessage", json, botToken);
        }

        /// <summary>
        /// Отправление сообщения в чат без клавиатуры
        /// </summary>
        /// <param name="message">Сообщение пользователю</param>
        /// <param name="chatID">Идентификатор чата</param>
        /// <param name="botToken">токен бота</param>
        internal static void SendSimpleMessage(string message, int chatID, string botToken)
        {
            Dictionary<string, string> messageDict = new Dictionary<string, string>()
            {
                { "text", message },
                { "chat_id", chatID.ToString() }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(messageDict);
            Send("sendMessage", json, botToken);
        }

        internal static async Task<string> SendSimpleMessageWithResponceAsync(string message, int chatID, string botToken)
        {
            Dictionary<string, string> messageDict = new Dictionary<string, string>()
            {
                { "text", message },
                { "chat_id", chatID.ToString() }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(messageDict);
            string responce = await SendWithResponseAsync("sendMessage", json, botToken);
            return responce;
        }

        /// <summary>
        /// Всплывающее сообщение при нажатии пользователем книпки inline-клавиатуры
        /// </summary>
        /// <param name="message">Сообщение, которое будет всплывать у пользователя</param>
        /// <param name="callbackQueryId">callback_query_id который пришёл в запросе после нажатия пользователем кнопки inline-клавиатуры</param>
        internal static void AnswerCallbackQuery(string message, string callbackQueryId, string botToken)
        {
            Dictionary<string, string> messageDict = new Dictionary<string, string>()
            {
                { "text", message },
                { "callback_query_id", callbackQueryId }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(messageDict);
            Send("answerCallbackQuery", json, botToken);
        }

        internal static void ReplyKeyboardRemove(string message, int chat_id, string botToken)
        {
            Dictionary<string, string> messageDict = new Dictionary<string, string>()
            {
                { "text", message },
                { "chat_id", chat_id.ToString() },
                { "reply_markup", "{ \"remove_keyboard\":true}" }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(messageDict);
            Send("sendMessage", json, botToken);
        }

        internal static async Task<string> ReplyKeyboardRemoveWithResponceAsync(string message, int chat_id, string botToken)
        {
            Dictionary<string, string> messageDict = new Dictionary<string, string>()
            {
                { "text", message },
                { "chat_id", chat_id.ToString() },
                { "reply_markup", "{ \"remove_keyboard\":true}" }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(messageDict);
            string responce = await SendWithResponseAsync("sendMessage", json, botToken);
            return responce;
        }

        #region PRIVATE

        private static void Send(string method, string messageBody, string botToken)
        {
            string uri = "https://api.telegram.org/bot" + botToken + "/" + method;
            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", messageBody, ParameterType.RequestBody);
            client.ExecuteAsync(request);
        }

        private static async Task<string> SendWithResponseAsync(string method, string messageBody, string botToken)
        {
            string uri = "https://api.telegram.org/bot" + botToken + "/" + method;
            var client = new RestClient(uri);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", messageBody, ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteAsync(request);
            string ret = response.Content;
            return ret;
        }

#endregion PRIVATE

    }
}
