using System.Collections.Generic;
using TelegramQuestBot.Quest.Service;

namespace TelegramQuestBot.Quest.Model.Session
{
    public class QuizSession : ISession
    {
        public QuizSession(User player)
        {
            Player = player;
        }

        public User Player { get; }
        public Common.Quest CurrentQuestion { get; set; }
        public List<Common.Quest> ComplitedQuestions { get; set; }

        public void CompliteCurrentQuestion()
        {
            if (ComplitedQuestions == null)
                ComplitedQuestions = new List<Common.Quest>();

            ComplitedQuestions.Add(CurrentQuestion);
            CurrentQuestion = null;
            QuestService.Serialize(this);
        }

        public Common.Quest GetNewQuestion(Quiz quiz)
        {
            CurrentQuestion = quiz.GetNextQuest(ComplitedQuestions);
            QuestService.Serialize(this);

            return CurrentQuestion;
        }

        public bool CheckAnswer(int answerNumber) =>
            CurrentQuestion.CheckAnswer(answerNumber);
    }
}
