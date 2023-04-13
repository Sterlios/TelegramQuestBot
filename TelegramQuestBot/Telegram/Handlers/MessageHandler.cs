using System;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class MessageHandler : IHandler<Update>
{
    public Action<Update> SentText;

    public void Handle(Update update)
    {
        TextHandler textHandler = TextHandler.Instantiate();
        MessageType messageType = update.Message.Type;

        switch (messageType)
        {
            case MessageType.Text:
                textHandler.Handle(update);
                break;

            default:
                break;
        }
    }
}
