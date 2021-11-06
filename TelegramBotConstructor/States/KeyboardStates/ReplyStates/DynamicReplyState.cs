using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.BotGenerator;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.StateHandlers;

namespace TelegramBotConstructor.States
{
    internal class DynamicReplyState : KeyboardState
    {
        Func<Update, DynamicReplyKeyboardBuilder> keyboardGenerator;

        /// <summary>
        /// Состояние конечного автомата с динамической reply-клавиатурой
        /// </summary>
        /// <param name="name">Имя состояния</param>
        /// <param name="description">Описание</param>
        /// <param name="uid">Идентификатор состояния</param>
        /// <param name="getMessage">Функция, возвращающая сообщение пользователю, которое находится над клавиатурой</param>
        /// <param name="_handler">Обработчик состояния</param>
        /// <param name="botToken">токен бота</param>
        /// <param name="defaultNextStateUid">Идентификатор следующего состояния (состояние по умолчанию - если пользователь не нажал кнопку, а ввёл сообщение)</param>
        /// <param name="keyboardGenerator">Делегат на функцию генератора клавиатуры</param>
        /// <param name="tryDeletePrevKeyboard">Нужно ли пытаться удалить предыдущее сообщение, если оно поступило из inline-клавиатуры.</param>
        public DynamicReplyState(string name, string description, Guid uid, Func<Update, string> getMessage
                                , Action<Update> _handler, string botToken, Guid defaultNextStateUid, Func<Update, DynamicReplyKeyboardBuilder> keyboardGenerator, TryDeletePrevKeyboard tryDeletePrevKeyboard)
            : base(name, description, uid, getMessage, botToken, defaultNextStateUid, tryDeletePrevKeyboard)
        {
            this.keyboardGenerator = keyboardGenerator;
            StateHandler = new ReplyKeyboardStateHandler(_handler, getMessage, botToken, this);
        }
        internal override IKeyboard GetKeyboard(Update update)
        {
            return keyboardGenerator(update).GetKeyboard;
        }
    }
}
