using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor.IncomingTelegramUpdateHandlers
{
    abstract class IncomingTelegramUpdateHandler
    {
        protected readonly Bot bot;

        public IncomingTelegramUpdateHandler(Bot bot)
        {
            this.bot = bot;
        }

        internal abstract void AddUpdateToQueue(Update update);
    }
}
