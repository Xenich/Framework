using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor.Keyboards.Buttons
{
    internal class ReplyKeyboardButton
    {
        internal readonly string Text;
        internal readonly bool RequestContact = false;
        internal readonly bool RequestLocation = false;

        internal ReplyKeyboardButton(string text, bool requestContact, bool requestLocation)
        {
            Text = text;
            RequestContact = requestContact;
            RequestLocation = requestLocation;
        }
    }
}
