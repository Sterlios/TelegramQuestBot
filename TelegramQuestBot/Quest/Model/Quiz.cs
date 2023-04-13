using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TelegramQuestBot.Quest.Model
{
    public class Quiz
    {
        public Quiz(List<Common.Quest> quests)
        {
            Quests = quests;
        }

        public List<Common.Quest> Quests;

        public Common.Quest GetNextQuest(List<Common.Quest> complitedQuests) =>
            Quests.Where(quest => complitedQuests == null || !complitedQuests.Any(complitedQuest => complitedQuest.Id == quest.Id))
                .OrderBy(quest => quest.Id)
                .FirstOrDefault();
    }
}
