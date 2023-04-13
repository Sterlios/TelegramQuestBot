using System;
using Telegram.Bot.Types;

public class TextHandler : IHandler<Update>
{
    private static TextHandler _handler;

    public Action SentStart;
    public Action<TelegramQuestBot.Quest.Model.User> StartedQuest;
    public Action SentResetConfig;
    public Action<TelegramQuestBot.Quest.Model.User> SentResetQuest;
    public Action SentUpdateQuest;
    public Action SentInfo;
    public Action SentOtherText;

    private TextHandler() { }

    public static TextHandler Instantiate()
    {
        if (_handler == null)
            _handler = new TextHandler();

        return _handler;
    }

    public void Handle(Update update)
    {
        TelegramQuestBot.Quest.Model.User user = new TelegramQuestBot.Quest.Model.User(update.Message.From);

        switch (update.Message.Text.ToLower())
        {
            case BotConst.StartCommand:
                StartedQuest?.Invoke(user);
                break;

            case BotConst.StartQuestCommand:
                StartedQuest?.Invoke(user);
                break;

            case BotConst.ResetConfigCommand:
                SentResetConfig?.Invoke();
                break;

            case BotConst.ResetQuestCommand:
                SentResetQuest?.Invoke(user);
                break;

            case BotConst.UpdateQuestCommand:
                SentUpdateQuest?.Invoke();
                break;

            case BotConst.InfoCommand:
                SentInfo?.Invoke();
                break;

            default:
                SentOtherText?.Invoke();
                break;
        }
    }
}
