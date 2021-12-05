using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor
{
    class ResponceFromTelegram
    {
        public bool Ok { get; set; }
        public Result Result { get; set; }
    }

    class Result
    { 
        [JsonProperty("message_id")]
        public int MessageId { get; set; }
    }
}
