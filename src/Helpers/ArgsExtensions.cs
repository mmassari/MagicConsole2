using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MagicConsole
{
    public enum Range { Ok, OutOfRange, Greater, Less }
    public static class ArgsExtensions
    {
        public static Range RangeValidate<T>(this T value, T max, T min) where T : class, IComparable
        {
            if (min != null && max != null && (value.CompareTo(min) < 0 || value.CompareTo(max) > 0))
                return Range.OutOfRange;
            else if (min != null && value.CompareTo(min) < 0)
                return Range.Less;
            else if (max != null && value.CompareTo(max) > 0)
                return Range.Greater;

            return Range.Ok;
        }
        public static bool CheckExists(this string[] args, string arg)
        {
            return args.FirstOrDefault(c => c.ToLower().Trim() == arg.ToLower().Trim()) != null;
        }
        public static bool CheckExistsAny(this string[] args, IEnumerable<string> strings)
        {
            return args.Count(c => strings.Contains(c.ToLower().Trim())) > 0;
        }
        public static T GetEnum<T>(this string[] args, T defaultValue, params Tuple<string, T>[] associations) where T : Enum
        {

            foreach (var item in associations)
            {
                var a = args.FirstOrDefault(c => String.Compare(c, item.Item1, CultureInfo.CurrentCulture,
                    CompareOptions.IgnoreSymbols | CompareOptions.IgnoreCase) == 0);

                if (a != null)
                {
                    return item.Item2;
                }
            }
            return defaultValue;
        }
        public static string PadCenter(this string source, int length, char paddingChar = ' ')
        {
            int spaces = length - source.Length;
            int padLeft = spaces / 2 + source.Length;
            return source.PadLeft(padLeft, paddingChar).PadRight(length, paddingChar);

        }
        public static T GetSafeValue<T>(this JObject s, string v, T clearConsole)
        {
            if (s.TryGetValue(v, out JToken tk))
            {
                if (typeof(T).IsEnum)
                    return (T)Enum.Parse(typeof(T), tk.Value<string>(), true);
                else
                    return tk.Value<T>();
            }


            return clearConsole;
        }
    }
}
