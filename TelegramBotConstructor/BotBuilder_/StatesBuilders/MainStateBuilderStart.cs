using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.StatesBuilders
{
    public class MainStateBuilderStart<T>
    {
        protected StatesBuilder statesBuilder;
        protected State state;
        protected T derivedStateBuilder;          // ссылка на экземпляр класса-наследника

        internal MainStateBuilderStart(StatesBuilder statesBuilder, State state)
        {
            this.statesBuilder = statesBuilder;
            this.state = state;
        }

        /// <summary>
        /// Нужно ли отправлять всплывающие сообщения в случае, если это был запрос обратного вызова, отправленный с inline-клавиатуры
        /// </summary>
        /// <param name="callbackQueryNotificationText">Текст, посылаемый юзеру в виде всплывающего сообщения в случае, если на это состояние он попал после нажатия кнопки inline-клавиатуры</param>
        /// <returns></returns>
        public T WithCallbackQueryNotification(string callbackQueryNotificationText)
        {
            //state.StateHandler.CallbackQueryNotificationText = callbackQueryNotificationText;
            state.StateHandler.IsNeedCallbackQueryNotification = true;

            state.StateHandler.GetCallbackQueryNotificationText = (u) => { return callbackQueryNotificationText; };
            return derivedStateBuilder;
        }

        /// <summary>
        /// Нужно ли отправлять всплывающие сообщения в случае, если это был запрос обратного вызова, отправленный с inline-клавиатуры. Метод используется в случае динамического формирования текста
        /// </summary>
        /// <param name="getCallbackQueryNotificationText"> функция, возвращющая текст, посылаемый юзеру в виде всплывающего сообщения в случае, если на это состояние он попал после нажатия кнопки inline-клавиатуры</param>
        /// <returns></returns>
        public T WithCallbackQueryNotification(Func<Update, string> getCallbackQueryNotificationText)
        {
            //state.StateHandler.CallbackQueryNotificationText = callbackQueryNotificationText;
            state.StateHandler.IsNeedCallbackQueryNotification = true;

            state.StateHandler.GetCallbackQueryNotificationText = getCallbackQueryNotificationText;
            return derivedStateBuilder;
        }

        /// <summary>
        /// Нужно ли пытаться спрятать reply-клавиатуру (в случае, если предполагается попадние на этот state после reply-клавиатуры )
        /// </summary>
        public T TryHideReplyKeyBoard
        {
            get
            {
                state.StateHandler.IsNeedTryHideReplyKeyboard = true;
                return derivedStateBuilder;
            }
        }
    }
}
