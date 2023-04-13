using System.Collections.Generic;

public class Option
{
    public List<string> Texts { get; set; }

    public Option ToCopy()
    {
        Option option = new Option();
        List<string> texts = new List<string>(Texts.Count);

        for (int i = 0; i < Texts.Count; i++)
            texts.Add(Texts[i]);

        option.Texts = texts;

        return option;
    }
}

