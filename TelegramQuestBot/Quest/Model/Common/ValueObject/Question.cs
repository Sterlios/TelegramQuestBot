using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramQuestBot.Quest.Model.Common.ValueObject
{
    public class Question
    {
        public Question(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
