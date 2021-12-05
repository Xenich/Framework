using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor
{
    class IncomingUpdatesQueueMultiThreadHandler
    {
        /*
        private ConcurrentQueue<Update> updatesQueue;
        private Bot bot;

        internal IncomingUpdatesQueueMultiThreadHandler(Bot bot)
        {
            this.bot = bot;

            int processorsCount = Environment.ProcessorCount;
            updatesQueue = new ConcurrentQueue<Update>();


        }

        internal void AddUpdatesToQueue(Response incomingUpdates)
        {
            foreach (Update update in incomingUpdates.Updates)
                updatesQueue.Enqueue(update);



        }
        */
    }
}
