using System;
using System.Collections.Generic;
using TelegramBotConstructor;

namespace ConsoleTestTBConstructor
{
    class StateResolver : IStateResolver
    {
        Dictionary<int, Guid> chatIdToCurrentStateDic = new Dictionary<int, Guid>();

        public Guid InlineMessageResolve(Update update)
        {
            int chatId = update.GetChatId();

            if (chatIdToCurrentStateDic.ContainsKey(chatId))
                if (update.CallbackQuery.Data == StateNames.name1.ToString())
                    return Program.stateNameToGuidDic[StateNames.name2.ToString()];
                else
                    return Program.stateNameToGuidDic[update.CallbackQuery.Data];
            else
                return Program.stateNameToGuidDic[update.CallbackQuery.Data];

        }

        public Guid SimpleMessageResolve(Update update)
        {
            int chatId = update.GetChatId();
            Guid guid = Guid.Empty;
            if (update.Message.Type == MessageTypes.Text)
            {
                string message = update.GetMessageText();
                if (message == "/start")
                    return Program.stateNameToGuidDic["name2"];
                if (message == "666")
                    return Program.stateNameToGuidDic["name1"];
                else
                {
                    if (chatIdToCurrentStateDic.ContainsKey(chatId))
                        return chatIdToCurrentStateDic[chatId];
                    else
                        return Program.stateNameToGuidDic["name2"];
                }
            }

            if (update.Message.Type == MessageTypes.Photo)
            {
                string message = update.GetMessageText();
                if (message == "/start")
                    return Program.stateNameToGuidDic["name2"];
                else
                    return Program.stateNameToGuidDic["name1"];
            }


            if (chatIdToCurrentStateDic.ContainsKey(chatId))
                return chatIdToCurrentStateDic[chatId];
            else
                return Program.stateNameToGuidDic["name2"];
        }

        public void SetNewCurrentState(Update update, Guid defaultNextStateUid)
        {
            int chatId = update.GetChatId();
            if (chatIdToCurrentStateDic.ContainsKey(chatId))
                chatIdToCurrentStateDic[chatId] = defaultNextStateUid;
            else
            {
                chatIdToCurrentStateDic.Add(chatId, defaultNextStateUid);
            }
        }
    }
}