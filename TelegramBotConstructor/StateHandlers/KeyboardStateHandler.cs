using System;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.StateHandlers
{
    internal class KeyboardStateHandler : StateHandler
    {
        private readonly KeyboardState inlineState;

        internal KeyboardStateHandler(Action<Update> handler, Func<Update, string> getMessage, string botToken, KeyboardState inlineState)
            : base(handler, getMessage, botToken)
        {
            this.inlineState = inlineState;
        }

        public override void HandleWithoutResponce(Update update)
        {
            CustomHandlerCall(update);
            HideReplyKeyboard(update);
            CallbackQueryNotification(update);
            SendConcreteKeyboard(update);
        }

        public override async Task<string> HandleWithResponceAsync(Update update)
        {
            CustomHandlerCall(update);
            HideReplyKeyboard(update);
            CallbackQueryNotification(update);
            return await SendConcreteKeyboardWithResponceAsync(update);
        }



        protected void SendConcreteKeyboard(Update update)
        {
            IKeyboard keyboard = inlineState.GetKeyboard(update);

            if (inlineState.tryDeletePrevKeyboard == TryDeletePrevKeyboard.YES)
            {
                if (update.IsCallbackQueryMessage())
                    BotHelper.EditMessageText(getMessage(update), update.GetChatId(), update.GetCallbackQueryMessageId(), keyboard, botToken);
                else
                    BotHelper.SendMessageWithKeyboard(getMessage(update), update.GetChatId(), keyboard, botToken);
            }
            else
                BotHelper.SendMessageWithKeyboard(getMessage(update), update.GetChatId(), keyboard, botToken);
        }

        protected async Task<string> SendConcreteKeyboardWithResponceAsync(Update update)
        {
            IKeyboard keyboard = inlineState.GetKeyboard(update);

            if (inlineState.tryDeletePrevKeyboard == TryDeletePrevKeyboard.YES)
            {
                if (update.IsCallbackQueryMessage())
                    return await BotHelper.EditMessageTextWithResponceAsync(getMessage(update), update.GetChatId(), update.GetCallbackQueryMessageId(), keyboard, botToken);
                else
                    return await BotHelper.SendMessageWithKeyboardWithResponceAsync(getMessage(update), update.GetChatId(), keyboard, botToken);
            }
            else
                return await BotHelper.SendMessageWithKeyboardWithResponceAsync(getMessage(update), update.GetChatId(), keyboard, botToken);
        }
    }
}
