
using System.Collections.Generic;

public class Quest
{
    public Option Option { get; set; }
    public string Question { get; set; }
    public IList<Answer> Answers { get; set; }
    public Reward Reward { get; set; }

    public string[] GetAnswers()
    {
        string[] answers = new string[Answers.Count];

        for (int i = 0; i < Answers.Count; i++)
            answers[i] = Answers[i].Text;

        return answers;
    }

    public bool CheckAnswer(int answerNumber)
    {
        if (answerNumber < 0 || answerNumber >= Answers.Count)
            return false;

        return Answers[answerNumber].IsRight;
    }

    public Option GetOption()
    {
        return Option?.ToCopy();
    }
}
