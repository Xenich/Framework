using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.MessageHandlers
{
    internal class MessageHandlerFactory
    {
        internal static MessageHandler CreateHandler(Update update, Bot bot)
        {
            if (update.IsChatMemberUpdated())           // событие изменения статуса бота (напр. юзер добавил бота или удалил)
                return new ChatMemberUpdatedHandler(update, bot);
            else
                return new MainHandler(update, bot);    // основной обработчик входящего сообщения
        }
    }

    internal abstract class MessageHandler
    {
        protected Bot bot;
        protected Update update;

        internal MessageHandler(Update update, Bot bot)
        {            
            this.update = update;
            this.bot = bot;
        }

        internal abstract Task Handle();   
    }
}
