using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.States;

namespace TelegramBotConstructor.MessageHandlers
{
    internal class MainHandler : MessageHandler
    {
        internal MainHandler(Update update, Bot bot)
            : base(update, bot)
        { }

        internal override async Task Handle()
        {
            int chatId = update.GetChatId();

                // 0. Определяем сначала, нужно ли вообще обрабатывать сообщение (если оно - инлайн)
            if (bot.CheckIfPreviousInlineMessage(chatId, update) )
                return;

                // 1. Определяем текущее состояние
            Guid stateUid = ResolveStateUid(update);            // Определяем идентификатор текущего состояния
            State currentState = bot.GetStateByUid(stateUid);   // Определяем текущее состояние

            if (currentState != null)
            {
                // 2. Вызываем обработчик текущего состояния
                await StateHandler(update, chatId, currentState);

                // 3. Устанавливаем новое текущее состояние
                bot._userDefinedStateResolver.SetNewCurrentState(update, currentState.DefaultNextStateUid);

                // 4. Конец обработки сообщения
                bot.Log(string.Format("HandleMessageFromTelegram: update_id = {0}", update.UpdateId));
            }
            else
                bot.LogError(string.Format("HandleMessageFromTelegram: update_id = {0} . Не удалось найти State с Guid = {1}", update.UpdateId, stateUid.ToString()));

            return;
        }
     

        /// <summary>
        /// Определение идентификатора текущего состояния
        /// </summary>
        /// <param name="update"></param>
        /// <returns>Идентификатор текущего состояния</returns>
        private Guid ResolveStateUid(Update update)
        {
            Guid stateUid = Guid.Empty;

            if (update.Type == UpdateTypes.Message)           // пришло сообщение, введенное пользователем
            {
                stateUid = bot._userDefinedStateResolver.SimpleMessageResolve(update);
            }
            if (update.Type == UpdateTypes.CallbackQuery)     // пришло сообщение от inline-клавиатуры
            {
                if (bot.IsCustomInlineStateResolver)            // выбираем кастомный или стандартный resolver состояния
                    stateUid = bot._userDefinedStateResolver.InlineMessageResolve(update);
                else
                    stateUid = StandartResolveInlineState(update.CallbackQuery.Data);
            }
            return stateUid;
        }

        /// <summary>
        /// Стандартный метод, определяющий состояние конечного автомата при входящем сообщении после нажатия кнопки inline-клавиатуры.
        /// Считывает первые 32 байта callbackQuery и преобразовывает их в Guid
        /// </summary>
        /// <param name="callbackQueryData">объект Update.CallbackQuery.Data</param>
        /// <returns>Идентификатор состояния</returns>
        private Guid StandartResolveInlineState(string callbackQueryData)
        {
            if (callbackQueryData.Length < 32)
                throw new Exception(string.Format("Не удаётся прочитать Guid состояния. callbackQueryData: {0}", callbackQueryData));
            string uid = callbackQueryData.Substring(0, 32);
            Guid guid;
            bool succeess = Guid.TryParse(uid, out guid);
            if (!succeess)
                throw new Exception(string.Format("Не удаётся прочитать Guid состояния. callbackQueryData: {0}", callbackQueryData));
            else
                return guid;
        }


        /// <summary>
        /// Обработчик текущего состояния конечного автомата
        /// </summary>
        /// <param name="update"></param>
        /// <param name="chatId"></param>
        /// <param name="state"></param>
        private async Task StateHandler(Update update, int chatId, State state)
        {
            if (bot.isNeedToCheckPreviousInlineMessage)
            {
                string response = await state.StateHandler.HandleWithResponceAsync(update);
                if (response == "BADSTATUS")
                {
                    bot.LogError(string.Format("HandleMessageFromTelegram: update_id = {0}. BADSTATUS response", update.UpdateId));
                }
                else
                {
                    ResponceFromTelegram resp = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponceFromTelegram>(response);
                    if (resp.Ok)
                    {
                        int message_id = resp.Result.MessageId;
                        //bot.chatIdToCurrentMessageId_dic[chatId] = message_id;
                        bot.SetCurrentMessageIdToChatId(chatId, message_id);
                    }
                }
            }
            else
                state.StateHandler.HandleWithoutResponce(update);
        }
    }
}
