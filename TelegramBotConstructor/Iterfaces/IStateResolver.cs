using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor
{
    /// <summary>
    /// Интерфейс для оперделения состояния конечного автомата при получении разного типа сообщений от пользователя
    /// </summary>
    public interface IStateResolver
    {
        /// <summary>
        /// Метод определения состояния при получении простого сообщения, введённого пользователе вручную
        /// </summary>
        /// <param name="update">Сообщение от пользователя</param>
        /// <returns>Идентификатор состояния</returns>
        Guid SimpleMessageResolve(Update update);

        /// <summary>
        /// Метод определения состояния при получении сообщения из inline-клавиатуры
        /// </summary>
        /// <param name="update">Сообщение от пользователя</param>
        /// <returns>Идентификатор состояния</returns>
        Guid InlineMessageResolve(Update update);

        /// <summary>
        /// Метод определения состояния при получении фотосообщения
        /// </summary>
        /// <param name="update">Сообщение от пользователя</param>
        /// <returns>Идентификатор состояния</returns>
        Guid PhotoMessageResolve(Update update);

        /// <summary>
        /// Метод, устанавливающий состояние конечного автомата, вызывается после обработки текущего состояния.
        /// </summary>
        /// <param name="update">Сообщение от пользователя</param>
        /// <param name="currentState">Идентификатор сосояния</param>
        void SetCurrentState(Update update, Guid currentState);
    }
}
