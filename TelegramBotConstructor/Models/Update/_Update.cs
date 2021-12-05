using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor
{
    public enum UpdateTypes
    {
        Unknown = 0,
        Message,
        EditedMessage,
        ChannelPost,
        EditedChannelPost,
        InlineQuery,
        ChosenInlineResult,
        CallbackQuery,
        ShippingQuery,
        PreCheckoutQuery,
        Poll,
        PollAnswer,
        MyChatMember,
        ChatMember,
        ChatJoinRequest
    }

    /// <summary>
    /// This object represents an incoming update from Telegram API.
    /// </summary>
    public class Update
    {
        public  UpdateTypes Type { get; private set; } = UpdateTypes.Unknown; 

        [JsonProperty("update_id")]
        public int UpdateId { get; set; }


        private Message message;
        private CallbackQuery callbackQuery;
        private ChatMemberUpdated chatMemberUpdated;
        private Message editedMessage;

        /// <summary>
        /// New incoming message of any kind — text, photo, sticker, etc.
        /// </summary>
        [JsonProperty("message")]
        public Message Message
        {
            get { return message; }
            set { message = value; Type = UpdateTypes.Message; }
        }

        /// <summary>
        /// New incoming callback query from a callback button in an inline keyboard
        /// </summary>
        [JsonProperty("callback_query")]
        public CallbackQuery CallbackQuery
        {
            get { return callbackQuery; }
            set { callbackQuery = value; Type = UpdateTypes.CallbackQuery; }
        }

        /// <summary>
        /// Optional. The bot's chat member status was updated in a chat. 
        /// For private chats, this update is received only when the bot is blocked or unblocked by the user.
        /// </summary>
        [JsonProperty("my_chat_member")]
        public ChatMemberUpdated MyChatMember
        {
            get { return chatMemberUpdated; }
            set { chatMemberUpdated = value; Type = UpdateTypes.MyChatMember; }
        }

        /// <summary>
        /// New version of a message that is known to the bot and was edited
        /// </summary>
        [JsonProperty("edited_message")]
        public Message EditedMessage
        {
            get { return editedMessage; }
            set { editedMessage = value; Type = UpdateTypes.EditedMessage; }
        }


//**********************************************************************************
        public int GetChatId()
        {
            switch (this.Type)
            {
                case UpdateTypes.CallbackQuery:
                    return CallbackQuery.From.Id;
                case UpdateTypes.MyChatMember:
                    return MyChatMember.From.Id;
                case UpdateTypes.Message:
                    if (Message.From != null)
                        return Message.From.Id;
                    else
                        return 0;
                default:
                    return 0;
            }
        }

        public string GetMessageText()
        {
            if (this.Type == UpdateTypes.Message)
            {
                if (message.Type == MessageTypes.Text)
                    return message.Text;
                if (message.Type == MessageTypes.Animation ||
                    message.Type == MessageTypes.Audio ||
                    message.Type == MessageTypes.Document ||
                    message.Type == MessageTypes.Photo ||
                    message.Type == MessageTypes.Video ||
                    message.Type == MessageTypes.Voice)
                    return message.Caption;
            }
            return "";
        }

        public int GetCallbackQueryMessageId()
        {
            if (Type == UpdateTypes.CallbackQuery)
                return CallbackQuery.Message.MessageId;
            else return 0;
        }

        public int GetMessageId()
        {
            switch (this.Type)
            {
                case (UpdateTypes.Unknown):
                    return 0;
                case (UpdateTypes.Message):
                    return Message.MessageId;
                case (UpdateTypes.EditedMessage):
                    return Message.MessageId;
                case (UpdateTypes.ChannelPost):
                    return 0;
                case (UpdateTypes.EditedChannelPost):
                    return 0;
                case (UpdateTypes.InlineQuery):
                    return 0;
                case (UpdateTypes.ChosenInlineResult):
                    return 0;
                case (UpdateTypes.CallbackQuery):
                    return CallbackQuery.Message.MessageId;
                case (UpdateTypes.ShippingQuery):
                    return 0;
                case (UpdateTypes.PreCheckoutQuery):
                    return 0;
                case (UpdateTypes.Poll):
                    return 0;
                case (UpdateTypes.PollAnswer):
                    return 0;
                case (UpdateTypes.MyChatMember):
                    return 0;
                case (UpdateTypes.ChatMember):
                    return 0;
                case (UpdateTypes.ChatJoinRequest):
                    return 0;

                default: return 0;
            }
        }
    }
}
