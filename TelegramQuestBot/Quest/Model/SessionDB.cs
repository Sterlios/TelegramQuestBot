using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TelegramQuestBot.Quest.Model
{
    class SessionDB
    {
        public SessionDB()
        {
            //Load();
        }

        public List<ISession> Sessions { get; private set; }

        public void AddSession(User user)
        {
            Sessions.AddSession(user);

        }

        public ISession GetSession(User user)
        {
            foreach(var session in Sessions)
            {
                if (session.Player.Equals(user))
                    return session;
            }

            return null;
        }

        public bool HasSession(User user) =>
            Sessions.ContainsUser(user);

        public void RestartSession()
        {

        }

        private void Load()
        {
            Sessions = JsonConvert.DeserializeObject<List<ISession>>(File.ReadAllText($"{Directory.GetCurrentDirectory()}\\src\\Sessions.json"));
        }
    }
}
