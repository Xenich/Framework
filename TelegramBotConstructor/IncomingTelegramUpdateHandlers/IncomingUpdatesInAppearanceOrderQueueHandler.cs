using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.MessageHandlers;

namespace TelegramBotConstructor.IncomingTelegramUpdateHandlers
{
    /// <summary>
    /// Класс представляет собой очередь сообщений, которая обрабатывается последовательно
    /// </summary>
    internal class IncomingUpdatesInAppearanceOrderQueueHandler: IncomingTelegramUpdateHandler
    {
        private ConcurrentQueue<Update> updates = new ConcurrentQueue<Update>();
        private bool isInProcessNow = false;
        private DateTime lastUpdateTime;

        internal IncomingUpdatesInAppearanceOrderQueueHandler(Bot bot):
            base(bot)
        {
            lastUpdateTime = DateTime.Now;
        }

        internal override void AddUpdateToQueue(Update update)
        {
            //Update update = (Update)_update;

            lastUpdateTime = DateTime.Now;
            updates.Enqueue(update);
            if (!isInProcessNow)
                StartHandle();
        }

        private async Task StartHandle()
        {
            isInProcessNow = true;

            Update update;
            while (updates.TryDequeue(out update))
                await HandleMessageFromTelegramAsync(update);

            isInProcessNow = false;
        }

        private async Task HandleMessageFromTelegramAsync(Update update)
        {
            try
            {
                MessageHandler messageHandler = MessageHandlerFactory.CreateHandler(update, bot);
                await messageHandler.Handle();
            }
            catch (Exception ex)
            {
                bot.LogError(ex.Message);
            }
        }

    }
}
