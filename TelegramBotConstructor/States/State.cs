using System;
using TelegramBotConstructor.StateHandlers;

namespace TelegramBotConstructor.States
{
    /// <summary>
    /// Состояние конечного автомата.
    /// </summary>
    public abstract class State
    {
        public readonly string Name;
        public readonly string Description;
        public readonly Guid Uid;
        public readonly Guid DefaultNextStateUid;
        internal readonly TryDeletePrevKeyboard tryDeletePrevKeyboard;
        protected readonly Func<Update, string> getMessage;

        protected StateType stateType;
        internal StateHandler StateHandler;

        protected State(string name, string description, Guid uid, Func<Update, string> getMessage, Guid defaultNextStateUid, TryDeletePrevKeyboard tryDeletePrevKeyboard = TryDeletePrevKeyboard.NO)
        {
            this.getMessage = getMessage;
            this.tryDeletePrevKeyboard = tryDeletePrevKeyboard;
            Uid = uid;
            Name = name;
            Description = description;
            DefaultNextStateUid = defaultNextStateUid;
        }

    }


}
