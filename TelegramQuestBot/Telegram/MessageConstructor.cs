using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramQuestBot.Quest.Service;

namespace TelegramQuestBot
{
    public class MessageConstructor
    {
        private QuestService _questService;
        private TextHandler _textHandler;
        private Task _task;

        public MessageConstructor()
        {
            _textHandler = TextHandler.Instantiate();
            _questService = QuestService.Instantiate();
            _questService.Started += OnStarted;
            _questService.SentQuestion += OnSentQuestion;
            _questService.IsCorrectAnswer += OnIsCorrectAnswer;
            _questService.IsUnCorrectAnswer += OnIsUnCorrectAnswer;
            _questService.Finished += OnFinished;
        }

        public void Create()
        {

        }

        private void OnStarted(Quest.Model.User user)
        {
            string text = "Воу воу воу!!! Кто это празднует сегодня свой День Варенья?\n\n" +
                "Легенда, супервумен, и просто потрясающая... Реджи!!!👏👏👏\n" +
                "Поздравляем с Твоим Днем Рожденья!! 🎂🎉\n\n" +
                "И хоть мы не можем обнять тебя сегодня, но посылаем лучи любви, и более того - мы подготовили для тебя сегодня нечто необычное🤩 -целый квест🧙\n\n" +
                "Правила просты: за каждый правильный ответ ты получишь подарок, он появится автоматически после твоего ответа🧚‍♂️\n\n" +
                "Так чего же ты ждешь ? !🚀\n" +
                "Вперед!🤟";

            _task = TelegramBot.Bot.SendTextMessageAsync(user.Id, text, replyMarkup: InitializeButton(BotConst.ReadyButton));
        }

        private void OnSentQuestion(Quest.Model.User user, Quest.Model.Common.Quest quest, List<IAlbumInputMedia> album)
        {
            SendAlbum(user, album);
            DoDelay(0);
            SendQuestion(user, quest.Question.Text, quest.Answers);
        }

        private void DoDelay(int miliseconds)
        {
            while (!_task.IsCompleted) { }

            Thread.Sleep(miliseconds);
        }

        private void SendAlbum(Quest.Model.User user, List<IAlbumInputMedia> album)
        {
            if (album.Count == 0)
                return;

            if (album.Count > 1)
            {
                _task = TelegramBot.Bot.SendMediaGroupAsync(user.Id, album.GetRange(0, 10));

                foreach (var stream in album)
                    stream.Media.Content.Close();

                return;
            }

            if (album[0] is InputMediaPhoto)
            {
                InputOnlineFile ioFile = new InputOnlineFile(album[0].Media.Content);
                _task = TelegramBot.Bot.SendPhotoAsync(user.Id, ioFile);
                DoDelay(0);
                album[0].Media.Content.Close();
            }
            else if (album[0] is InputMediaAudio)
            {
                InputOnlineFile ioFile = new InputOnlineFile(album[0].Media.Content);
                _task = TelegramBot.Bot.SendAudioAsync(user.Id, ioFile);
                DoDelay(0);
                album[0].Media.Content.Close();
            }
            else if (album[0] is InputMediaVideo)
            {
                InputOnlineFile ioFile = new InputOnlineFile(album[0].Media.Content);
                InputMediaVideo imVodeo = (InputMediaVideo)album[0];
                _task = TelegramBot.Bot.SendVideoAsync(user.Id, ioFile,width: 720, height: 1200);
                DoDelay(0);
                album[0].Media.Content.Close();
            }
        }

        private void SendQuestion(Quest.Model.User user, string question, List<Quest.Model.Common.ValueObject.Answer> answers)
        {
            IReplyMarkup? buttons = InitializeButtons(answers);
            _task = TelegramBot.Bot.SendTextMessageAsync(user.Id, question, replyMarkup: buttons);
        }

        private IReplyMarkup InitializeButton(string text)
        {
            List<InlineKeyboardButton[]> buttons = new List<InlineKeyboardButton[]>();

            buttons.Add(new[] { InlineKeyboardButton.WithCallbackData(text), });

            return new InlineKeyboardMarkup(buttons);
        }

        private IReplyMarkup InitializeButtons(List<Quest.Model.Common.ValueObject.Answer> answers)
        {
            List<InlineKeyboardButton[]> buttons = new List<InlineKeyboardButton[]>(answers.Count);

            for (int i = 0; i < answers.Count; i++)
            {
                buttons.Add(new[] { InlineKeyboardButton.WithCallbackData(answers[i].Text, i.ToString()), });
            }

            return new InlineKeyboardMarkup(buttons);
        }

        private void OnIsCorrectAnswer(Quest.Model.User user, List<IAlbumInputMedia> album, string correctAnswer)
        {

            if (album != null && album.Count > 0)
            {
                _task = TelegramBot.Bot.SendTextMessageAsync(user.Id, $"Ответ: {correctAnswer}\n" +
                    $"Верно, вот твоя награда =)");
                DoDelay(0);
                SendAlbum(user, album);
            }
            else
            {
                _task = TelegramBot.Bot.SendTextMessageAsync(user.Id, "К сожалению, сейчас у меня нет награды на этот вопрос, но не расстраиваемся😘\n" +
                    "Давай пойдем дальше!");
            }

            DoDelay(0);
            _task = TelegramBot.Bot.SendTextMessageAsync(user.Id, "Нажимай, когда будешь готова.", replyMarkup: InitializeButton(BotConst.NextQuestionButton));
        }

        private void OnIsUnCorrectAnswer(Quest.Model.User user)
        {
            _task = TelegramBot.Bot.SendTextMessageAsync(user.Id, "Ответ неправильный. \n" +
                "Но не расстраивайся, попробуй еще раз! \n" +
                "Я в тебя верю!");
        }

        private void OnFinished(Quest.Model.User user)
        {
            _task = TelegramBot.Bot.SendTextMessageAsync(user.Id, "Реджи, надеемся, что тебе понравился наш необычный подарок🥰\n" +
                "Продолжай вдохновлять нас и дальше, а также освещать мир вокруг себя❤️😘\n" +
                "Еще раз с Днем Рожденья!!!🥳");
            DoDelay(0);

            FileStream stream = System.IO.File.Open($"{Directory.GetCurrentDirectory()}\\src\\Юху.mp4", FileMode.Open);
            _task = TelegramBot.Bot.SendVideoAsync(user.Id, stream);
            DoDelay(0);
            stream.Close();
        }
    }
}
