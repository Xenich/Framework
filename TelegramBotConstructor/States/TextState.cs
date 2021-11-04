using System;
using TelegramBotConstructor.StateHandlers;

namespace TelegramBotConstructor.States
{
    public class TextState : State
    {

        /// <summary>
        /// Состояние конечного автомата с простым текстовым сообщением пользователю
        /// Характеризуется безальтернативным переходом в следующее ссостояние по nextStateUid.        
        /// </summary>
        /// <param name="name">имя состояния</param>
        /// <param name="description">описание</param>
        /// <param name="uid">Идентификатор состояния</param>
        /// <param name="getMessage">Функция, возвращающая сообщение пользователю</param>
        /// <param name="handler">Обработчик состояния</param>
        /// <param name="botToken">Токен бота</param>
        /// <param name="nextStateUid">Идентификатор состояния на которое следует перейти</param>
        public TextState(string name, string description, Guid uid, Func<Update, string> getMessage, Action<Update> handler, string botToken, Guid nextStateUid) 
            : base(name, description, uid, getMessage, nextStateUid)
        {
            stateType = StateType.WithoutKeyboard;
            StateHandler = new TextStateHandler(handler, getMessage, botToken);
        }
    }
}
