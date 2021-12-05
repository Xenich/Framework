using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor
{
    public enum MessageTypes
    {
        Unknown = 0,
        Text,
        Photo,
        Animation,
        Audio,
        Document, 
        Video,
        Voice,
        Sticker,
        Contact,
        Location
    }

    public class Message
    {
        public MessageTypes Type{get; private set;} = MessageTypes.Unknown;

        private string text;
        private Photo[] photo;
        private object animation;
        private object audio;
        private object document;
        private object video;
        private object voice;
        private object sticker;
        private Contact contact;
        private Location location;

        [JsonProperty("message_id")]
        public int MessageId { get; set; }
        /// <summary>
        /// Optional. Sender, empty for messages sent to channels
        /// </summary>
        [JsonProperty("from")]        
        public User From { get; set; }
        [JsonProperty("chat")]
        public Chat Chat { get; set; }
        [JsonProperty("date")]
        public int Date { get; set; }

        #region MessageTypes
        /// <summary>
        /// Optional. For text messages, the actual UTF-8 text of the message, 0-4096 characters
        /// </summary>
        [JsonProperty("text")]
        public string Text 
        {
            get { return text; }
            internal set { text = value; Type = MessageTypes.Text; } 
        }

        /// <summary>
        /// Optional. Message is a photo, available sizes of the photo
        /// </summary>
        [JsonProperty("photo")]
        public Photo[] Photo 
        {
            get { return photo; }
            set { photo = value; Type = MessageTypes.Photo; }
        }

        /// <summary>
        /// Optional. Message is an animation, information about the animation. For backward compatibility, when this field is set, the document field will also be set
        /// </summary>
        [JsonProperty("animation")]
        public object Animation
        {
            get { return animation; }
            set { animation = value; Type = MessageTypes.Animation; }
        }
        
        /// <summary>
        /// Optional. Message is a voice message, information about the file
        /// </summary>
        [JsonProperty("voice")]
        public object Voice
        {
            get { return voice; }
            set { voice = value; Type = MessageTypes.Voice; }
        }

        /// <summary>
        /// Optional. Message is an audio file, information about the file
        /// </summary>
        [JsonProperty("audio")]
        public object Audio
        {
            get { return audio; }
            set { audio = value; Type = MessageTypes.Audio; }
        }

        /// <summary>
        /// Optional. Message is a general file, information about the file
        /// </summary>
        [JsonProperty("document")]
        public object Document
        {
            get { return document; }
            set 
            {
                if (this.animation != null)
                    Type = MessageTypes.Animation;      // For backward compatibility, when Animation field is set, the document field will also be set
                else
                {
                    document = value;
                    Type = MessageTypes.Document;
                }
            }
        }

        /// <summary>
        /// Optional. Message is a video, information about the video
        /// </summary>
        [JsonProperty("video")]
        public object Video
        {
            get { return video; }
            set { video = value; Type = MessageTypes.Video; }
        }
        

        /// <summary>
        /// Optional. Message is a sticker, information about the sticker
        /// </summary>
        [JsonProperty("sticker")]
        public object Sticker
        {
            get { return sticker; }
            set { sticker = value; Type = MessageTypes.Sticker; }
        }

        /// <summary>
        /// Optional. Message is a shared contact, information about the contact
        /// </summary>
        [JsonProperty("contact")]
        public Contact Contact
        {
            get { return contact; }
            set { contact = value; Type = MessageTypes.Contact; }
        }

        /// <summary>
        /// Optional. Message is a shared contact, information about the contact
        /// </summary>
        [JsonProperty("location")]
        public Location Location
        {
            get { return location; }
            set { location = value; Type = MessageTypes.Location; }
        }
        
        #endregion MessageTypes

        /// <summary>
        /// Optional. Caption for the animation, audio, document, photo, video or voice, 0-1024 characters
        /// </summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }

        //public string reply_markup { get; set; }
    }

}
