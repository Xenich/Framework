using System;
using TelegramBotConstructor.BotGenerator;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.StatesBuilders
{
    public class StatesBuilder
    {
        private readonly Bot bot;
        internal StatesBuilder(Bot bot)
        {
            this.bot = bot;
        }

        /// <summary>
        /// Метод добавляет состояние конечного автомата с фиксированной inline-клавиатурой.
        /// Сообщение пользователю формируется динамически делегатом getMessage
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="description">Описание</param>
        /// <param name="uid">Идентификатор состояния</param>
        /// <param name="getMessage">Функция, возвращающая сообщение пользователю, которое находится над клавиатурой</param>
        /// <param name="handler">Обработчик при переходе на данное состояние. Отправка клавиатуры, связанной с данным State выполняется автоматически после вызова данного обработчика</param>
        /// <param name="nextStateUid">Идентификатор следующего состояния (состояние по умолчанию - если пользователь не нажал кнопку, а ввёл сообщение)</param>
        /// <param name="tryDeletePrevKeyboard">Нужно ли пытаться удалить предыдущее сообщение. NO по умолчанию</param>
        /// <returns>InlineKeyboardBuilderStart</returns>
        public FixedInlineStateBuilder_Start AddFixedInlineState(string name, string description, Guid uid, Func<Update, string> getMessage, Action<Update> handler, Guid nextStateUid, TryDeletePrevKeyboard tryDeletePrevKeyboard)
        {
            FixedInlineState inlineState = new FixedInlineState(name, description, uid, getMessage, handler, bot.Token, nextStateUid, tryDeletePrevKeyboard);

            bool success = bot.TryAddState(inlineState);

            if (!success)
                throw new Exception("Не уалось добавить State. Проверьте наименование и UID на предмет дублирования");

            FixedInlineStateBuilder_Start fixedInlineKeyboardBuilderStart = new FixedInlineStateBuilder_Start(this, inlineState);
            return fixedInlineKeyboardBuilderStart;
        }

        /// <summary>
        /// Метод добавляет состояние конечного автомата с динамической inline-клавиатурой. Клавиатура генерируется не на этапе создания бота, а при переходе бота в соответствующее состояние.
        /// Вместо процедуры создания клавиатуры, как это делается в методе AddFixedInlineState, этот метод принимает делегат keyboardGenerator, создающий клавиатуру "на лету".
        /// Сообщение пользователю формируется динамически делегатом getMessage.
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="description">Описание</param>
        /// <param name="uid">Идентификатор состояния</param>
        /// <param name="getMessage">Функция, возвращающая сообщение пользователю, которое находится над клавиатурой</param>
        /// <param name="handler">Обработчик при переходе на данное состояние. Отправка клавиатуры, связанной с данным State выполняется автоматически после вызова данного обработчика</param>
        /// <param name="defaultNextStateUid">Идентификатор следующего состояния (состояние по умолчанию - если пользователь не нажал кнопку, а ввёл сообщение)</param>
        /// <param name="keyboardGenerator">Делегат на функцию генератора клавиатуры</param>
        /// <param name="tryDeletePrevKeyboard">Нужно ли пытаться удалить предыдущее сообщение. NO по умолчанию</param> 
        /// <returns>DinamicInlineKeyboardBuilderStart</returns>
        public StateBuilderStart AddDynamicInlineState(string name, string description, Guid uid, 
                                                        Func<Update, string> getMessage, Action<Update> handler,
                                                        Guid defaultNextStateUid,
                                                        Func<Update, DynamicInlineKeyboardBuilder> keyboardGenerator,
                                                        TryDeletePrevKeyboard tryDeletePrevKeyboard = TryDeletePrevKeyboard.NO)
        {
            DynamicInlineState state = new DynamicInlineState(name, description, uid, getMessage, handler, bot.Token, defaultNextStateUid, keyboardGenerator, tryDeletePrevKeyboard);

            bool success = bot.TryAddState(state);

            if (!success)
                throw new Exception("Не уалось добавить State. Проверьте наименование и UID на предмет дублирования");

            StateBuilderStart stateBuilderStart = new StateBuilderStart(this, state);
            return stateBuilderStart;
        }

        /// <summary>
        /// Метод добавляет состояние конечного автомата с динамической reply-клавиатурой. Клавиатура генерируется не на этапе создания бота, а при переходе бота в соответствующее состояние.
        /// Вместо процедуры создания клавиатуры, как это делается в методе AddFixedReplyState, этот метод принимает делегат keyboardGenerator, создающий клавиатуру "на лету".
        /// Сообщение пользователю формируется динамически делегатом getMessage.
        /// </summary>
        /// <param name="name">Имя состояния</param>
        /// <param name="description">Описание</param>
        /// <param name="uid">Идентификатор состояния</param>
        /// <param name="getMessage">Функция, возвращающая сообщение пользователю, которое находится над клавиатурой</param>
        /// <param name="handler">Обработчик состояния</param>
        /// <param name="defaultNextStateUid">Идентификатор следующего состояния (состояние по умолчанию - если пользователь не нажал кнопку, а ввёл сообщение)</param>
        /// <param name="keyboardGenerator">Делегат на функцию генератора клавиатуры</param>
        /// <param name="tryDeletePrevKeyboard">Нужно ли пытаться удалить предыдущее сообщение, если оно поступило из inline-клавиатуры. NO по умолчанию</param>
        /// <returns></returns>
        public StateBuilderStart AddDynamicReplyState(string name, string description, Guid uid, 
                                                        Func<Update, string> getMessage,
                                                        Action<Update> handler, Guid defaultNextStateUid,
                                                        Func<Update, DynamicReplyKeyboardBuilder> keyboardGenerator,
                                                        TryDeletePrevKeyboard tryDeletePrevKeyboard = TryDeletePrevKeyboard.NO)
        {
            DynamicReplyState state = new DynamicReplyState(name, description, uid, getMessage, handler, bot.Token, defaultNextStateUid, keyboardGenerator, tryDeletePrevKeyboard );

            bool success = bot.TryAddState(state);

            if (!success)
                throw new Exception("Не уалось добавить State. Проверьте наименование и UID на предмет дублирования");

            StateBuilderStart stateBuilderStart = new StateBuilderStart(this, state);
            return stateBuilderStart;
        }


        /// <summary>
        /// Метод добавляет состояние конечного автомата с фиксированной relply-клавиатурой (даёт возможность выбрать текстовые ответы из предложенных).
        /// Сообщение пользователю формируется динамически делегатом getMessage
        /// </summary>
        /// <param name="name">Имя состояния</param>
        /// <param name="description">описание</param>
        /// <param name="uid">Идентификатор состояния</param>
        /// <param name="getMessage">Функция, возвращающая сообщение пользователю, которое находится над клавиатурой</param>
        /// <param name="handler">Обработчик состояния</param>
        /// <param name="nextStateUid">Идентификатор следующего состояния</param>
        /// <param name="tryDeletePrevKeyboard">Нужно ли пытаться удалить предыдущее сообщение, если оно поступило из inline-клавиатуры. NO по умолчанию</param>
        public FixedReplyStateBuilderStart AddFixedReplyState(string name, string description, Guid uid,
                                                                Func<Update, string> getMessage,
                                                                Action<Update> handler, Guid nextStateUid,
                                                                TryDeletePrevKeyboard tryDeletePrevKeyboard = TryDeletePrevKeyboard.NO)
        {
            FixedReplyState replyState = new FixedReplyState(name, description, uid, getMessage, handler, bot.Token, nextStateUid, tryDeletePrevKeyboard);

            bool success = bot.TryAddState(replyState);

            if (!success)
                throw new Exception("Не уалось добавить State. Проверьте наименование и UID на предмет дублирования");

            FixedReplyStateBuilderStart replyStateBuilderStart = new FixedReplyStateBuilderStart(this, replyState);
            return replyStateBuilderStart;
        }



        /// <summary>
        /// Метод добавляет состояние конечного автомата с обычным текстовым сообщением и безальтернативным переходом в следующее состояние
        /// </summary>
        /// <param name="name">Наименование</param>
        /// <param name="description">Описание</param>
        /// <param name="uid">Идентификатор состояния</param>
        /// <param name="getMessage">Функция, возвращающая сообщение пользователю</param>
        /// <param name="handler">Обработчик при переходе на данное состояние. Отправка текстового сообщения, связанного с данным State выполняется автоматически после вызова данного обработчика</param>
        /// <param name="nextStateUid">Идентификатор следующего состояния</param>
        /// <returns>BotBuilder</returns>
        public StateBuilderStart AddTextState(string name, string description, Guid uid, Func<Update, string> getMessage, Action<Update> handler, Guid nextStateUid)
        {
            TextState state = new TextState(name, description, uid, getMessage, handler, bot.Token, nextStateUid);

            bool success = bot.TryAddState(state);
            if (!success)
                throw new Exception("Не уалось добавить State. Проверьте наименование и UID на предмет дублирования");

            //TextStateBuilderStart textStateBuilderStart = new TextStateBuilderStart(this, state);

            StateBuilderStart stateBuilderStart = new StateBuilderStart(this, state);
            return stateBuilderStart;
        }

        /// <summary>
        /// Закончить добавлять состояния
        /// </summary>
        public BotBuilder StopAddStates
        {
            get
            {
                BotBuilder botBuilderFinisher = new BotBuilder(bot);
                return botBuilderFinisher;
            }
        }
    }
}
