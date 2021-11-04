using System;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.StateHandlers;

namespace TelegramBotConstructor.States
{
    internal abstract class KeyboardState : State
    {
        public KeyboardState(string name, string description, Guid uid, Func<Update, string> getMessage, Action<Update> handler, string botToken, Guid defaultNextStateUid, TryDeletePrevKeyboard tryDeletePrevKeyboard) 
            : base(name, description, uid, getMessage, defaultNextStateUid, tryDeletePrevKeyboard)
        {         
            stateType = StateType.WithKeyboard;
            StateHandler = new KeyboardStateHandler(handler, getMessage, botToken, this);
        }

        internal abstract IKeyboard GetKeyboard(Update update);       // этот метод нужен только для создания InlineStateHandler (потому что клавиатуры на момент его создания ещё не существует - она попадает в класс не через конструктор)
    }
}
