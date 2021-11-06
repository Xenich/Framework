using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.Keyboards.Buttons;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.StatesBuilders
{
    public class FixedReplyStateBuilderStart : MainStateBuilderStart<FixedReplyStateBuilderStart>
    {
        internal FixedReplyStateBuilderStart(StatesBuilder statesBuilder, FixedReplyState state)
            : base(statesBuilder, state)
        {
            derivedStateBuilder = this;
        }



        /// <summary>
        /// Начать создавать reply - клавиатуру
        /// </summary>
        /// <param name="inputFieldPlaceholder">The placeholder to be shown in the input field when the keyboard is active; 1-64 characters</param>
        /// <param name="oneTimeKeyboard">Requests clients to hide the keyboard as soon as it's been used. The keyboard will still be available, but clients will automatically display the usual letter-keyboard in the chat – the user can press a special button in the input field to see the custom keyboard again.</param>
        /// <param name="resizeKeyboard">Requests clients to resize the keyboard vertically for optimal fit. Defaults to false, in which case the custom keyboard is always of the same height as the app's standard keyboard.</param>
        /// <returns></returns>
        public FixedReplyKeyboardBuilder CreateReplyKeyboard(string inputFieldPlaceholder, bool oneTimeKeyboard, bool resizeKeyboard)
        {
            FixedReplyKeyboardBuilder fixedInlineKeyboardBuilder = new FixedReplyKeyboardBuilder(statesBuilder, state as FixedReplyState, inputFieldPlaceholder, oneTimeKeyboard, resizeKeyboard);
            return fixedInlineKeyboardBuilder;

        }
    }
}
