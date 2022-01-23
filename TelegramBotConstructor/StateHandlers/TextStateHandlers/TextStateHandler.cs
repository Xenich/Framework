using System;
using System.Threading.Tasks;

namespace TelegramBotConstructor.StateHandlers
{
    internal class TextStateHandler : StateHandler
    {
        internal TextStateHandler(Action<Update> handler, Func<Update, string> getMessage, string botToken)
            : base(handler, getMessage, botToken)
        {
        }

        public override void HandleWithoutResponce(Update update)
        {
            CustomHandlerCall(update);
            //HideReplyKeyboard(update);
            CallbackQueryNotification(update);
            if (IsNeedTryHideReplyKeyboard)
                BotHelper.ReplyKeyboardRemove(getMessage(update), update.GetChatId(), botToken);
            else
            BotHelper.SendSimpleMessage(getMessage(update), update.GetChatId(), botToken);
        }

        public override async Task<string> HandleWithResponceAsync(Update update)
        {
            CustomHandlerCall(update);
            //HideReplyKeyboard(update);
            CallbackQueryNotification(update);
            if (IsNeedTryHideReplyKeyboard)
                return await BotHelper.ReplyKeyboardRemoveWithResponceAsync(getMessage(update), update.GetChatId(), botToken);
            else
                return await BotHelper.SendSimpleMessageWithResponceAsync(getMessage(update), update.GetChatId(), botToken);            
        }
    }
}
