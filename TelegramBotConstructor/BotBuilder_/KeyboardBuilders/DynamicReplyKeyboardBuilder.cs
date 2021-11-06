using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards;
using TelegramBotConstructor.StatesBuilders;

namespace TelegramBotConstructor.BotGenerator
{
    public class DynamicReplyKeyboardBuilder
    {
        private readonly ReplyKeyboard replyKeyboard;

        public DynamicReplyKeyboardBuilder(string inputFieldPlaceholder, bool oneTimeKeyboard, bool resizeKeyboard)
        {
            replyKeyboard = new ReplyKeyboard(inputFieldPlaceholder,  oneTimeKeyboard,  resizeKeyboard);
        }
        
        public ReplyButtonBuilder<DynamicReplyKeyboardBuilder> AddRow
        {
            get
            {
                ReplyKeyboardRow row = new ReplyKeyboardRow();
                replyKeyboard.AddRow(row);
                ReplyButtonBuilder<DynamicReplyKeyboardBuilder> buttonBuilder = new ReplyButtonBuilder<DynamicReplyKeyboardBuilder>(this, row);
                return buttonBuilder;
            }
        }
        
        internal ReplyKeyboard GetKeyboard
        {
            get
            {
                return replyKeyboard;
            }
        }
    }
}
