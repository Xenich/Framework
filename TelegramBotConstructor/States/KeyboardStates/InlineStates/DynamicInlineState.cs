using System;
using TelegramBotConstructor.BotGenerator;
using TelegramBotConstructor.Keyboards;

namespace TelegramBotConstructor.States
{
    internal class DynamicInlineState : KeyboardState
    {
        Func<Update, DynamicInlineKeyboardBuilder> keyboardGenerator; 

        /// <summary>
        /// Состояние конечного автомата с динамической inline-клавиатурой  (Update.callback_query != null), клавиатура генерируется не на этапе создания бота, а при переходе бота в соответствующее состояние.
        /// Характеризуется несколькими вариантами переходов в другие состояния.
        /// Имеет клавиатуру с кнопками, каждая из которых переводит конечный автомат в определённое состояние.
        /// Состояние конечного автомата, в которое ведёт та или иная кнопка, удобно кодировать UID-ом этого состояния в первых 32 байтах объекта Update.callback_query.data
        /// </summary>
        /// <param name="name">имя состояния</param>
        /// <param name="description">описание</param>
        /// <param name="uid">Идентификатор состояния</param>
        /// <param name="getMessage">Функция, возвращающая сообщение пользователю, которое находится над клавиатурой</param>
        /// <param name="_handler">Обработчик состояния</param>
        /// <param name="botToken">Токен бота</param>
        /// <param name="defaultNextStateUid">Идентификатор следующего состояния (состояние по умолчанию - если пользователь не нажал кнопку, а ввёл сообщение с клавиатуры)</param>
        /// <param name="keyboardGenerator">Делегат на функцию генератора клавиатуры</param>
        /// <param name="tryDeletePrevKeyboard">Нужно ли пытаться удалить предыдущее сообщение</param>
        public DynamicInlineState(string name, string description, Guid uid, Func<Update, string> getMessage, Action<Update> _handler, string botToken, Guid defaultNextStateUid, Func<Update, DynamicInlineKeyboardBuilder> keyboardGenerator, TryDeletePrevKeyboard tryDeletePrevKeyboard)
            : base(name, description, uid, getMessage, _handler, botToken, defaultNextStateUid, tryDeletePrevKeyboard)
        {
            this.keyboardGenerator = keyboardGenerator;
        }

        internal override InlineKeyboard GetKeyboard(Update update)       // этот метод нужен только для создания InlineStateHandler (потому что клавиатуры на момент его создания ещё не существует - она попадает в класс не через конструктор)        
        {
            return keyboardGenerator(update).GetKeyboard;
        }
    }
}
