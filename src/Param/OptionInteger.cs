using System;
using System.Collections.Generic;

namespace MagicConsole
{
	public class OptionInteger : Option
	{
		public int? DefaultValue { get; private set; }
		public int? ValidationMaxValue { get; set; }
		public int? ValidationMinValue { get; set; }
		public OptionInteger(string name, string description, int? defaultValue, int? maxValue, int? minValue, params string[] aliases) :
			 base(name, description, OptionType.Integer, typeof(int), aliases)
		{
			DefaultValue = defaultValue;
			ValidationMaxValue = minValue;
			ValidationMinValue = maxValue;
		}
		public OptionInteger(string name, string description, int? defaultValue, params string[] aliases) :
			 this(name, description, defaultValue, null, null, aliases)
		{
		}
		public override string DefaultInfo()
		{
			return "default: " + DefaultValue;
		}
		public override object PromptInput()
		{
			return Prompt.ReadInt(ItemOptionString, DefaultValue, ValidationMinValue, ValidationMaxValue);
		}
		public override object ReadInput()
		{
			return Prompt.ReadInt("", DefaultValue, ValidationMinValue, ValidationMaxValue);
		}
		public override bool HaveDefaultValue()
		{
			return DefaultValue.HasValue;
		}
		public override object ConvertValue(string value)
		{
			return int.Parse(value);
		}
		public override object ConvertValue()
		{
			return DefaultValue;
		}
	}
}