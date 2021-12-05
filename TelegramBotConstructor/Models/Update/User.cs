using Newtonsoft.Json;


namespace TelegramBotConstructor
{
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
}
