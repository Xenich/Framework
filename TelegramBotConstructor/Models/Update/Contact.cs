using Newtonsoft.Json;

namespace TelegramBotConstructor
{
    /// <summary>
    /// This object represents a phone contact
    /// </summary>
    public class Contact
    {
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("user_id")]
        public int UserId { get; set; }
        [JsonProperty("vcard")]
        public string VCard { get; set; }
    }
}
