using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor.IncomingTelegramUpdateHandlers
{
    /// <summary>
    /// В классе входящие апдейты группируются по chatId в словаре. Каждый chatId ассоциирован со своей очередью сообщений, которая обрабатывается ПОСЛЕДОВАТЕЛЬНО
    /// </summary>
    class IncomingUpdatesByChatIdQueueHandler: IncomingTelegramUpdateHandler
    {
        private readonly ConcurrentDictionary<int, IncomingUpdatesInAppearanceOrderQueueHandler> chatIdToUpdates_Dic = 
            new ConcurrentDictionary<int, IncomingUpdatesInAppearanceOrderQueueHandler>();

        public IncomingUpdatesByChatIdQueueHandler(Bot bot):
            base(bot)
        {
        }

        internal override void AddUpdateToQueue(Update update)
        {
            //Update update = (Update)_update;    

            int chatId = update.GetChatId();
            IncomingUpdatesInAppearanceOrderQueueHandler incomingUpdatesInAppearanceOrderQueueHandler;

            bool success = chatIdToUpdates_Dic.TryGetValue(chatId, out incomingUpdatesInAppearanceOrderQueueHandler);
            if (!success)
            {
                incomingUpdatesInAppearanceOrderQueueHandler = new IncomingUpdatesInAppearanceOrderQueueHandler(bot);
                chatIdToUpdates_Dic.GetOrAdd(chatId, incomingUpdatesInAppearanceOrderQueueHandler);
            }
            incomingUpdatesInAppearanceOrderQueueHandler.AddUpdateToQueue(update);
        }


    }
}