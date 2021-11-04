using System;
using TelegramBotConstructor.Keyboards;

namespace TelegramBotConstructor.States
{
    internal class ReplyState : KeyboardState
    {
        private ReplyKeyboard keyboard;

        /// <summary>
        /// Состояние конечного автомата с фиксированной relply-клавиатурой, клавиатура создаётся на этапе создания бота.
        /// Характеризуется одним вариантами переходов в другие состояния.
        /// </summary>
        /// <param name="name">имя состояния</param>
        /// <param name="description">описание</param>
        /// <param name="uid">Идентификатор состояния</param>
        /// <param name="getMessage">Функция, возвращающая сообщение пользователю, которое находится над клавиатурой</param>
        /// <param name="handler">Обработчик состояния</param>
        /// <param name="botToken">Токен бота</param>
        /// <param name="defaultNextStateUid">Идентификатор следующего состояния</param>
        /// <param name="tryDeletePrevKeyboard">Нужно ли пытаться удалить предыдущее сообщение</param>
        public ReplyState(string name, string description, Guid uid, Func<Update, string> getMessage, Action<Update> handler, string botToken, Guid defaultNextStateUid, TryDeletePrevKeyboard tryDeletePrevKeyboard)
            : base(name, description, uid, getMessage, handler, botToken, defaultNextStateUid, tryDeletePrevKeyboard)
        { }

        internal void SetKeyboard(ReplyKeyboard keyboard)
        {
            this.keyboard = keyboard;
        }

        internal override ReplyKeyboard GetKeyboard(Update update)      // этот метод нужен только для создания InlineStateHandler (потому что клавиатуры на момент его создания ещё не существует - она попадает в класс не через конструктор)        
        {
            return keyboard;
        }
    }
}
