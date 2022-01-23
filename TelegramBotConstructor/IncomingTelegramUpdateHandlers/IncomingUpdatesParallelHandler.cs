using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.MessageHandlers;

namespace TelegramBotConstructor.IncomingTelegramUpdateHandlers
{
    /// <summary>
    /// Класс-обработчик телеграм-апдейтов. Параллельно обрабатывает всё подряд без постановки в очередь
    /// </summary>
    class IncomingUpdatesParallelHandler : IncomingTelegramUpdateHandler
    {
        public IncomingUpdatesParallelHandler(Bot bot) :
            base(bot)
        {
        }

        internal override void AddUpdateToQueue(Update update)
        {
            try
            {
                MessageHandler messageHandler = MessageHandlerFactory.CreateHandler(update, bot);
                messageHandler.Handle();
            }
            catch (Exception ex)
            {
                bot.LogError(ex.Message);
            }
        }
    }
}
