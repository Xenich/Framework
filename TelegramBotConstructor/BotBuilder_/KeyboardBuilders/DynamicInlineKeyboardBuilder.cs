using System;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.Keyboards.Buttons;
using TelegramBotConstructor.StatesBuilders;

namespace TelegramBotConstructor.BotGenerator
{
    /// <summary>
    /// Динамическая inline-клавиатура формируется не на этапе пострения бота, а "на лету" на этапе формирования ответа на запрос юзера
    /// </summary>
    public class DynamicInlineKeyboardBuilder
    {
        private readonly InlineKeyboard inlineKeyboard = new InlineKeyboard();

        public InlineButtonBuilder<DynamicInlineKeyboardBuilder> AddRow
        {
            get
            {
                InlineKeyboardRow row = new InlineKeyboardRow();
                inlineKeyboard.AddRow(row);
                InlineButtonBuilder<DynamicInlineKeyboardBuilder> buttonBuilder = new InlineButtonBuilder<DynamicInlineKeyboardBuilder>(this, row);
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


}
