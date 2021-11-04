using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor
{
    public class Response
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }
        [JsonProperty("result")]
        public List<Update> Updates { get; set; }
    }

    /// <summary>
    /// This object represents an incoming update from Telegram API.
    /// </summary>
    public class Update
    {
        [JsonProperty("update_id")]
        public int UpdateId { get; set; }
        //public string message_id { get; set; }
        [JsonProperty("message")]
        public Message Message { get; set; }
        [JsonProperty("callback_query")]
        public CallbackQuery CallbackQuery { get; set; }

        public int GetChatId()
        {
            if (IsCallbackQueryMessage())
                return CallbackQuery.From.Id;
            else
                return Message.From.Id;
        }

        public string GetMessageText()
        {
            if (Message != null)
            {
                return Message.Text;
            }
            else
                return "";
        }

        /// <summary>
        /// Метод показывает, что это сообщение из Inline-клавиатуры
        /// </summary>
        /// <returns></returns>
        public bool IsCallbackQueryMessage()
        {
            return CallbackQuery != null;
        }

        public string GetCallbackQueryId()
        {
            if (IsCallbackQueryMessage())
                return CallbackQuery.Id;
            else return "";
        }

        public int GetCallbackQueryMessageId()
        {
            if (IsCallbackQueryMessage())
                return CallbackQuery.Message.MessageId;
            else return 0;
        }

        public int GetMessageId()
        {
            if (IsCallbackQueryMessage())
                return CallbackQuery.Message.MessageId;
            if (Message != null)
                return Message.MessageId;
            else return 0;
        }

        /// <summary>
        /// Метод показывает, что это простое сообщение, введенное пользователем
        /// </summary>
        /// <returns></returns>
        public bool IsSimpleMessage()
        {
            if (Message != null && (Message.Photo == null ? true : Message.Photo.Length == 0))
                return true;
            else
                return false;
        }

        public bool IsPhotoMessage()
        {
            if (Message != null && Message.Photo != null && Message.Photo.Length > 0)
                return true;
            else 
                return false;
        }
    }

    /// <summary>
    /// This object represents an incoming callback query from a callback button in an inline keyboard
    /// </summary>
    public class CallbackQuery
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("from")]
        public From From { get; set; }
        [JsonProperty("message")]
        public Message Message { get; set; }
        [JsonProperty("chat_instance")]
        public string ChatInstance { get; set; }
        [JsonProperty("data")]
        public string Data { get; set; }
    }
    public class Message
    {
        [JsonProperty("message_id")]
        public int MessageId { get; set; }
        [JsonProperty("from")]
        public From From { get; set; }
        [JsonProperty("chat")]
        public Chat Chat { get; set; }
        [JsonProperty("date")]
        public int Date { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("photo")]
        public Photo[] Photo { get; set; }
        //public string reply_markup { get; set; }
    }

    public class From
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        //public bool is_bot { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("language_code")]
        public string LanguageCode { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public class Chat
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Photo
    {
        [JsonProperty("file_id")]
        public string FileId { get; set; }
        [JsonProperty("file_unique_id")]
        public string FileUniqueId { get; set; }
        [JsonProperty("file_size")]
        public long FileSize { get; set; }
        [JsonProperty("width")]
        public long Width { get; set; }
        [JsonProperty("height")]
        public long Height { get; set; }
    }


    //public class From2
    //{
    //    public int id { get; set; }
    //    public bool is_bot { get; set; }
    //    public string first_name { get; set; }
    //    public string language_code { get; set; }
    //}

    //public class From3
    //{
    //    public int id { get; set; }
    //    public bool is_bot { get; set; }
    //    public string first_name { get; set; }
    //    public string username { get; set; }
    //}

    //public class Chat2
    //{
    //    public int id { get; set; }
    //    public string first_name { get; set; }
    //    public string type { get; set; }
    //}

    //public class Entity
    //{
    //    public int offset { get; set; }
    //    public int length { get; set; }
    //    public string type { get; set; }
    //}

    //public class ReplyMarkup
    //{
    //    //public List<List<>> inline_keyboard { get; set; }
    //}


    //public class Message2
    //{
    //    public int message_id { get; set; }
    //    public From from { get; set; }
    //    public Chat chat { get; set; }
    //    public int date { get; set; }
    //    public string text { get; set; }
    //    public List<Entity> entities { get; set; }
    //   // public ReplyMarkup reply_markup { get; set; }
    //}
}
