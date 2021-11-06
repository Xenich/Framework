using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.StatesBuilders
{
    public class FixedReplyKeyboardBuilder
    {
        private readonly StatesBuilder statesBuilder;
        private readonly ReplyKeyboard replyKeyboard;
        private readonly FixedReplyState replyState;

        internal FixedReplyKeyboardBuilder(StatesBuilder statesBuilder, FixedReplyState replyState, string inputFieldPlaceholder, bool oneTimeKeyboard, bool resizeKeyboard)
        {
            this.statesBuilder = statesBuilder;
            replyKeyboard = new ReplyKeyboard(inputFieldPlaceholder, oneTimeKeyboard, resizeKeyboard);
            this.replyState = replyState;
        }

        public ReplyButtonBuilder<FixedReplyKeyboardBuilder> AddRow
        {
            get
            {
                ReplyKeyboardRow row = new ReplyKeyboardRow();
                replyKeyboard.AddRow(row);
                ReplyButtonBuilder<FixedReplyKeyboardBuilder> buttonBuilder = new ReplyButtonBuilder<FixedReplyKeyboardBuilder>(this, row);
                return buttonBuilder;
            }
        }

        public StatesBuilder FinishKeyboard
        {
            get
            {
                replyState.SetKeyboard(replyKeyboard);
                return statesBuilder;
            }
        }

    }

}
