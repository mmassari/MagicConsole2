using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MagicConsole
{
    public static class Prompt
    {
        private static bool ErrorPrinted;
        private static Point InputCursorPos;
        public static string LastInput;
        public static T ReadEnum<T>(string promptMessage, T defaultVal) where T : Enum
        {
            Output.DisplayPrompt(promptMessage);
            InputCursorPos = new Point(Console.CursorLeft, Console.CursorTop);
            Console.WriteLine();
            Menu menu = new Menu();

            T choice = default;
            foreach (var value in Enum.GetValues(typeof(T)))
                menu.Add(Enum.GetName(typeof(T), value), () => { choice = (T)value; });
            menu.Display();
            Console.SetCursorPosition(InputCursorPos.X, InputCursorPos.Y);
            menu.Prompt();
            LastInput = choice.ToString();
            PrintSelectedValue(LastInput, false);
            menu.Clear();
            return choice;
        }
        public static string ReadString(string promptMessage, string defaultVal = null, int maxLength = 255)
        {
            string val = null;

            Output.DisplayPrompt(promptMessage);
            while (GetInput())
            {
                if (LastInput.Length > 0 && LastInput.Length < maxLength)
                    val = LastInput;
                else if (string.IsNullOrWhiteSpace(LastInput) && defaultVal != null)
                    val = defaultVal;

                if (val != null)
                {
                    PrintSelectedValue(val, string.IsNullOrWhiteSpace(LastInput) ? true : false);
                    break;
                }
                else
                    PrintInputError(Strings.Flag.Errors.Invalid, LastInput);
            }
            return val;
        }
        public static bool ReadFlag(string promptMessage, Dictionary<bool, FlagInput> inputs, bool? defaultVal = null)
        {
            bool? val = null;

            Output.DisplayPrompt(promptMessage);
            while (GetInput())
            {
                if (LastInput.In(true, true, inputs[false].Inputs.ToArray()))
                    val = false;
                else if (LastInput.In(true, true, inputs[true].Inputs.ToArray()))
                    val = true;
                else if (string.IsNullOrWhiteSpace(LastInput) && defaultVal.HasValue)
                    val = defaultVal.Value;

                if (val.HasValue)
                {
                    PrintSelectedValue(inputs[val.Value].Text, string.IsNullOrWhiteSpace(LastInput) ? true : false);
                    break;
                }
                else
                    PrintInputError(Strings.Flag.Errors.Invalid, LastInput);
            }
            return val.Value;
        }
        public static int ReadInt(string promptMessage = "", int? defaultVal = null, int? min = null, int? max = null)
        {
            int val = 0;
            Output.DisplayPrompt(promptMessage);
            while (GetInput())
            {
                if (int.TryParse(LastInput, out val))
                {
                    if (min.HasValue && max.HasValue && (val < min || val > max))
                        PrintInputError(Strings.Integer.Errors.MinMax, min, max);
                    else if (min.HasValue && val < min)
                        PrintInputError(Strings.Integer.Errors.Min, min);
                    else if (max.HasValue && val > max)
                        PrintInputError(Strings.Integer.Errors.Max, max);
                    else
                    {
                        PrintSelectedValue(val.ToString(), false);
                        break;
                    }
                }
                else if (String.IsNullOrWhiteSpace(LastInput) && defaultVal.HasValue)
                {
                    val = defaultVal.Value;
                    PrintSelectedValue(val.ToString(), true);
                    break;
                }
                else
                    PrintInputError(Strings.Integer.Errors.Invalid, LastInput);

            }
            return val;
        }
        public static decimal ReadDecimal(string promptMessage = "", decimal? defaultVal = null, decimal? min = null, decimal? max = null)
        {
            decimal val = 0;
            Output.DisplayPrompt(promptMessage);
            while (GetInput())
            {
                if (decimal.TryParse(LastInput, out val))
                {
                    if (min.HasValue && max.HasValue && (val < min || val > max))
                        PrintInputError(Strings.Integer.Errors.MinMax, min, max);
                    else if (min.HasValue && val < min)
                        PrintInputError(Strings.Integer.Errors.Min, min);
                    else if (max.HasValue && val > max)
                        PrintInputError(Strings.Integer.Errors.Max, max);
                    else
                    {
                        PrintSelectedValue(val.ToString(), false);
                        break;
                    }
                }
                else if (String.IsNullOrWhiteSpace(LastInput) && defaultVal.HasValue)
                {
                    val = defaultVal.Value;
                    PrintSelectedValue(val.ToString(), true);
                    break;
                }
                else
                    PrintInputError(Strings.Integer.Errors.Invalid, LastInput);

            }
            return val;
        }
        public static TimeSpan ReadTime(string promptMessage = "", TimeSpan? defaultVal = null, TimeSpan? min = null, TimeSpan? max = null)
        {
            Output.DisplayPrompt(promptMessage);

            TimeSpan val = TimeSpan.Zero;
            while (GetInput())
            {
                if (TimeSpan.TryParse(LastInput, out val))
                {
                    if (min.HasValue && max.HasValue && (val < min || val > max))
                        PrintInputError(Strings.Time.Errors.MinMax, LastInput, min.Value.ToString(), max.Value.ToString());
                    else if (min.HasValue && val < min)
                        PrintInputError(Strings.Time.Errors.Min, LastInput, min.Value.ToString());
                    else if (max.HasValue && val > max)
                        PrintInputError(Strings.Time.Errors.Max, LastInput, max.Value.ToString());
                    else
                    {
                        PrintSelectedValue(val.ToString(), false);
                        break;
                    }

                }
                else if (string.IsNullOrWhiteSpace(LastInput) && defaultVal.HasValue)
                {
                    val = defaultVal.Value;
                    PrintSelectedValue(val.ToString(), true);
                }
                else
                    PrintInputError(Strings.Time.Errors.Invalid, LastInput, "HH:mm:ss");
            }
            return val;
        }
        public static DateTime ReadDateTime(string promptMessage = "", DateTime? defaultVal = null, DateTime? min = null, DateTime? max = null)
        {
            return ReadDate("dd/MM/yyyy HH:mm", promptMessage, defaultVal, min, max);
        }
        public static DateTime ReadDate(string promptMessage = "", DateTime? defaultVal = null, DateTime? min = null, DateTime? max = null)
        {
            return ReadDate("dd/MM/yyyy", promptMessage, defaultVal, min, max);
        }

        private static DateTime ReadDate(string format, string promptMessage = "", DateTime? defaultVal = null, DateTime? min = null, DateTime? max = null)
        {
            Output.DisplayPrompt(promptMessage);
            DateTime val = DateTime.MinValue;
            while (GetInput())
            {
                if (DateTime.TryParse(LastInput, out val))
                {
                    if (min.HasValue && max.HasValue && (val < min || val > max))
                        PrintInputError(Strings.Date.Errors.MinMax, LastInput, min.Value.ToString(format), max.Value.ToString(format));
                    else if (min.HasValue && val < min)
                        PrintInputError(Strings.Date.Errors.Min, LastInput, min.Value.ToString(format));
                    else if (max.HasValue && val > max)
                        PrintInputError(Strings.Date.Errors.Max, LastInput, max.Value.ToString(format));
                    else
                    {
                        PrintSelectedValue(val.ToString(format), false);
                        break;
                    }
                }
                else if (String.IsNullOrWhiteSpace(LastInput) && defaultVal.HasValue)
                {
                    val = defaultVal.Value;
                    PrintSelectedValue(val.ToString(format), true);
                    break;
                }
                else
                    PrintInputError(Strings.Date.Errors.Invalid, LastInput, format);

            }
            return val;
        }

        public static void CleanError()
        {
            if (ErrorPrinted)
            {
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, InputCursorPos.Y);
            }
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

        public static void PrintInputError(string message, params object[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(string.Format(message, args).PadRight(Console.WindowWidth));
            Console.ResetColor();
            ErrorPrinted = true;
            SetCursorInput(true);
        }

        public static void PrintSelectedValue(string value, bool isDefault)
        {
            CleanError();
            SetCursorInput(true);
            if (isDefault)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(value);
            if (isDefault)
                Console.Write(" (default)");
            Console.WriteLine("");
            Console.ResetColor();
        }

        private static bool GetInput()
        {
            InputCursorPos = new Point(Console.CursorLeft, Console.CursorTop);
            LastInput = Console.ReadLine();
            return true;
        }
    }
}
