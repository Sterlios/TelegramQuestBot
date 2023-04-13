using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TelegramQuestBot.Quest.Model.Common.ValueObject.Option
{
    class VideoOption : IOption
    {
        public Stream Stream { get; set; }
    }
}
