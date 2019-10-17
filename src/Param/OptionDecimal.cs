using System;
using System.Collections.Generic;

namespace MagicConsole
{
	public class OptionDecimal : Option
	{
		public decimal? DefaultValue { get; private set; }
		public decimal? ValidationMaxValue { get; set; }
		public decimal? ValidationMinValue { get; set; }
		public OptionDecimal(string title, string description, decimal? defaultValue, decimal? maxValue, decimal? minValue, params string[] aliases) :
			 base(title, description, OptionType.Decimal, typeof(decimal), aliases)
		{
			DefaultValue = defaultValue;
			ValidationMaxValue = minValue;
			ValidationMinValue = maxValue;
		}
		public OptionDecimal(string title, string description, decimal? defaultValue, params string[] aliases) :
			 this(title, description, defaultValue, null, null, aliases)
		{
		}
		public override string DefaultInfo()
		{
			return "default: " + DefaultValue;
		}
		public override object PromptInput()
		{
			return Prompt.ReadDecimal(ItemOptionString, DefaultValue, ValidationMinValue, ValidationMaxValue);
		}

		public override object ReadInput()
		{
			return Prompt.ReadDecimal("", DefaultValue, ValidationMinValue, ValidationMaxValue);
		}
		public override bool HaveDefaultValue()
		{
			return DefaultValue.HasValue;
		}
		public override object ConvertValue(string value)
		{
			return decimal.Parse(value);
		}
		public override object ConvertValue()
		{
			return DefaultValue;
		}
	}
}