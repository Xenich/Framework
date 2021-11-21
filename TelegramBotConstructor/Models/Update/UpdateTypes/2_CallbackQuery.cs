using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor
{
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
}
