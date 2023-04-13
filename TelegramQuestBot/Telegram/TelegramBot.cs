using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using TelegramQuestBot;
using TelegramQuestBot.Quest.Service;

public class TelegramBot
{
    private MessageHandler _messageHandler;
    public static ITelegramBotClient Bot;
    private QuestService _questService;
    private MessageConstructor _messageConstructor;

    private static readonly ILogger _logger = LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
        builder.AddDebug();
    }).CreateLogger<TelegramBot>();

    public TelegramBot(string token, QuestService questService, MessageConstructor messageConstructor)
    {
        _messageHandler = new MessageHandler();
        _questService = questService;
        _messageConstructor = messageConstructor;
        Bot = new TelegramBotClient(token);
    }

    public void Run()
    {
        Console.WriteLine("Запущен бот " + Bot.GetMeAsync().Result.FirstName);
        Console.WriteLine("Login " + Bot.GetMeAsync().Result.Username);

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { },
        };

        Bot.StartReceiving(
            HandleUpdateAsync,
            HandleError,
            receiverOptions,
            cancellationToken
        );

        Console.ReadLine();
    }

    private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
        Console.WriteLine();

        UpdateType updateType = update.Type;

        switch (updateType)
        {
            case UpdateType.Message:
                _messageHandler.Handle(update);
                break;

            case UpdateType.CallbackQuery:
                HandleCallbackQuery(update);
                break;

            default:
                break;
        }
    }

    private async Task HandleError(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine();
        _logger.LogError(exception.ToString());
    }

    private void HandleCallbackQuery(Update update)
    {
        int messageID = update.CallbackQuery.Message.MessageId;

        if (int.TryParse(update.CallbackQuery.Data, out int answerNumber))
        {
            if(_questService.OnAnswered(new TelegramQuestBot.Quest.Model.User(update.CallbackQuery.From), answerNumber))
                Bot.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat, messageID);
        }
        else if (update.CallbackQuery.Data == BotConst.ReadyButton)
        {
            Bot.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat, messageID);
            _questService.StartNewQuest(new TelegramQuestBot.Quest.Model.User(update.CallbackQuery.From));
        }
        else if (update.CallbackQuery.Data == BotConst.NextQuestionButton)
        {
            Bot.EditMessageReplyMarkupAsync(update.CallbackQuery.Message.Chat, messageID);
            _questService.SendQuestion(new TelegramQuestBot.Quest.Model.User(update.CallbackQuery.From));
        }
    }
}