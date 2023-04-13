using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramQuestBot.Quest.Model
{
    public class User
    {
        public User(Telegram.Bot.Types.User user)
        {
            Id = user.Id;

            TakeUserName(user);
            TakeFullName(user);
        }
        public User()
        {

        }

        public long Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if(!(obj is User))
                return false;

            User user = (User)obj;

            return Id == user.Id;
        }

        public static string GetFileName(User user)
        {
            string fileName = user.Id.ToString();

            if (user.UserName != string.Empty)
                fileName += $"_{user.UserName}";

            if (user.Name != string.Empty)
                fileName += $"_{user.Name}";

            fileName += $".json";

            return fileName;
        }

        private void TakeUserName(Telegram.Bot.Types.User user)
        {
            if (user.Username != string.Empty)
                UserName = user.Username;
        }

        private void TakeFullName(Telegram.Bot.Types.User user)
        {
            if (user.FirstName != string.Empty)
                Name += user.FirstName;

            if (user.LastName != string.Empty)
                Name += $" {user.LastName}";
        }

    }
}
