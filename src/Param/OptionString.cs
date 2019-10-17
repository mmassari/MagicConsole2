using System;
using System.Collections.Generic;

namespace MagicConsole
{
	public class OptionString : Option
	{
		public string DefaultValue { get; set; }
		public int ValidationMaxLength { get; set; }
		public OptionString(string name, string description, string defaultValue, params string[] aliases) :
			 base(name, description, OptionType.String, typeof(string), aliases)
		{
		}

		public override string DefaultInfo()
		{
			return "default: " + DefaultValue;
		}
		public override object PromptInput()
		{
			return Prompt.ReadString(ItemOptionString, DefaultValue, ValidationMaxLength);
		}
		public override object ReadInput()
		{
			return Prompt.ReadString(string.Empty, DefaultValue, ValidationMaxLength);
		}
		public override bool HaveDefaultValue()
		{
			return DefaultValue != null;
		}
		public override object ConvertValue(string value)
		{
			return value.Trim();
		}
		public override object ConvertValue()
		{
			return DefaultValue;
		}
	}
}