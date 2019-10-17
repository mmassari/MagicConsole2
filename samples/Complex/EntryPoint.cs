using MagicConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ComplexApp
{
    class EntryPoint
    {
        enum Fruit
        {
            Apple,
            Banana,
            Coconut
        }
        public static void Main(params string[] args)
        {
            //var defaultValue = true;
            //string defaultText;
            //if (defaultValue)
            //    defaultText = $"({Strings.Flag.Inputs[true].Inputs[0].ToUpper()}/{Strings.Flag.Inputs[false].Inputs[0].ToLower()})";
            //else
            //    defaultText = $"({Strings.Flag.Inputs[true].Inputs[0].ToLower()}/{Strings.Flag.Inputs[false].Inputs[0].ToUpper()})";

            //Prompt.ReadEnum("Select a fruit:", Fruit.Apple);
            //Prompt.ReadFlag(string.Format(Strings.Flag.Prompt, defaultText), Strings.Flag.Inputs, true);
            //Prompt.ReadInt(Strings.Integer.Prompt, null, 1, 20);
            //Prompt.ReadDate(Strings.Date.Prompt, null, new DateTime(2018, 1, 1), new DateTime(2020, 1, 1));
            //Prompt.ReadDateTime(Strings.DateTime.Prompt, null, new DateTime(2019, 1, 1, 22, 50, 0));
            //Prompt.ReadTime(Strings.Time.Prompt, null, new TimeSpan(9, 0, 0), new TimeSpan(18, 0, 0));


            //Console.ReadLine();
            new SyncProgram()
                .LoadFromFile("magic.json")
                .Start(args);
        }
    }
}
