using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.MessageHandlers;

namespace TelegramBotConstructor
{
    /// <summary>
    /// Класс представляет собой очередь сообщений, которая обрабатывается последовательно
    /// </summary>
    internal class IncomingUpdatesInAppearanceOrderQueueHandler
    {
        private readonly Bot bot;
        private ConcurrentQueue<Update> updates = new ConcurrentQueue<Update>();
        private bool isInProcessNow = false;
        private DateTime lastUpdateTime;

        internal IncomingUpdatesInAppearanceOrderQueueHandler(Bot bot)
        {
            this.bot = bot;
            lastUpdateTime = DateTime.Now;
        }

        internal void AddUpdateToQueue(Update update)
        {
            //Update update = (Update)_update;

            lastUpdateTime = DateTime.Now;
            updates.Enqueue(update);
            if (!isInProcessNow)
                StartHandle();
        }

        async Task StartHandle()
        {
            isInProcessNow = true;

            Update update;
            while (updates.TryDequeue(out update))
                await HandleMessageFromTelegramAsync(update);

            isInProcessNow = false;
        }

        async Task HandleMessageFromTelegramAsync(Update update)
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
