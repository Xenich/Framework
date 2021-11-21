using JsonSubTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor
{
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
