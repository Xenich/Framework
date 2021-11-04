using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.BotGenerator;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.StatesBuilders
{
    public class FixedInlineStateBuilder_Start : MainStateBuilderStart<FixedInlineStateBuilder_Start>
    {
        internal FixedInlineStateBuilder_Start(StatesBuilder statesBuilder, FixedInlineState state)
            : base(statesBuilder, state)
        {
            derivedStateBuilder = this;
        }


        public FixedInlineKeyboardBuilder CreateKeyboard
        {
            get
            {
                FixedInlineKeyboardBuilder fixedInlineKeyboardBuilder = new FixedInlineKeyboardBuilder(statesBuilder, state as FixedInlineState);
                return fixedInlineKeyboardBuilder;
            }
        }
    }

    public class FixedInlineKeyboardBuilder
    {
        private readonly StatesBuilder statesBuilder;
        private readonly InlineKeyboard inlineKeyboard;
        private readonly FixedInlineState inlineState;

        internal FixedInlineKeyboardBuilder(StatesBuilder statesBuilder, FixedInlineState inlineState)
        {
            this.statesBuilder = statesBuilder;
            inlineKeyboard = new InlineKeyboard();
            this.inlineState = inlineState;
        }

        public InlineButtonBuilder AddRow
        {
            get
            {
                InlineKeyboardRow row = new InlineKeyboardRow();
                inlineKeyboard.AddRow(row);
                InlineButtonBuilder buttonBuilder = new InlineButtonBuilder(this, row);
                return buttonBuilder;
            }
        }

        public StatesBuilder FinishKeyboard
        {
            get
            {
                inlineState.SetKeyboard(inlineKeyboard);
                return statesBuilder;
            }
        }
    }
}
