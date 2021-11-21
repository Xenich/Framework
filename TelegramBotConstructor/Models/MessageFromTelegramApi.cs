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



   






}
