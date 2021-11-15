using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.StateHandlers
{
    internal class InlineKeyboardStateHandler : BaseKeyboardStateHandler
    {
        internal InlineKeyboardStateHandler(Action<Update> handler, Func<Update, string> getMessage, string botToken, KeyboardState keyboardState)
            : base(handler, getMessage, botToken, keyboardState)
        { }

        protected override void SendConcreteKeyboard(Update update)
        {
            IKeyboard keyboard = keyboardState.GetKeyboard(update);

            if (keyboardState.tryDeletePrevKeyboard == TryDeletePrevKeyboard.YES)
            {
                if (update.IsCallbackQueryMessage())
                {
                    BotHelper.EditMessageText(getMessage(update), update.GetChatId(), update.GetCallbackQueryMessageId(), keyboard, botToken);
                }
                else
                    BotHelper.SendMessageWithKeyboard(getMessage(update), update.GetChatId(), keyboard, botToken);
            }
            else
                BotHelper.SendMessageWithKeyboard(getMessage(update), update.GetChatId(), keyboard, botToken);
        }

        protected override async Task<string> SendConcreteKeyboardWithResponceAsync(Update update)
        {
            IKeyboard keyboard = keyboardState.GetKeyboard(update);

            if (keyboardState.tryDeletePrevKeyboard == TryDeletePrevKeyboard.YES)
            {
                if (update.IsCallbackQueryMessage())
                {
                    return await BotHelper.EditMessageTextWithResponceAsync(getMessage(update), update.GetChatId(), update.GetCallbackQueryMessageId(), keyboard, botToken);
                }
                else
                    return await BotHelper.SendMessageWithKeyboardWithResponceAsync(getMessage(update), update.GetChatId(), keyboard, botToken);
            }
            else
                return await BotHelper.SendMessageWithKeyboardWithResponceAsync(getMessage(update), update.GetChatId(), keyboard, botToken);
        }
    }
}
