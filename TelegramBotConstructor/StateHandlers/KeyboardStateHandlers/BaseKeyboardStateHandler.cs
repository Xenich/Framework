using System;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.StateHandlers
{
    internal abstract class BaseKeyboardStateHandler : StateHandler
    {
        protected readonly KeyboardState keyboardState;

        internal BaseKeyboardStateHandler(Action<Update> handler, Func<Update, string> getMessage, string botToken, KeyboardState keyboardState)
            : base(handler, getMessage, botToken)
        {
            this.keyboardState = keyboardState;
        }

        public override void HandleWithoutResponce(Update update)
        {
            CustomHandlerCall(update);
            //HideReplyKeyboard(update);
            CallbackQueryNotification(update);
            SendConcreteKeyboard(update);
        }

        public override async Task<string> HandleWithResponceAsync(Update update)
        {
            CustomHandlerCall(update);
            //HideReplyKeyboard(update);
            CallbackQueryNotification(update);
            return await SendConcreteKeyboardWithResponceAsync(update);
        }


        protected abstract void SendConcreteKeyboard(Update update);      

        protected abstract Task<string> SendConcreteKeyboardWithResponceAsync(Update update);
        
    }
}
