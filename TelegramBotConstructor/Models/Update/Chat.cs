using Newtonsoft.Json;


namespace TelegramBotConstructor
{
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
