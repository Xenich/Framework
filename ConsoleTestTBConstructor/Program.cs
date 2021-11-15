using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using TelegramBotConstructor;
using TelegramBotConstructor.BotGenerator;

using System.Collections.Generic;


using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTestTBConstructor
{
    class Program
    {
        public static Dictionary<string, Guid> stateNameToGuidDic = new Dictionary<string, Guid>();

        static void Main(string[] args)
        {
            //Console.ReadKey();

            Console.WriteLine("ПРИМЕР СОЗДАНИЯ БОТА");

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;

            var serviceProvider = new ServiceCollection()
                                        .AddLogging(builder => builder.AddConsole())
                                        .BuildServiceProvider();           

            ILogger logger = serviceProvider
                                .GetService<ILoggerFactory>()
                                .CreateLogger<Program>();

            IStateResolver stateResolver = new StateResolver();

            Guid inlineState_1UID = Guid.NewGuid();
            Guid inlineState_2UID = Guid.NewGuid();
            Guid inlineState_3UID = Guid.NewGuid();
            Guid textState_1UID = Guid.NewGuid();
            Guid textState_2UID = Guid.NewGuid();
            Guid dynamicState_1UID = Guid.NewGuid();
            Guid replyState_1UID = Guid.NewGuid();
            Guid replyDynamicState_1UID = Guid.NewGuid();

            
            stateNameToGuidDic.Add(StateNames.name1.ToString(), inlineState_1UID);
            stateNameToGuidDic.Add(StateNames.name2.ToString(), inlineState_2UID);
            stateNameToGuidDic.Add(StateNames.name3.ToString(), inlineState_3UID);
            stateNameToGuidDic.Add(StateNames.name4.ToString(), textState_1UID);
            stateNameToGuidDic.Add(StateNames.name5.ToString(), textState_2UID);
            stateNameToGuidDic.Add(StateNames.dyamic.ToString(), dynamicState_1UID);
            stateNameToGuidDic.Add(StateNames.reply.ToString(), replyState_1UID);
            stateNameToGuidDic.Add(StateNames.dynamicRep.ToString(), replyDynamicState_1UID);
            
            Action<Update> handler =
                (upd) =>
                {
                    logger.LogWarning(upd.UpdateId.ToString());
                };

            Action<Update> handler1 =
                (upd) =>
                {
                    logger.LogWarning("Обработчик textState_1UID" + upd.UpdateId.ToString());
                };
            
            Func<Update, DynamicInlineKeyboardBuilder> dynamicInlineKeyboard_1=
                (upd) =>
                {
                    DynamicInlineKeyboardBuilder keyboardBuilder = new DynamicInlineKeyboardBuilder();
                    return
                    keyboardBuilder
                        .AddRow
                            .AddButtonWithStateInCallbackData(inlineState_2UID, "ChatId: " + upd.GetChatId().ToString())                            
                            .AddURLButton("google.com", "GOTO GOOGLE")
                         .FinishRow
                         .AddRow
                            .AddURLButton("facebook.com", "GOTO Facebook")
                         .FinishRow;
                };

            Func<Update, DynamicReplyKeyboardBuilder> dynamicReplyKeyboard_1 =
                (upd) =>
                {
                    DynamicReplyKeyboardBuilder keyboardBuilder = new DynamicReplyKeyboardBuilder("fieldPlaceholder", true, true);
                    return
                    keyboardBuilder
                        .AddRow
                            .AddRequestContactButton("Контакт")
                            .AddButton("Введено: " + upd.GetMessageText())                            
                         .FinishRow
                         .AddRow
                            .AddRequestLocationButton("Локация")
                         .FinishRow;
                };

            Action<Update> botKickedHandler = (upd) => { logger.LogWarning("Бот был забанен" + upd.UpdateId.ToString()); };

            Bot bot;

            try
            {
                BotBuilder builder =
                    Bot
                    .StartFlowBuilder(logger, stateResolver)
                    .SetToken("1120463837:AAHEvmnejgfiH7CvnEts9M5TliR-SQdigpc")
                    .SetInternalHandlerWebHook(1000)
                    .SetBotKickedEventHandler(botKickedHandler)
                    .SetBotAddedEventHandler((upd)=> { logger.LogWarning("Бот был добавлен" + upd.UpdateId.ToString()); })
                    .BeginAddStates
                        //.SetCustomInlineStateResolver
                        .AddFixedInlineState(StateNames.name1.ToString(), "description", inlineState_1UID, (upd) => "chatId=" + upd.GetChatId().ToString() + ": message NAME1", null, inlineState_1UID, TryDeletePrevKeyboard.YES)
                            .WithCallbackQueryNotification("")
                            .CreateKeyboard
                                .AddRow
                                    .AddButtonWithStateInCallbackData(textState_1UID, "To text state 1")
                                    .AddButtonWithStateInCallbackData(inlineState_3UID,"name3", StateNames.name3.ToString())
                                .FinishRow
                                .AddRow
                                    .AddButtonWithStateInCallbackData(inlineState_3UID, "name3", StateNames.name3.ToString())
                                .FinishRow
                            .FinishKeyboard
                         .AddFixedInlineState(StateNames.name2.ToString(), "description2", inlineState_2UID, (upd) => "message NAME2", null, inlineState_1UID, TryDeletePrevKeyboard.YES)
                            .WithCallbackQueryNotification("")
                            .TryHideReplyKeyBoard
                            .CreateKeyboard
                                .AddRow
                                    .AddButtonWithStateInCallbackData(replyDynamicState_1UID, "TO dynamic Reply")
                                    .AddButtonWithStateInCallbackData(inlineState_3UID, "name3", StateNames.name3.ToString())
                                .FinishRow
                                .AddRow
                                    .AddButtonWithStateInCallbackData(inlineState_1UID, "name1", StateNames.name1.ToString())
                                    .AddURLButton("facebook.com", "google.com")
                                .FinishRow
                            .FinishKeyboard
                        .AddFixedInlineState(StateNames.name3.ToString(), "description2", inlineState_3UID, (upd) => "message NAME3", handler, inlineState_3UID, TryDeletePrevKeyboard.YES)
                            .WithCallbackQueryNotification("Callbackquery Notification Text")
                            .CreateKeyboard
                                .AddRow
                                    .AddButtonWithStateInCallbackData(inlineState_1UID, "name1")
                                    .AddButtonWithStateInCallbackData(inlineState_2UID, "name2")
                                    .AddButtonWithStateInCallbackData(replyState_1UID, "TO REPLY")
                                    .AddButtonWithStateInCallbackData(dynamicState_1UID, "К динамике")
                                .FinishRow
                            .FinishKeyboard                            
                        .AddTextState(StateNames.name4.ToString(), "description simple text", textState_1UID, (upd) => "chatId=" + upd.GetChatId().ToString() + "Простое текстовое сообщение1", handler1, textState_2UID)
                            .WithCallbackQueryNotification("Ok")
                            .Next                        
                        .AddTextState(StateNames.name5.ToString(), "description simple text", textState_2UID, (upd) => "chatId=" + upd.GetChatId().ToString() + "Простое текстовое сообщение2", handler1, inlineState_2UID)
                            .Next
                        .AddDynamicInlineState(StateNames.dyamic.ToString(), "", dynamicState_1UID, (upd) => "chatId=" + upd.GetChatId().ToString() + " Динамическое состояние", handler1, inlineState_1UID, dynamicInlineKeyboard_1, TryDeletePrevKeyboard.YES)
                            .WithCallbackQueryNotification("Dynamic Callback Notification")
                            .Next
                        .AddDynamicReplyState(StateNames.dynamicRep.ToString(),"descr", replyDynamicState_1UID, (upd)=>"Динамически сформированная Reply-клавиатура", handler1, inlineState_1UID, dynamicReplyKeyboard_1, TryDeletePrevKeyboard.YES)
                            .Next
                        .AddFixedReplyState(StateNames.reply.ToString(),"", replyState_1UID, (upd) => "chatId=" + upd.GetChatId().ToString() + "ReplyState!!!", handler1, inlineState_2UID, TryDeletePrevKeyboard.YES)
                            .WithCallbackQueryNotification("CallbackQueryNotification FROM REPLY KEYBOARD")
                            .CreateReplyKeyboard("fieldPlaceholder", true, true)                            
                                .AddRow
                                    .AddRequestContactButton("Запрос контакта" )
                                    .AddButton("Трулала")
                                .FinishRow
                                .AddRow
                                    .AddRequestLocationButton("Запрос гео")
                                    .AddButton("Трулала2")
                                .FinishRow
                            .FinishKeyboard
                    .StopAddStates;
                
                if (builder.IsReadyToBuild())
                {
                    bot = builder.Build();
                    bot.StartAsync(cancellationToken);
                }
                else
                    logger.LogError("Bot not ready");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
                     

            Console.ReadKey();
            //inlineState_2.SetKeyboard()
        }
    }

    enum StateNames
    {
        name1,
        name2,
        name3,
        name4, 
        name5,
        dyamic,
        reply,
        dynamicRep
    }
}
