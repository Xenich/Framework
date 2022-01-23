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

    /// <summary>
    /// 1) ByChatIdQueueHandling - группировка по chatId. Каждый chatId ассоциирован со своей очередью сообщений, которая обрабатывается последовательно.
    /// 2) InAppearanceOrderQueueHandling - Последовательная обработка каждого ообщения. Все сообщения ставятся в очередь.
    /// 3) ParallelHandling - параллельная обрабатка всего подряд без постановки в очередь.
    /// </summary>
    public enum OrderOfUpdatesHandling
    {
        ByChatIdQueueHandling,                  
        InAppearanceOrderQueueHandling,
        ParallelHandling
    }
}
