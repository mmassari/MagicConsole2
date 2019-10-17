using System;
using System.Collections.Generic;

namespace MagicConsole
{
	public class FlagInput
	{
		public List<string> Inputs { get; set; }
		public string Text { get; set; }

		public FlagInput(string text, params string[] inputs)
		{
			Text = text;
			Inputs = new List<String>(inputs);
		}
	}
	public class OptionFlag : Option
	{
		public bool? DefaultValue { get; private set; }
		public Dictionary<bool, FlagInput> Inputs { get; set; }
		public OptionFlag(string title, string description, bool? defaultValue, params string[] aliases) :
			 base(title, description, OptionType.Flag, typeof(bool), aliases)
		{
			DefaultValue = defaultValue;
			Inputs = new Dictionary<bool, FlagInput>()
			{
				[true] = new FlagInput("Yes", "y", "yes"),
				[false] = new FlagInput("No", "n", "no")
			};
		}
		public override string DefaultInfo()
		{
			if (!DefaultValue.HasValue)
				return "";
			else if (DefaultValue.Value)
				return $"{Inputs[true].Inputs[0].ToUpper()}/{Inputs[false].Inputs[0].ToLower()}";
			else
				return $"{Inputs[true].Inputs[0].ToLower()}/{Inputs[false].Inputs[0].ToUpper()}";
		}
		public override object PromptInput()
		{
			return Prompt.ReadFlag(ItemOptionString, Inputs, DefaultValue);
		}
		public override object ReadInput()
		{
			return Prompt.ReadFlag(string.Empty, Inputs, DefaultValue);
		}
		public override bool HaveDefaultValue()
		{
			return DefaultValue.HasValue;
		}
		public override object ConvertValue(string value)
		{
			return bool.Parse(value);
		}
		public override object ConvertValue()
		{
			return DefaultValue;
		}
	}
}