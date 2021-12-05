using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotConstructor
{
    /// <summary>
    /// Интерфейс для определения состояния конечного автомата при получении разного типа сообщений от пользователя
    /// </summary>
    public interface IStateResolver
    {
        /// <summary>
        /// Метод определения состояния при получении простого сообщения, введённого пользователе вручную или из reply-клавиатуры или медиасообщения (фото, видео, голос, документ и т.д.)
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
        /// Метод, устанавливающий новое текущее состояние конечного автомата, вызывается после обработки текущего состояния.
        /// </summary>
        /// <param name="update">Сообщение от пользователя</param>
        /// <param name="defaultNextStateUid">Идентификатор следующего состояния по умолчанию</param>
        void SetNewCurrentState(Update update, Guid defaultNextStateUid);
    }
}
