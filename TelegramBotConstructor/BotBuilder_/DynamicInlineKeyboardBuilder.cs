using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.BotGenerator;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.Keyboards.Buttons;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.BotGenerator
{
    /*
    public class DynamicInlineStateBuilder_Start : StateBuilderStart
    {
        internal DynamicInlineStateBuilder_Start(StatesBuilder statesBuilder, DynamicInlineState inlineState)
            : base(statesBuilder, inlineState)
        { }
    }
    */

    /// <summary>
    /// Динамическая inline-клавиатура формируется не на этапе пострения бота, а "на лету" на этапе формирования ответа на запрос юзера
    /// </summary>
    public class DynamicInlineKeyboardBuilder
    {
        private readonly InlineKeyboard inlineKeyboard = new InlineKeyboard();
        private InlineKeyboardRow currentRow;
        public DynamicKeyboardButtonBuilder AddRow
        {
            get
            {
                InlineKeyboardRow row = new InlineKeyboardRow();
                inlineKeyboard.AddRow(row);
                currentRow = row;
                DynamicKeyboardButtonBuilder buttonBuilder = new DynamicKeyboardButtonBuilder(this, currentRow);
                return buttonBuilder;
            }
        }

        internal InlineKeyboard GetKeyboard
        {
            get
            {
                return inlineKeyboard;
            }
        }
    }

    public class DynamicKeyboardButtonBuilder
    {
        private readonly InlineKeyboardRow currentRow;
        private readonly DynamicInlineKeyboardBuilder keyboardBuilder;

        internal DynamicKeyboardButtonBuilder(DynamicInlineKeyboardBuilder inlineKeyboardBuilder, InlineKeyboardRow currentRow)
        {
            this.currentRow = currentRow;
            this.keyboardBuilder = inlineKeyboardBuilder;
        }

        public DynamicKeyboardButtonBuilder AddButtonToRow( string text, string callback_data)
        {
            InlineKeyboardButton button = new InlineKeyboardButton(text, callback_data);
            currentRow.AddButton(button);
            return this;
        }

        public DynamicKeyboardButtonBuilder AddURLButton(string url, string text)
        {
            InlineKeyboardButton button = new InlineKeyboardButton(url, text, true);
            currentRow.AddButton(button);
            return this;
        }

        public DynamicInlineKeyboardBuilder FinishRow
        {
            get
            {
                return keyboardBuilder;
            }
        }
    }
}
