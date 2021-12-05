using Newtonsoft.Json;

namespace TelegramBotConstructor
{
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
}
