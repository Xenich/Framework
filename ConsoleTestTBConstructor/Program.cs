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
        public static Dictionary<string, Guid> nameToGuidDic = new Dictionary<string, Guid>();

        static void Main(string[] args)
        {
           
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

            nameToGuidDic.Add(StateNames.name1.ToString(), inlineState_1UID);
            nameToGuidDic.Add(StateNames.name2.ToString(), inlineState_2UID);
            nameToGuidDic.Add(StateNames.name3.ToString(), inlineState_3UID);
            nameToGuidDic.Add(StateNames.name4.ToString(), textState_1UID);
            nameToGuidDic.Add(StateNames.name5.ToString(), textState_2UID);
            nameToGuidDic.Add(StateNames.dyamic.ToString(), dynamicState_1UID);
            nameToGuidDic.Add(StateNames.reply.ToString(), replyState_1UID);

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
            
            Func<Update, DynamicInlineKeyboardBuilder> dynamicKeyboard_1=
                (upd) =>
                {
                    DynamicInlineKeyboardBuilder keyboardBuilder = new DynamicInlineKeyboardBuilder();
                    return
                    keyboardBuilder
                        .AddRow
                            .AddButtonToRow( "ChatId: " + upd.GetChatId().ToString(), StateNames.name2.ToString())
                            .AddURLButton("google.com", "GOTO GOOGLE")
                         .FinishRow
                         .AddRow
                            .AddURLButton("facebook.com", "GOTO Facebook")
                         .FinishRow;
                };
            
            Bot bot;

            try
            {
                BotBuilder builder =
                    Bot
                    .StartFlowBuilder(logger, stateResolver)
                    .SetToken("1120463837:AAHEvmnejgfiH7CvnEts9M5TliR-SQdigpc")
                    .SetInternalHandlerWebHook(1000)
                    .BeginAddStates
                        .AddFixedInlineState(StateNames.name1.ToString(), "description", inlineState_1UID, (upd) => "chatId=" + upd.GetChatId().ToString() + ": message NAME1", null, inlineState_1UID, TryDeletePrevKeyboard.YES)
                            .WithCallbackQueryNotification("")
                            .CreateKeyboard
                                .AddRow
                                    .AddButton(inlineState_2UID, "name2", StateNames.name2.ToString())
                                    .AddButton(inlineState_3UID, "name3", StateNames.name3.ToString())
                                .FinishRow
                                .AddRow
                                    .AddButton(inlineState_3UID, "name3", StateNames.name3.ToString())
                                .FinishRow
                            .FinishKeyboard
                         .AddFixedInlineState(StateNames.name2.ToString(), "description2", inlineState_2UID, (upd) => "message NAME2", null, inlineState_1UID, TryDeletePrevKeyboard.YES)
                            .WithCallbackQueryNotification("")
                            .TryHideReplyKeyBoard
                            .CreateKeyboard
                                .AddRow
                                    .AddButton(inlineState_2UID, "name2", StateNames.name2.ToString())
                                    .AddButton(inlineState_3UID, "name3", StateNames.name3.ToString())
                                .FinishRow
                                .AddRow
                                    .AddButton(inlineState_1UID, "name1", StateNames.name1.ToString())
                                    .AddURLButton("facebook.com", "google.com")
                                .FinishRow
                            .FinishKeyboard
                        .AddFixedInlineState(StateNames.name3.ToString(), "description2", inlineState_3UID, (upd) => "message NAME3", handler, inlineState_3UID, TryDeletePrevKeyboard.YES)
                            .WithCallbackQueryNotification("Callbackquery Notification Text")
                            .CreateKeyboard
                                .AddRow
                                    .AddButton(inlineState_1UID, "name1", StateNames.name1.ToString())
                                    .AddButton(inlineState_2UID, "name2", StateNames.name2.ToString())
                                    .AddButton(replyState_1UID, "TO REPLY", StateNames.reply.ToString())
                                    .AddButton(dynamicState_1UID, "К динамике", StateNames.dyamic.ToString())
                                .FinishRow
                            .FinishKeyboard                            
                        .AddTextState(StateNames.name4.ToString(), "description simple text", textState_1UID, (upd) => "chatId=" + upd.GetChatId().ToString() + "Простое текстовое сообщение1", handler1, textState_2UID)
                            .WithCallbackQueryNotification("Ok")
                            .Next                        
                        .AddTextState(StateNames.name5.ToString(), "description simple text", textState_2UID, (upd) => "chatId=" + upd.GetChatId().ToString() + "Простое текстовое сообщение2", handler1, inlineState_2UID)
                            .Next
                        .AddDynamicInlineState(StateNames.dyamic.ToString(), "", dynamicState_1UID, (upd) => "chatId=" + upd.GetChatId().ToString() + " Динамическое состояние", handler1, inlineState_1UID, dynamicKeyboard_1, TryDeletePrevKeyboard.YES)
                            .WithCallbackQueryNotification("Dynamic Callback Notification")
                            .Next
                        .AddReplyState(StateNames.reply.ToString(),"", replyState_1UID, (upd) => "chatId=" + upd.GetChatId().ToString() + "ReplyState!!!", handler1, inlineState_2UID)
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
        reply
    }
}
