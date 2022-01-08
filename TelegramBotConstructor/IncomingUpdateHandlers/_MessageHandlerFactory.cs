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
            MessageHandler messageHandler = null;
            switch (update.Type)
            {
                case UpdateTypes.Unknown:
                    messageHandler = new UnknownMessageTypeHandler(update, bot);        // сообщение неизвестного типа
                    break;
                case UpdateTypes.Message:
                    messageHandler = new MainHandler(update, bot);                      // основной обработчик входящего сообщения
                    break;
                case UpdateTypes.EditedMessage:                                         // редактированное сообщение
                    messageHandler = new EditedMessageHandler(update, bot);
                    break;
                case UpdateTypes.ChannelPost:
                    messageHandler = new UnknownMessageTypeHandler(update, bot);
                    break;
                case UpdateTypes.EditedChannelPost:
                    messageHandler = new UnknownMessageTypeHandler(update, bot);
                    break;
                case UpdateTypes.InlineQuery:
                    messageHandler = new UnknownMessageTypeHandler(update, bot);
                    break;
                case UpdateTypes.ChosenInlineResult:
                    messageHandler = new UnknownMessageTypeHandler(update, bot);
                    break;
                case UpdateTypes.CallbackQuery:                                         // нажатие на кнопку inline-клавиатуры
                    messageHandler = new MainHandler(update, bot);
                    break;
                case UpdateTypes.ShippingQuery:
                    messageHandler = new UnknownMessageTypeHandler(update, bot);
                    break;
                case UpdateTypes.PreCheckoutQuery:
                    messageHandler = new UnknownMessageTypeHandler(update, bot);
                    break;
                case UpdateTypes.Poll:
                    messageHandler = new UnknownMessageTypeHandler(update, bot);
                    break;
                case UpdateTypes.PollAnswer:
                    messageHandler = new UnknownMessageTypeHandler(update, bot);
                    break;
                case UpdateTypes.MyChatMember:           // событие изменения статуса бота (напр. юзер добавил бота или удалил)
                    messageHandler = new ChatMemberUpdatedHandler(update, bot);
                    break;
                case UpdateTypes.ChatMember:
                    messageHandler = new UnknownMessageTypeHandler(update, bot);
                    break;
                case UpdateTypes.ChatJoinRequest:
                    messageHandler = new UnknownMessageTypeHandler(update, bot);
                    break;
            }
            return messageHandler;
        }
    }
}
