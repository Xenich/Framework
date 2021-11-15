using JsonSubTypes;
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
        /// <summary>
        /// Optional. The bot's chat member status was updated in a chat. For private chats, this update is received only when the bot is blocked or unblocked by the user.
        /// </summary>
        [JsonProperty("my_chat_member")]
        public ChatMemberUpdated ChatMemberUpdated { get; set; }

        public int GetChatId()
        {
            if (IsCallbackQueryMessage())
                return CallbackQuery.From.Id;
            if (IsChatMemberUpdated())
                return ChatMemberUpdated.From.Id;
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

        /// <summary>
        /// Метод показывает был ли изменен статус бота в чате. Для приватных чатов это происходит только тогда, когда бот блокируется или разблокируется юзером
        /// </summary>
        /// <returns></returns>
        public bool IsChatMemberUpdated()
        {
            return ChatMemberUpdated != null;
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
        public User From { get; set; }
        [JsonProperty("message")]
        public Message Message { get; set; }
        [JsonProperty("chat_instance")]
        public string ChatInstance { get; set; }
        /// <summary>
        /// Data associated with the callback button
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }
    }

    public class Message
    {
        [JsonProperty("message_id")]
        public int MessageId { get; set; }
        [JsonProperty("from")]
        public User From { get; set; }
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

    public class User
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

    /// <summary>
    /// This object represents changes in the status of a chat member.
    /// </summary>
    public class ChatMemberUpdated
    {
        /// <summary>
        /// Chat the user belongs to
        /// </summary>
        [JsonProperty("chat")]       
        public Chat Chat { get; set; }
        /// <summary>
        /// Performer of the action, which resulted in the change
        /// </summary>
        [JsonProperty("from")]
        public User From { get; set; }
        /// <summary>
        /// Date the request was sent in Unix time
        /// </summary>
        [JsonProperty("date")]
        public int Date { get; set; }
        /// <summary>
        /// Previous information about the chat member
        /// </summary>
        [JsonProperty("old_chat_member")]
        public ChatMember OldChatMember { get; set; }
        /// <summary>
        /// New information about the chat member
        /// </summary>
        [JsonProperty("new_chat_member")]
        public ChatMember NewChatMember { get; set; }
    }

    /// <summary>
    /// This object contains information about one member of a chat.
    /// </summary>
    [JsonConverter(typeof(JsonSubtypes), "Status")]
    [JsonSubtypes.KnownSubType(typeof(ChatMemberOwner), "creator")]
    [JsonSubtypes.KnownSubType(typeof(ChatMemberAdministrator), "administrator")]
    [JsonSubtypes.KnownSubType(typeof(ChatMemberMember), "member")]
    [JsonSubtypes.KnownSubType(typeof(ChatMemberRestricted), "restricted")]
    [JsonSubtypes.KnownSubType(typeof(ChatMemberLeft), "left")]
    [JsonSubtypes.KnownSubType(typeof(ChatMemberBanned), "kicked")]

    public class ChatMember
    {
        public string Status { get; set; }
        public User User { get; set; }

    }

    public class ChatMemberOwner : ChatMember 
    {

    }

    public class ChatMemberAdministrator : ChatMember
    {

    }

    public class ChatMemberMember : ChatMember
    {

    }

    public class ChatMemberRestricted : ChatMember
    {

    }

    public class ChatMemberLeft : ChatMember
    {
    }

    public class ChatMemberBanned : ChatMember
    {

    }






}
