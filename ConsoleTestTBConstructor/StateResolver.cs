using System;
using System.Collections.Generic;
using TelegramBotConstructor;

namespace ConsoleTestTBConstructor
{
    class StateResolver : IStateResolver
    {
        //Guid currentState;
        Dictionary<int, Guid> chatIdToCurrentStateDic = new Dictionary<int, Guid>();

        public Guid InlineMessageResolve(Update update)
        {
            int chatId = update.GetChatId();

            if (chatIdToCurrentStateDic.ContainsKey(chatId))
                if (update.CallbackQuery.Data == StateNames.name1.ToString())
                    return Program.nameToGuidDic[StateNames.name2.ToString()];
                else
                    return Program.nameToGuidDic[update.CallbackQuery.Data];
            else
                return Program.nameToGuidDic[update.CallbackQuery.Data];

        }

        public Guid PhotoMessageResolve(Update update)
        {
            return Program.nameToGuidDic["name1"]; 
        }



        public Guid SimpleMessageResolve(Update update)
        {
            int chatId = update.GetChatId();

            string message = update.GetMessageText();
            if (message == "/start")
                return Program.nameToGuidDic["name2"];
            if (message == "666")
                return Program.nameToGuidDic["name1"];
            else
            {
                if (chatIdToCurrentStateDic.ContainsKey(chatId))
                    return chatIdToCurrentStateDic[chatId];
                else
                    return Program.nameToGuidDic["name2"]; 
            }
        }

        public void SetCurrentState(Update update, Guid currentState)
        {
            SetUserCurrentState(update, currentState);
        }

        private void SetUserCurrentState(Update update, Guid currentState)
        {
            int chatId = update.GetChatId();
            if (chatIdToCurrentStateDic.ContainsKey(chatId))
                chatIdToCurrentStateDic[chatId] = currentState;
            else
            {
                chatIdToCurrentStateDic.Add(chatId, currentState);
            }
        }
    }
}