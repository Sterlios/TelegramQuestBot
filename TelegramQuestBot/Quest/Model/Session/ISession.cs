using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramQuestBot.Quest.Model
{
    interface ISession
    {
        public User Player { get; }
    }
}
