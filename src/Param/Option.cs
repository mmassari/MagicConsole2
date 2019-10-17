using System;
using System.Collections.Generic;

namespace MagicConsole
{
	public abstract class Option : ParamBase, ICloneable
	{
		public Type DataType { get; set; }
		public OptionType OptionType { get; set; }
		public virtual string ItemOptionString => $" - {Title} ({DefaultInfo()}): ";
		public Option(string title, string description, OptionType type, Type datatype, params string[] alias) :
			 base(title, description, alias)
		{
			OptionType = type;
			DataType = datatype;
		}
		public abstract string DefaultInfo();
		public abstract bool HaveDefaultValue();
		public object Clone()
		{
			return this.MemberwiseClone();
		}
		public void PrintItemOption()
		{
			Console.Write(ItemOptionString);
		}
		public abstract object PromptInput();
		public abstract object ReadInput();
		public abstract object ConvertValue(string value);
		public abstract object ConvertValue();
	}
}