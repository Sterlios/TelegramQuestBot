using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TelegramQuestBot.Quest.Service;

namespace TelegramQuestBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText($"{Directory.GetCurrentDirectory()}\\src\\config.json"));

            string playersPath = $"{Directory.GetCurrentDirectory()}\\{config.Direction.PlayersPath}";
            string quizFullPath = $"{Directory.GetCurrentDirectory()}\\{config.Direction.QuizPath}";
            string additionFilePath = $"{Directory.GetCurrentDirectory()}\\{config.Direction.AdditionFileDirectoryPath}";
            string rewardDirectoryPath = $"{Directory.GetCurrentDirectory()}\\{config.Direction.RewardDirectoryPath}";

            QuestService questService = QuestService.Instantiate(playersPath, quizFullPath, additionFilePath, rewardDirectoryPath);
            MessageConstructor messageConstructor = new MessageConstructor();

            TelegramBot bot = new TelegramBot(config.Token, questService, messageConstructor);
            bot.Run();
        }
    }
}
