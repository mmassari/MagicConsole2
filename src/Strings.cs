using System;
using System.Collections.Generic;
using System.Text;

namespace MagicConsole
{
    public static class Strings
    {
        public static class Flag
        {
            public static string Prompt = "Please enter {0}: ";
            public static Dictionary<bool, FlagInput> Inputs
            {
                get
                {
                    return new Dictionary<bool, FlagInput>()
                    {
                        [true] = new FlagInput("Yes", "y", "yes"),
                        [false] = new FlagInput("No", "y", "no")
                    };
                }
            }
            public static class Errors
            {
                public static string Invalid = "ERROR! The value {0} is invalid. Please input {1} or {2}";
            }
        }
        public static class Integer
        {
            public static string Prompt = "Please enter an integer number: ";
            public static class Errors
            {
                public static string Invalid = "ERROR! The value is not an integer number";
                public static string MinMax = "ERROR! The value must be an integer between {0} and {1} (inclusive)";
                public static string Min = "ERROR! The value must be an integer greater than {0}";
                public static string Max = "ERROR! The value must be an integer less than {0}";
            }
        }
        public static class DateTime
        {
            public static string Prompt = "Please enter an date/time: ";
            public static class Errors
            {
                public static string Invalid = "ERROR! The value {0} is not a valid date/time ({1})";
                public static string MinMax = "ERROR! The value {0} must be a date/time between {1} and {2} (inclusive)";
                public static string Min = "ERROR! The value {0} must be a  date/time greater than {1}";
                public static string Max = "ERROR! The value {0} must be a date/time less than {1}";
            }
        }
        public static class Date
        {
            public static string Prompt = "Please enter a date: ";
            public static class Errors
            {
                public static string Invalid = "ERROR! The value {0} is not a valid date ({1})";
                public static string MinMax = "ERROR! The value {0} must be a date between {1} and {2} (inclusive)";
                public static string Min = "ERROR! The value {0} must be a date greater than {1}";
                public static string Max = "ERROR! The value {0} must be a date less than {1}";
            }
        }
        public static class Time
        {
            public static string Prompt = "Please enter a time: ";
            public static class Errors
            {
                public static string Invalid = "ERROR! The value {0} must be a valid time (HH:mm)";
                public static string MinMax = "ERROR! The value {0} must be a time between {1} and {2} (inclusive)";
                public static string Min = "ERROR! The value {0} must be a time greater than {1}";
                public static string Max = "ERROR! The value {0} must be a time less than {1}";
            }
        }
    }
}

