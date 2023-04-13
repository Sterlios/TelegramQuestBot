using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramQuestBot.Quest.Model
{
    static class Extantion
    {
        public static void AddSession(this List<ISession> list, User user)
        {
            if (user == null)
                return;

            foreach (var session in list)
                if (session.Player.Equals(user))
                    return;

            list.Add(new Session.QuizSession(user));
        }

        public static bool ContainsUser(this List<ISession> list, User user)
        {
            if (user == null)
                return false;

            foreach (var session in list)
                if (session.Player.Equals(user))
                    return true;

            return false;
        }
        public static bool ContainsQuest(this List<ISession> list, User user)
        {
            if (user == null)
                return false;

            foreach (var session in list)
                if (session.Player.Equals(user))
                    return true;

            return false;
        }
    }
}
