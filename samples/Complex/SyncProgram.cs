using MagicConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComplexApp
{
	public enum SyncCommands { Import, Export, Transfer }
	public enum SyncOptions { DateOption, IntegerOption, EnumOption, FlagOption }
	public enum Fruit { Orange, Banana, Coconut, Apple}
	class SyncProgram : Program<SyncCommands, SyncOptions, Fruit>
	{
		public override void ProgramStarted(SyncCommands command, Dictionary<SyncOptions, object> options)
		{
			switch (command)
			{
				case SyncCommands.Import:
				case SyncCommands.Export:
				case SyncCommands.Transfer:
					Console.WriteLine($"Inizio procedura {command.ToString()}");
					Console.WriteLine($"Opzioni selezionate:");
					foreach (KeyValuePair<SyncOptions, object> opt in options)
						Console.WriteLine($"{opt.Key.ToString()}:{opt.Value.ToString()}");

					for (int i = 0; i < 20; i++)
					{
						Console.WriteLine($"Sto lavorando... Procedura {command} - Step {i}");
						Thread.Sleep(100);
					}
					Console.WriteLine($"Fine procedura {command.ToString()}");
					break;
				default:
					break;
			}
		}

	}
}
