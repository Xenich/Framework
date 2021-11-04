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
    public class ReplyStateBuilderStart : MainStateBuilderStart<ReplyStateBuilderStart>
    {
        internal ReplyStateBuilderStart(StatesBuilder statesBuilder, ReplyState state)
            : base(statesBuilder, state)
        {
            derivedStateBuilder = this;
        }

        public ReplyKeyboardBuilder CreateReplyKeyboard(string inputFieldPlaceholder, bool oneTimeKeyboard, bool resizeKeyboard)
        {
            ReplyKeyboardBuilder fixedInlineKeyboardBuilder = new ReplyKeyboardBuilder(statesBuilder, state as ReplyState, inputFieldPlaceholder, oneTimeKeyboard, resizeKeyboard);
            return fixedInlineKeyboardBuilder;

        }
    }

    public class ReplyKeyboardBuilder
    {
        private readonly StatesBuilder statesBuilder;
        private readonly ReplyKeyboard replyKeyboard;
        private readonly ReplyState inlineState;

        internal ReplyKeyboardBuilder(StatesBuilder statesBuilder, ReplyState inlineState, string inputFieldPlaceholder, bool oneTimeKeyboard, bool resizeKeyboard)
        {
            this.statesBuilder = statesBuilder;
            replyKeyboard = new ReplyKeyboard(inputFieldPlaceholder, oneTimeKeyboard, resizeKeyboard);
            this.inlineState = inlineState;
        }

        public ReplyButtonBuilder AddRow
        {
            get
            {
                ReplyKeyboardRow row = new ReplyKeyboardRow();
                replyKeyboard.AddRow(row);
                ReplyButtonBuilder buttonBuilder = new ReplyButtonBuilder(this, row);
                return buttonBuilder;
            }
        }

        public StatesBuilder FinishKeyboard
        {
            get
            {
                inlineState.SetKeyboard(replyKeyboard);
                return statesBuilder;
            }
        }

    }


   
}
