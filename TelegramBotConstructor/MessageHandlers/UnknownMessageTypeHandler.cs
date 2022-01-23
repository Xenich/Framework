using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor.MessageHandlers
{
    internal class UnknownMessageTypeHandler : MessageHandler
    {
        internal UnknownMessageTypeHandler(Update update, Bot bot)
            : base(update, bot)
        { }
        internal override async Task Handle()
        {
            bot.Log(string.Format("UnknownMessageHandler: update_id = {0}", update.UpdateId));
            return;
        }
    }
}
