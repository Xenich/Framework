using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.StatesBuilders;

namespace TelegramBotConstructor.StatesBuilders
{
    public class StatesBuilderStarter
    {
        readonly Bot bot;
        internal StatesBuilderStarter(Bot bot)
        {
            this.bot = bot;
        }

        /// <summary>
        /// Начать добавлять состояния. Каждое состояние бота представляет собой состояние конечного автомата.
        /// </summary>
        public StatesBuilder BeginAddStates
        {
            get
            {
                StatesBuilder statesBuilder = new StatesBuilder(bot);
                return statesBuilder;
            }
        }

        /// <summary>
        /// Установить обработчик события удаления чата с ботом
        /// </summary>
        /// <param name="botKickedHandler">Делегат на обработчик</param>
        /// <returns></returns>
        public StatesBuilderStarter SetBotKickedEventHandler(Action<Update> botKickedHandler)
        {
            bot.BotKickedEventHandler = botKickedHandler;
            return this;
        }

        /// <summary>
        /// Установить обработчик события добавления юзером бота
        /// </summary>
        /// <param name="botAddedEventHandler">Делегат на обработчик</param>
        /// <returns></returns>
        public StatesBuilderStarter SetBotAddedEventHandler(Action<Update> botAddedEventHandler)
        {
            bot.BotAddedEventHandler = botAddedEventHandler;
            return this;
        }

        /// <summary>
        /// Установить необходимость обрабатывать сообщения в порядке их поступления. По умолчанию - false.
        /// </summary>
        /// <param name="value">true - каждое следующее сообщение будет ожидать обработки предыдущего</param>
        /// <returns></returns>
        public StatesBuilderStarter SetIsNeedHandleMessagesInApearenceOrder(bool value)
        {
            bot.IsNeedHandleMessagesInApearenceOrder = value;
            return this;
        }
    }
}
