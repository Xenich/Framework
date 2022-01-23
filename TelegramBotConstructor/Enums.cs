using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor
{
    public enum StateType
    {
        WithKeyboard,
        WithoutKeyboard
    }

    public enum TryDeletePrevKeyboard
    {
        NO,
        YES
    }

    public enum InlineStateResolver
    {
        Standart,
        Custom
    }
}
