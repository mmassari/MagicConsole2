using System;

namespace MagicConsole
{
    public static class Output
    {
        public static void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(format, args);
            Console.ResetColor();
        }

        public static void WriteLine(ConsoleColor color, string value)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }

        public static void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public static void DisplayPrompt(string format, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(format)) return;
            format = format.Trim() + " ";
            Console.Write(format, args);
        }
        public static void DisplayError(string format, params object[] args)
        {
            WriteLine(ConsoleColor.Red, format, args);
        }
    }
}
