using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Iterfaces;

namespace TelegramBotConstructor.StateHandlers
{
    internal abstract class StateHandler : IStateHandler
    {
        protected Action<Update> _handler;
        protected Func<Update, string> getMessage;
        protected string botToken;

        protected StateHandler(Action<Update> handler, Func<Update, string> getMessage, string botToken)
        {
            _handler = handler;
            this.getMessage = getMessage;
            this.botToken = botToken;
        }

        /// <summary>
        /// Текст, посылаемый юзеру в случае, если на это состояние он попал после нажатия кнопки inline-клавиатуры И если isNeedCallbackQueryNotification == true
        /// </summary>
        internal string CallbackQueryNotificationText { get; set; }

        /// <summary>
        /// Нужно ли отправлять ответы на запросы обратного вызова, отправленные с inline-клавиатуры
        /// </summary>
        internal bool IsNeedCallbackQueryNotification { get; set; }

        /// <summary>
        /// Нужно ли пытаться спрятать reply-клавиатуру (в случае, если предполагается ппадние на этот state после reply-клавиатуры )
        /// </summary>
        internal bool IsNeedTryHideReplyKeyboard { get; set; }

        public abstract void HandleWithoutResponce(Update update);

        public abstract Task<string> HandleWithResponceAsync(Update update);

        protected void CustomHandlerCall(Update update)
        {
            if (_handler != null)
                _handler(update);
        }

        protected void CallbackQueryNotification(Update update)
        {
            if (IsNeedCallbackQueryNotification)
            {
                if(update.IsCallbackQueryMessage())
                    BotHelper.AnswerCallbackQuery(CallbackQueryNotificationText, update.GetCallbackQueryId(), botToken);
            }
        }
        protected void HideReplyKeyboard(Update update)
        {
            if (IsNeedTryHideReplyKeyboard)
                BotHelper.ReplyKeyboardRemove("_", update.GetChatId(), botToken);
        }
    }
}
