﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TelegramQuestBot.Quest.Model.Common.ValueObject.Option
{
    class PictureOption : IOption
    {
        public Stream Stream { get; set; }
    }
}
