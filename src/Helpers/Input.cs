using System;
using System.Drawing;

namespace MagicConsole
{
    public static class Input
    {
        private static bool ErrorPrinted;
        private static Point InputCursorPos;// int CursorLeft;
        public static string LastInput;

        public static int ReadInt(string prompt, int min, int max)
        {
            Output.DisplayPrompt(prompt);
            RegisterPromptPosition();
            return ReadInt(min, max);
        }
        internal static void RegisterPromptPosition()
        {
            ErrorPrinted = false;
            InputCursorPos = new Point(Console.CursorLeft, Console.CursorTop);
        }
        public static void SetCursorInput(bool clean)
        {
            if (clean)
            {
                Console.SetCursorPosition(InputCursorPos.X, InputCursorPos.Y);
                Console.Write(new string(' ', LastInput.Length));
            }
            Console.SetCursorPosition(InputCursorPos.X, InputCursorPos.Y);
        }

        public static int ReadInt(int min, int max)
        {
            int value = ReadInt();

            while (value < min || value > max)
            {
                Output.DisplayError("Please enter an integer between {0} and {1} (inclusive)", min, max);
                value = ReadInt();
            }

            return value;
        }

        public static int ReadInt()
        {
            string input = Console.ReadLine();
            int value;

            while (!int.TryParse(input, out value))
            {
                Output.DisplayPrompt("Please enter an integer");
                input = Console.ReadLine();
            }

            return value;
        }

        public static string ReadString(string prompt)
        {
            Output.DisplayPrompt(prompt);
            return Console.ReadLine();
        }

        public static TEnum ReadEnum<TEnum>(string prompt) where TEnum : struct, IConvertible, IComparable, IFormattable
        {
            Type type = typeof(TEnum);

            if (!type.IsEnum)
                throw new ArgumentException("TEnum must be an enumerated type");

            Output.WriteLine(prompt);
            Menu menu = new Menu();

            TEnum choice = default(TEnum);
            foreach (var value in Enum.GetValues(type))
                menu.Add(Enum.GetName(type, value), () => { choice = (TEnum)value; });
            menu.Display();

            return choice;
        }
    }
}
