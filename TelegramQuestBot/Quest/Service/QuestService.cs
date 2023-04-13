using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Telegram.Bot.Types;

namespace TelegramQuestBot.Quest.Service
{
    public class QuestService : IService
    {
        private TextHandler _textHandler;

        private Model.Quiz _quiz;
        private static string _playersPath;
        private string _additionFileDirectoryPath;
        private string _rewardDirectoryPath;
        private string[] _playerFilesWithPath;
        private char[] _seperators = new char[] { '_', '.' };
        private static QuestService _service;

        public Action<Model.User> Started;
        public Action<Model.User, List<IAlbumInputMedia>, string> IsCorrectAnswer;
        public Action<Model.User> IsUnCorrectAnswer;
        public Action<Model.User, Model.Common.Quest, List<IAlbumInputMedia>> SentQuestion;
        public Action<Model.User> Finished;

        private QuestService(string playersPath, string quizFullPath, string additionFileDirectoryPath, string rewardDirectoryPath)
        {
            _textHandler = TextHandler.Instantiate();
            _textHandler.StartedQuest += OnStartedQuest;
            _textHandler.SentResetQuest += OnResetedQuest;
            _playersPath = playersPath;
            _additionFileDirectoryPath = additionFileDirectoryPath;
            _rewardDirectoryPath = rewardDirectoryPath;
            _quiz = JsonConvert.DeserializeObject<Model.Quiz>(System.IO.File.ReadAllText(quizFullPath));
        }

        public static QuestService Instantiate(string playersPath = null, string quizFullPath = null, string additionFileDirectoryPath = null, string rewardDirectoryPath = null)
        {
            if (_service == null)
            {
                _service = new QuestService(playersPath, quizFullPath, additionFileDirectoryPath, rewardDirectoryPath);
            }

            return _service;
        }

        public void Start(Model.User user)
        {
            if (TryGetFileName(user, out string _))
                return;

            string userFileName = $"{Model.User.GetFileName(user)}";
            AddFile(userFileName);
            Started?.Invoke(user);
        }

        public void Update()
        {

        }

        public void End()
        {

        }

        private void OnStartedQuest(Model.User user)
        {
            Start(user);
        }

        private void AddFile(string userFileName)
        {
            System.IO.File.Create($"{_playersPath}\\{userFileName}").Close();
        }

        public void StartNewQuest(Model.User user)
        {
            Model.Session.QuizSession session = new Model.Session.QuizSession(user);

            Serialize(session);
            SendQuestion(user);
        }

        public static void Serialize(Model.Session.QuizSession session)
        {
            string userFileName = $"{Model.User.GetFileName(session.Player)}";
            string filePath = $"{_playersPath}\\{userFileName}";

            using (StreamReader reader = new StreamReader(filePath))
            {
                string fileContents = reader.ReadToEnd();
                reader.Close();

                string[] lines = fileContents.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                lines[0] = JsonConvert.SerializeObject(session);
                fileContents = string.Join(Environment.NewLine, lines);

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(fileContents);
                    writer.Close();
                }
            }
        }

        private bool TryGetFileName(Model.User user, out string fileNameWithPath)
        {
            fileNameWithPath = string.Empty;
            _playerFilesWithPath = Directory.GetFiles(_playersPath);

            foreach (var playerFileWithPath in _playerFilesWithPath)
            {
                string fileName = System.IO.Path.GetFileName(playerFileWithPath);
                string[] namePathes = fileName.Split(_seperators);

                if (namePathes[0] == user.Id.ToString())
                {
                    fileNameWithPath = playerFileWithPath;
                    return true;
                }
            }

            return false;
        }

        private Model.Session.QuizSession GetSessionFromFile(Model.User user)
        {
            Model.Session.QuizSession session = null;

            if (TryGetFileName(user, out string fileNameWithPath))
            {
                using (StreamReader reader = new StreamReader(fileNameWithPath))
                {
                    string fileContents = reader.ReadToEnd();
                    reader.Close();

                    string[] lines = fileContents.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    session = JsonConvert.DeserializeObject<Model.Session.QuizSession>(lines[0]);
                }
            }

            return session;
        }

        private Model.Common.Quest GetQuestion(Model.Session.QuizSession session) =>
            session.GetNewQuestion(_quiz);

        private bool TryCompliteQuestion(Model.User user, int answerNumber)
        {
            Model.Session.QuizSession session = GetSessionFromFile(user);

            bool isCorrectAnswer = session.CheckAnswer(answerNumber);

            if (isCorrectAnswer)
            {
                CompliteQuestion(session, answerNumber);

                return true; 
            }

            IsUnCorrectAnswer?.Invoke(user);

            return false;
        }

        private void CompliteQuestion(Model.Session.QuizSession session, int answerNumber)
        {
            Model.Common.Quest quest = session.CurrentQuestion;

            List<IAlbumInputMedia> reward = quest.GetAdditionFiles(_rewardDirectoryPath);

            IsCorrectAnswer?.Invoke(session.Player, reward, quest.Answers[answerNumber].Text);
            session.CompliteCurrentQuestion();
        }

        public void SendQuestion(Model.User user)
        {
            Model.Session.QuizSession session = GetSessionFromFile(user);
            Model.Common.Quest quest = GetQuestion(session);
            
            if(quest == null)
            {
                Finished?.Invoke(session.Player);
                return;
            }
                
            List<IAlbumInputMedia> album = quest.GetAdditionFiles(_additionFileDirectoryPath);

            SentQuestion?.Invoke(session.Player, quest, album);
        }

        private void OnResetedQuest(Model.User user)
        {
            StartNewQuest(user);
        }

        public bool OnAnswered(Model.User user, int answer)
        {
            return TryCompliteQuestion(user, answer);
        }
    }
}
