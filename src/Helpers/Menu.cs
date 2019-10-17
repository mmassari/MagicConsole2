using System;
using System.Collections.Generic;
using System.Linq;

namespace MagicConsole
{
    public class Menu
    {
        private IList<MenuOption> Options { get; set; }
        private int ConsoleLineStart;
        private int ConsoleLineEnd;
        public Menu()
        {
            Options = new List<MenuOption>();
        }

        public void Display()
        {
            ConsoleLineStart = Console.CursorTop;

            for (int i = 0; i < Options.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, Options[i].Name);
            }
            ConsoleLineEnd = Console.CursorTop;
        }
        public void Prompt(string promptMessage="")
        {
            int choice = Input.ReadInt(promptMessage, min: 1, max: Options.Count);

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
