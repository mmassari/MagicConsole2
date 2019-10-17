using System;
using System.Collections.Generic;

namespace MagicConsole
{
	public enum DateInputFormat { OnlyDate, DateAndTime }
	public class OptionDate : Option
	{
		public DateTime? DefaultValue { get; private set; }
		public DateInputFormat Format { get; set; }
		public DateTime? ValidationMaxValue { get; set; }
		public DateTime? ValidationMinValue { get; set; }
		public OptionDate(string title, string description, DateTime? defaultValue, DateInputFormat format, DateTime? max, DateTime? min, params string[] aliases) :
			 base(title, description, OptionType.Date, typeof(DateTime), aliases)
		{
			DefaultValue = defaultValue;
			Format = format;
			ValidationMaxValue = max;
			ValidationMinValue = min;
		}

		public OptionDate(string title, string description, DateTime? defaultValue, params string[] aliases) :
			 this(title, description, defaultValue, DateInputFormat.OnlyDate, null, null, aliases)
		{ }

		public override string DefaultInfo()
		{
			if (Format == DateInputFormat.DateAndTime)
				return "yyyy/mm/dd hh:mm:ss";
			else
				return "yyyy/mm/dd";
		}
		public override object PromptInput()
		{
			if (Format == DateInputFormat.DateAndTime)
				return Prompt.ReadDateTime(ItemOptionString, DefaultValue, ValidationMinValue, ValidationMaxValue);
			else
				return Prompt.ReadDate(ItemOptionString, DefaultValue, ValidationMinValue, ValidationMaxValue);
		}
		public override object ReadInput()
		{
			return Prompt.ReadDateTime("", DefaultValue, ValidationMaxValue, ValidationMinValue);
		}

		public override bool HaveDefaultValue()
		{
			return DefaultValue.HasValue;
		}

		public override object ConvertValue(string value)
		{
			return DateTime.Parse(value);
		}
		public override object ConvertValue()
		{
			return DefaultValue;
		}
	}
}