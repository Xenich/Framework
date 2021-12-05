using Newtonsoft.Json;


namespace TelegramBotConstructor
{
    public class Location
    {
        /// <summary>
        /// Longitude as defined by sender
        /// </summary>
        [JsonProperty("longitude")]
        public float Longitude;
        /// <summary>
        /// Latitude as defined by sender
        /// </summary>
        [JsonProperty("latitude")]
        public float Latitude;
        /// <summary>
        /// Optional.The radius of uncertainty for the location, measured in meters; 0-1500
        /// </summary>
        [JsonProperty("horizontal_accuracy")]
        public float HorizontalAccuracy;
        /// <summary>
        /// Optional.Time relative to the message sending date, during which the location can be updated; in seconds.For active live locations only.
        /// </summary>
        [JsonProperty("live_period")]
        public int LivePeriod;
        /// <summary>
        /// Optional.The direction in which user is moving, in degrees; 1-360. For active live locations only.
        /// </summary>
        [JsonProperty("heading")]
        public int Heading;
        /// <summary>
        ///  Optional.Maximum distance for proximity alerts about approaching another chat member, in meters.For sent live locations only.
        /// </summary>
        [JsonProperty("proximity_alert_radius")]
        public int ProximityAlertRadius;

    }
}
