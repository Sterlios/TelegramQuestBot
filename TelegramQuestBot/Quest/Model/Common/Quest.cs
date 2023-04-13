using System.Collections.Generic;
using System.IO;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace TelegramQuestBot.Quest.Model.Common
{
    public class Quest
    {
        private const string _jpg = ".jpg";
        private const string _jpeg = ".jpeg";
        private const string _png = ".png";
        private const string _mp3 = ".mp3";
        private const string _mp4 = ".mp4";
        private const string _mov = ".mov";

        private static int _ids;

        static Quest()
        {
            _ids = 0;
        }

        public Quest(ValueObject.Question question, List<ValueObject.Answer> answers)
        {
            _ids++;
            Question = question;
            Answers = answers;
            Id = _ids;
        }

        public int Id { get; set; }
        public ValueObject.Question Question { get; set; }
        public List<ValueObject.Answer> Answers { get; set; }

        public bool CheckAnswer(int answerNumber) =>
            Answers[answerNumber].IsRight;

        public List<IAlbumInputMedia> GetAdditionFiles(string additionDirectoryPath)
        {
            List<IAlbumInputMedia> album = new List<IAlbumInputMedia>();
            
            string additionalFilesDirectoryPath = $"{additionDirectoryPath}{Id}\\";

            if (Directory.Exists(additionalFilesDirectoryPath))
            {
                string[] fileNames = Directory.GetFiles(additionalFilesDirectoryPath);

                if (fileNames.Length != 0)
                {
                    string extention = System.IO.Path.GetExtension(fileNames[0]).ToLower();

                    switch (extention)
                    {
                        case _jpg:
                        case _jpeg:
                        case _png:
                            album = AddPhoto(fileNames, extention);
                            break;

                        case _mp3:
                            album = AddAudio(fileNames, extention);
                            break;

                        case _mp4:
                        case _mov:
                            album = AddVideo(fileNames, extention);
                            break;

                        default:
                            break;
                    }
                }
            }

            return album;
        }

        private List<IAlbumInputMedia> AddPhoto(string[] fileNames, string extention)
        {
            List<IAlbumInputMedia> album = new List<IAlbumInputMedia>();

            foreach (var fileName in fileNames)
            {
                if (extention == System.IO.Path.GetExtension(fileName).ToLower())
                {
                    FileStream stream = System.IO.File.Open(fileName, FileMode.Open);
                    album.Add(new InputMediaPhoto(new InputMedia(stream, System.IO.Path.GetFileName(fileName))));
                    //stream.Close();
                }
            }

            return album;
        }

        private List<IAlbumInputMedia> AddAudio(string[] fileNames, string extention)
        {
            List<IAlbumInputMedia> album = new List<IAlbumInputMedia>();

            foreach (var fileName in fileNames)
            {
                if (extention == System.IO.Path.GetExtension(fileName).ToLower())
                {
                    FileStream stream = System.IO.File.Open(fileName, FileMode.Open);
                    album.Add(new InputMediaAudio(new InputMedia(stream, System.IO.Path.GetFileName(fileName))));
                    //stream.Close();
                }
            }

            return album;
        }

        private List<IAlbumInputMedia> AddVideo(string[] fileNames, string extention)
        {
            List<IAlbumInputMedia> album = new List<IAlbumInputMedia>();

            foreach (var fileName in fileNames)
            {
                if (extention == System.IO.Path.GetExtension(fileName).ToLower())
                {
                    FileStream stream = System.IO.File.Open(fileName, FileMode.Open);
                    album.Add(new InputMediaVideo(new InputMedia(stream, System.IO.Path.GetFileName(fileName))));
                    //stream.Close();
                }
            }

            return album;
        }
    }
}
