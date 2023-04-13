using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramQuestBot.Quest.Model.Common.ValueObject
{
    public class Answer
    {
        public Answer(string text, bool isRight)
        {
            Text = text;
            IsRight = isRight;
        }

        public string Text { get; }
        public bool IsRight { get; }
    }
}
