using System;
using System.Collections.Generic;

namespace MagicConsole
{
	public class OptionEnum<T> : Option where T : Enum
	{
		public T DefaultValue { get; private set; }
		//public List<string> Values { get; private set; }
		public OptionEnum(string title, string description, T defaultValue, params string[] aliases) :
			 base(title, description, OptionType.Enum, typeof(T), aliases)
		{
			DefaultValue = defaultValue;
		}
		public override string DefaultInfo()
		{
			return "default: " + DefaultValue;
		}
		public override object PromptInput()
		{
			return Prompt.ReadEnum<T>(ItemOptionString, DefaultValue);
		}
		public override object ReadInput()
		{
			return Prompt.ReadEnum<T>("", DefaultValue);
		}
		public override bool HaveDefaultValue()
		{
			return true;
		}
		public override object ConvertValue(string value)
		{
			return (T)Enum.Parse(typeof(T), value);
		}
		public override object ConvertValue()
		{
			return DefaultValue;
		}
	}
}