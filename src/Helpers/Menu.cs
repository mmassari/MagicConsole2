using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagicConsole
{
	public class Menu
	{
		private IList<MenuOption> Options { get; set; }
		private int ConsoleLineStart;
		private int ConsoleLineEnd;
		private ConsoleColor ConsoleColor;
		public int MenuWidth
		{
			get
			{
				return Options.Max(c => c.Name.Length) + Options.Count.ToString().Length + 2;
			}
		}
		public Menu(ConsoleColor color = ConsoleColor.Gray)
		{
			Options = new List<MenuOption>();
			ConsoleColor = color;
		}

		public void Display(int leftCursorPosition = 0)
		{
			Console.OutputEncoding = Encoding.ASCII;
			Console.ForegroundColor = ConsoleColor;
			ConsoleLineStart = Console.CursorTop;
			Console.SetCursorPosition(leftCursorPosition, ConsoleLineStart);
			Console.Write(new string((char)196, MenuWidth));
			for (int i = 0; i < Options.Count; i++)
			{
				Console.SetCursorPosition(leftCursorPosition, ConsoleLineStart + i);
				Console.Write("{0}. {1}", (i + 1).ToString().PadLeft(Options.Count.ToString().Length, '0'), Options[i].Name.PadRight(MenuWidth, ' '));
			}
			Console.WriteLine();
			ConsoleLineEnd = Console.CursorTop;
			Console.SetCursorPosition(leftCursorPosition, ConsoleLineEnd);
			Console.ResetColor();
		}
		public void PromptInput(string promptMessage = "")
		{
			int choice = Prompt.ReadInt(promptMessage, null, 1, Options.Count, ConsoleLineEnd + 1);
			//int choice = Input.ReadInt(promptMessage, min: 1, max: Options.Count);

			Options[choice - 1].Callback();
		}
		public Menu Add(string option, Action callback)
		{
			return Add(new MenuOption(option, callback));
		}

		public Menu Add(MenuOption option)
		{
			Options.Add(option);
			return this;
		}

		public bool Contains(string option)
		{
			return Options.FirstOrDefault((op) => op.Name.Equals(option)) != null;
		}

		internal void Clear()
		{
			for (int i = ConsoleLineStart; i <= ConsoleLineEnd; i++)
			{
				Console.SetCursorPosition(0, i);
				Console.Write(new string(' ', Console.WindowWidth));
			}
			Console.SetCursorPosition(0, ConsoleLineStart);
		}
	}
}
