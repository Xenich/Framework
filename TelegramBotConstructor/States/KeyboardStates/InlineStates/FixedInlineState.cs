using System;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.StateHandlers;

namespace TelegramBotConstructor.States
{
    internal class FixedInlineState : KeyboardState
    {
        private IKeyboard keyboard;

        /// <summary>
        /// Состояние конечного автомата с фиксированной inline-клавиатурой (Update.callback_query != null), клавиатура создаётся на этапе создания бота.
        /// Характеризуется несколькими вариантами переходов в другие состояния.
        /// Имеет клавиатуру с кнопками, каждая из которых переводит конечный автомат в определённое состояние.
        /// Состояние конечного автомата, в которое ведёт та или иная кнопка, удобно кодировать UID-ом этого состояния в первых 32 байтах объекта Update.callback_query.data
        /// </summary>
        /// <param name="name">имя состояния</param>
        /// <param name="description">описание</param>
        /// <param name="uid">Идентификатор состояния</param>
        /// <param name="getMessage">Функция, возвращающая сообщение пользователю, которое находится над клавиатурой</param>
        /// <param name="handler">Обработчик состояния</param>
        /// <param name="botToken">Токен бота</param>
        /// <param name="defaultNextStateUid">Идентификатор следующего состояния (состояние по умолчанию - если пользователь не нажал кнопку, а ввёл сообщение с клавиатуры)</param>
        /// <param name="tryDeletePrevKeyboard">Нужно ли пытаться удалить предыдущее сообщение</param>
        public FixedInlineState(string name, string description, Guid uid, Func<Update, string> getMessage, Action<Update> handler, string botToken, Guid defaultNextStateUid, TryDeletePrevKeyboard tryDeletePrevKeyboard)
            : base(name, description, uid, getMessage, botToken, defaultNextStateUid, tryDeletePrevKeyboard)
        {
            StateHandler = new InlineKeyboardStateHandler(handler, getMessage, botToken, this);
        }

        internal void SetKeyboard(IKeyboard keyboard)
        {
            this.keyboard = keyboard;
        }

        internal override IKeyboard GetKeyboard(Update update)      // этот метод нужен только для создания InlineStateHandler (потому что клавиатуры на момент его создания ещё не существует - она попадает в класс не через конструктор)        
        {
            return keyboard;
        }
    }
}
