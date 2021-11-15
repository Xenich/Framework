using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor.MessageHandlers
{

    internal class ChatMemberUpdatedHandler : MessageHandler
    {
        internal ChatMemberUpdatedHandler(Update update, Bot bot)
            : base(update, bot)
        { }

        internal override async Task Handle()
        {
            if (update.ChatMemberUpdated.NewChatMember.Status == "kicked")
            {
                if (bot.BotKickedEventHandler != null)
                    bot.BotKickedEventHandler(update);
            }
            if (update.ChatMemberUpdated.NewChatMember.Status == "member")
            {
                if (bot.BotAddedEventHandler != null)
                    bot.BotAddedEventHandler(update);
            }

            return;
        }
    }
}
