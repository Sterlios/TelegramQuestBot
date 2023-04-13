using System.Collections.Generic;

public class QuizTemplate
{
    private int _currentQuestNumber = 0;
    private Quest _currentQuest;

    public IList<Quest> Quests { get; set; }

    public bool TryGetNextQuestion(out Option option, out string question, out string[] answers)
    {
        question = string.Empty;
        answers = null;
        option = null;

        if (_currentQuestNumber >= Quests.Count)
            return false;

        _currentQuest = Quests[_currentQuestNumber];
        question = _currentQuest.Question;
        answers = _currentQuest.GetAnswers();
        option = _currentQuest.GetOption();

        _currentQuestNumber++;

        return true;
    }

    public bool CheckAnswer(int answerNumber, out Reward reward)
    {
        reward = new Reward();
        return true;//_currentQuest.CheckAnswer(answerNumber, out reward);
    }
}