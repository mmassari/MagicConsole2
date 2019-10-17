using System;
using System.Collections.Generic;

namespace MagicConsole
{
	public class OptionTime : Option
	{
		public TimeSpan? DefaultValue { get; private set; }
		public TimeSpan? ValidationMaxValue { get; set; }
		public TimeSpan? ValidationMinValue { get; set; }
		public OptionTime(string name, string description, TimeSpan? defaultValue, TimeSpan? max, TimeSpan? min, params string[] aliases) :
			 base(name, description, OptionType.Date, typeof(TimeSpan), aliases)
		{
			DefaultValue = defaultValue;
			ValidationMaxValue = max;
			ValidationMinValue = min;
		}

		public OptionTime(string name, string description, TimeSpan? defaultValue, params string[] aliases) :
			  this(name, description, null, null, null, aliases)
		{ }

		public override string DefaultInfo()
		{
			return "hh:mm[:ss]";
		}
		public override object PromptInput()
		{
			return Prompt.ReadTime(ItemOptionString, DefaultValue, ValidationMaxValue, ValidationMinValue);
		}
		public override object ReadInput()
		{
			return Prompt.ReadTime("", DefaultValue, ValidationMaxValue, ValidationMinValue);
		}
		public override bool HaveDefaultValue()
		{
			return DefaultValue.HasValue;
		}
		public override object ConvertValue(string value)
		{
			return TimeSpan.Parse(value);
		}
		public override object ConvertValue()
		{
			return DefaultValue;
		}
	}
}