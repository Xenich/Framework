using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor.MessageHandlers
{
    internal abstract class MessageHandler
    {
        protected Bot bot;
        protected Update update;

        protected int chatId;

        internal MessageHandler(Update update, Bot bot)
        {
            this.update = update;
            this.bot = bot;
            chatId = update.GetChatId();
        }

        internal abstract Task Handle();
    }
}
