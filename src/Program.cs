using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MagicConsole
{

	public abstract class Program<TCommand, TOption, TEnum>
		where TCommand : struct, Enum
		where TOption : struct, Enum
		where TEnum : struct, Enum
	{
		private const string DEFAULT_MENU_HEADER = "MENU:";
		private const string DEFAULT_MENU_FOOTER = "Please choose a menu item: ";
		/// <summary>
		/// Program settings
		/// </summary>
		public static class Settings
		{
			/// <summary>
			/// Clear console when program start
			/// </summary>
			public static bool ClearConsole { get; set; }
			/// <summary>
			/// The type of menu for choose the command
			/// </summary>
			public static MenuType MenuType { get; set; }
			/// <summary>
			/// The print template for menu header
			/// </summary>
			public static string MenuHeader { get; set; }
			/// <summary>
			/// The print template for menu footer
			/// </summary>
			public static string MenuFooter { get; set; }
			/// <summary>
			/// Specify if the program have a help command
			/// </summary>
			public static bool Help { get; set; }
			/// <summary>
			/// Specify if the program have to ask to exit
			/// </summary>
			public static bool AskForExit { get; set; }
		}

		/// <summary>
		/// 
		/// </summary>
		protected JObject JsonSettings { get; private set; }
		public AssemblyInfo Info { get; }
		public bool HaveCommands { get { return Commands.Count(c => c.Value.Type == CommandTypes.Custom) > 0; } }
		public bool HaveOptions { get { return Options.Count > 0; } }
		public ExecutionMode Mode { get; set; }
		private Dictionary<TCommand, Command<TOption>> Commands { get; set; }
		private Dictionary<TOption, Option> Options { get; set; }
		public Command<TOption> HelpCommand { get; set; }
		public Command<TOption> ExitCommand { get; set; }
		public TCommand DefaultCommand { get; set; }
		public TCommand SelectedCommand { get; set; }
		public Dictionary<TOption, object> SelectedOptions { get; set; }

		public delegate void ExecuteDelegate(TCommand command, Dictionary<TOption, object> options);
		public ExecuteDelegate ExecuteMethod { get; set; }

		public Program()
		{
			Mode = ExecutionMode.Smart;
			Info = new AssemblyInfo();
			Commands = new Dictionary<TCommand, Command<TOption>>();
			Options = new Dictionary<TOption, Option>();
			SelectedCommand = default;
			SelectedOptions = new Dictionary<TOption, object>();
			Settings.ClearConsole = false;
			Settings.MenuType = MenuType.Standard;
			Settings.MenuHeader = DEFAULT_MENU_HEADER;
			Settings.MenuFooter = DEFAULT_MENU_FOOTER;
			Settings.Help = true;
			Settings.AskForExit = false;
			HelpCommand = Command<TOption>.DEFAULT_HELP_COMMAND;
			ExitCommand = Command<TOption>.DEFAULT_EXIT_COMMAND;
			ExecuteMethod = ProgramStarted;

		}

		public abstract void ProgramStarted(TCommand command, Dictionary<TOption, object> options);

		#region Load Program Settings From Json

		public Program<TCommand, TOption, TEnum> LoadFromFile(string configFile)
		{
			if (File.Exists(configFile))
				return LoadFromJson(File.ReadAllText(configFile));

			throw new FileNotFoundException();
		}
		public Program<TCommand, TOption, TEnum> LoadFromJson(string jsonContent)
		{
			return Load(JObject.Parse(jsonContent));
		}
		public Program<TCommand, TOption, TEnum> Load(JObject jsonSettings)
		{
			JsonSettings = jsonSettings;

			//Mode = JsonSettings.GetSafeValue("type", ExecutionMode.Smart);
			Mode = SetJsonValue(JsonSettings, "type", ExecutionMode.Smart);

			#region Load Program Settings

			//Se sono state definite delle settings di base le carico
			if (JsonSettings["settings"] != null)
			{
				var s = (JObject)JsonSettings["settings"];
				Settings.ClearConsole = SetJsonValue(s, "clearConsole", Settings.ClearConsole);
				Settings.MenuType = SetJsonValue(s, "menuType", Settings.MenuType);
				Settings.MenuHeader = SetJsonValue(s, "menuHeader", Settings.MenuHeader);
				Settings.MenuFooter = SetJsonValue(s, "menuFooter", Settings.MenuFooter);
				Settings.Help = SetJsonValue(s, "help", Settings.Help);
				Settings.AskForExit = SetJsonValue(JsonSettings, "askForExit", false);
			}

			#endregion

			#region Load Program Options

			if (JsonSettings["options"] != null)
			{
				var optList = (JArray)JsonSettings["options"];
				foreach (JObject j in optList)
				{
					var jType = (OptionType)Enum.Parse(typeof(OptionType), j["type"].ToString());
					Option opt;
					var enumValue = (TOption)Enum.Parse(typeof(TOption), j["name"].ToString());
					switch (jType)
					{
						case OptionType.Flag:
							opt = new OptionFlag(
								 j["title"].ToString(),
								 j["description"] != null ? j["description"].ToString() : string.Empty,
								 j.ContainsKey("default") ? j["default"].ToObject<bool>() : (bool?)null,
								 j["alias"].ToList().ConvertAll(c => c.ToString()).ToArray()
							);
							break;
						case OptionType.Date:
							opt = new OptionDate(
								 j["title"].ToString(),
								 j["description"] != null ? j["description"].ToString() : string.Empty,
								 j.ContainsKey("default") ? j["default"].ToObject<DateTime>() : (DateTime?)null,
								 j.ContainsKey("format") ? (DateInputFormat)Enum.Parse(typeof(DateInputFormat), j["format"].ToString()) : DateInputFormat.OnlyDate,
								 j.ContainsKey("max") ? j["max"].ToObject<DateTime?>() : null,
								 j.ContainsKey("min") ? j["min"].ToObject<DateTime?>() : null,
								 j["alias"].ToList().ConvertAll(c => c.ToString()).ToArray()
							);
							break;
						case OptionType.Time:
							opt = new OptionTime(
								 j["title"].ToString(),
								 j["description"] != null ? j["description"].ToString() : string.Empty,
								 j.ContainsKey("default") ? j["default"].ToObject<TimeSpan>() : (TimeSpan?)null,
								 j.ContainsKey("max") ? j["max"].ToObject<TimeSpan?>() : null,
								 j.ContainsKey("min") ? j["min"].ToObject<TimeSpan?>() : null,
								 j["alias"].ToList().ConvertAll(c => c.ToString()).ToArray()
							);
							break;
						case OptionType.Integer:
							opt = new OptionInteger(
								 j["title"].ToString(),
								 j["description"] != null ? j["description"].ToString() : string.Empty,
								 j.ContainsKey("default") ? j["default"].ToObject<int>() : (int?)null,
								 j.ContainsKey("max") ? j["max"].ToObject<int?>() : null,
								 j.ContainsKey("min") ? j["min"].ToObject<int?>() : null,
								 j["alias"].ToList().ConvertAll(c => c.ToString()).ToArray()
							);
							break;
						case OptionType.Decimal:
							opt = new OptionDecimal(
								 j["title"].ToString(),
								 j["description"] != null ? j["description"].ToString() : string.Empty,
								 j.ContainsKey("default") ? j["default"].ToObject<decimal>() : (decimal?)null,
								 j.ContainsKey("max") ? j["max"].ToObject<decimal?>() : null,
								 j.ContainsKey("min") ? j["min"].ToObject<decimal?>() : null,
								 j["alias"].ToList().ConvertAll(c => c.ToString()).ToArray()
							);
							break;
						case OptionType.Enum:
							opt = new OptionEnum<TEnum>(
								 j["title"].ToString(),
								 j["description"] != null ? j["description"].ToString() : string.Empty,
								 (TEnum)Enum.Parse(typeof(TEnum), j["default"].ToString()),
								 j["alias"].ToList().ConvertAll(c => c.ToString()).ToArray()
							);
							break;
						default:
							opt = new OptionString(
								 j["title"].ToString(),
								 j["description"] != null ? j["description"].ToString() : string.Empty,
								 j["default"].ToString(),
								 j["alias"].ToList().ConvertAll(c => c.ToString()).ToArray()
							);
							break;
					}

					Options.Add(enumValue, opt);
				}
			}

			#endregion

			#region Load Program Commands

			//Se sono state definite dei commands di base le carico
			if (JsonSettings["commands"] != null)
			{
				var cmdList = (JArray)JsonSettings["commands"];
				foreach (JObject j in cmdList)
				{
					var enumValue = (TCommand)Enum.Parse(typeof(TCommand), j["name"].ToString());

					var cmd = new Command<TOption>(
						 j["title"].ToString(),
						 j["description"] != null ? j["description"].ToString() : string.Empty,
						 j["alias"].ToList().ConvertAll(c => c.ToString()).ToArray()
					);

					if (j["menuId"] != null)
					{
						var id = int.Parse(j["menuId"].ToString());
						if (Commands.Values.Count(c => c.MenuID == id) > 0)
							throw new ArgumentException("MenuID of commands must be unique");
						cmd.MenuID = id;
					}
					else
						cmd.MenuID = (Commands.Values.Count(c => c.MenuID.HasValue) > 0 ? Commands.Values.Max(c => c.MenuID) : 0) + 1;

					cmd.ShowInMenu = j["showInMenu"] != null ? j["showInMenu"].ToObject<bool>() : true;
					foreach (var item in j["options"].ToList().ConvertAll(c => c.ToString()).ToArray())
					{
						if (Options.Count(c => c.Key.ToString() == item) > 0)
							cmd.ValidOptions.Add(Options.FirstOrDefault(c => c.Key.ToString() == item).Key);
					}

					Commands.Add(enumValue, cmd);
				}
			}
			var maxMenuId = Commands.Values.Max(c => c.MenuID);
			if (Settings.Help)
				HelpCommand.MenuID = maxMenuId++;

			ExitCommand.MenuID = maxMenuId++;

			#endregion

			return this;
		}

		#endregion

		private T SetJsonValue<T>(JObject s, string v, T clearConsole)
		{
			if (s.TryGetValue(v, out JToken tk))
			{
				if (typeof(T).IsEnum)
					return (T)Enum.Parse(typeof(T), tk.Value<string>(), true);
				else
					return tk.Value<T>();
			}


			return clearConsole;
		}

		#region AddOption
		protected OptionFlag AddOptionFlag(TOption type, string title, string description, bool defaultValue, params string[] alias)
		{
			var opt = new OptionFlag(title, description, defaultValue, alias);
			Options.Add(type, opt);
			return opt;
		}
		protected OptionString AddOptionString(TOption type, string title, string description, string defaultValue, params string[] alias)
		{
			var opt = new OptionString(title, description, defaultValue, alias);
			Options.Add(type, opt);
			return opt;
		}
		protected OptionInteger AddOptionInteger(TOption type, string title, string description, int defaultValue, params string[] alias)
		{
			var opt = new OptionInteger(title, description, defaultValue, alias);
			Options.Add(type, opt);
			return opt;
		}
		protected OptionDecimal AddOptionDecimal(TOption type, string title, string description, decimal defaultValue, decimal? max, decimal? min, params string[] alias)
		{
			var opt = new OptionDecimal(title, description, defaultValue, max, min, alias);
			Options.Add(type, opt);
			return opt;
		}
		protected OptionDate AddOptionDate(TOption type, string title, string description, DateTime defaultValue, params string[] alias)
		{
			var opt = new OptionDate(title, description, defaultValue, alias);
			Options.Add(type, opt);
			return opt;
		}
		protected OptionTime AddOptionTime(TOption type, string title, string description, TimeSpan? defaultValue, params string[] alias)
		{
			var opt = new OptionTime(title, description, defaultValue, alias);
			Options.Add(type, opt);
			return opt;
		}
		protected OptionDate AddOptionDate(TOption type, string title, string description, DateTime defaultValue, DateInputFormat format, DateTime? max, DateTime? min, params string[] alias)
		{
			var opt = new OptionDate(title, description, defaultValue, format, max, min, alias);
			Options.Add(type, opt);
			return opt;
		}
		protected OptionEnum<T> AddOptionEnum<T>(TOption type, string title, string description, T defaultValue, params string[] alias) where T : Enum
		{
			var opt = new OptionEnum<T>(title, description, defaultValue, alias);
			Options.Add(type, opt);
			return opt;
		}
		#endregion
		public virtual void Start(params string[] args)
		{
			PrintHeader();
			var cmdLineParamsOk = ReadParameters(args);

				switch (Mode)
			{
				case ExecutionMode.Smart:
					if(!cmdLineParamsOk)
						ShowMenu();
					break;
				case ExecutionMode.Silent:
					if (!ReadParameters(args))
					{
						PrintHelp();
						Environment.Exit(0);
					}						
					break;
				case ExecutionMode.Interactive:
					ShowMenu();
					break;
			}
			ExecuteMethod(SelectedCommand, SelectedOptions);

			if (Settings.AskForExit)
			{
				Console.WriteLine("Esecuzione terminata. Premi un tasto per uscire.");
				Console.Read();
			}
			else if (Mode != ExecutionMode.Silent)
				Start(args);
		}

		private bool ReadParameters(string[] args)
		{
			var cmdFound = false;
			foreach (var cmd in Commands)
			{
				if (args.CheckExistsAny(cmd.Value.AliasList))
				{
					SelectedCommand = cmd.Key;
					cmdFound = true;
					break;
				}
			}
			if (cmdFound)
			{
				foreach (var o in Commands[SelectedCommand].ValidOptions)
				{
					var opt = Options[o];
					if (args.CheckExistsAny(opt.AliasList))
					{
						if (opt.OptionType == OptionType.Flag)
							SelectedOptions[o] = true;
						else
						{
							var arg = args.First(c => opt.AliasList.Contains(c));
							var value = arg.Split(new char[] { ':', '=' })[1].Trim();
							SelectedOptions[o] = opt.ConvertValue(value);
						}
					}
					else if (opt.HaveDefaultValue())
					{
						SelectedOptions[o] = opt.ConvertValue();
					}
					else
						cmdFound = false;
				}
			}
			return cmdFound;
		}

		private void ShowMenu()
		{
			if (Settings.ClearConsole)
				Console.Clear();

			Console.WriteLine(Settings.MenuHeader);
			Console.WriteLine(string.Empty);

			foreach (var item in Commands)
				item.Value.PrintItemMenu();

			if (Settings.Help)
				HelpCommand.PrintItemMenu();

			ExitCommand.PrintItemMenu();

			Console.WriteLine(string.Empty);
			Console.Write(Settings.MenuFooter);

			var menuId = Input.ReadInt(Commands.Values.Min(c => c.MenuID).Value, ExitCommand.MenuID.Value);
			if (Commands.Count(c => c.Value.MenuID == menuId) > 0)
				SelectedCommand = Commands.First(c => c.Value.MenuID == menuId).Key;
			else if (menuId == HelpCommand.MenuID)
				PrintHelp();
			else if (menuId == ExitCommand.MenuID)
				Environment.Exit(0);

			if (Commands[SelectedCommand].ValidOptions.Count > 0)
			{
				Console.WriteLine("\nPlease specify options: ");
				foreach (var opt in Commands[SelectedCommand].ValidOptions)
					SelectedOptions[opt] = Options[opt].PromptInput();
			}
		}

		private void PrintError(string message, params string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write(message, args);
			Console.ResetColor();
		}
		private void PrintHeader()
		{
			if (Settings.ClearConsole) Console.Clear();
			Console.WriteLine(Info[InfoItem.product].ToUpper());
			Console.WriteLine(Info[InfoItem.description]);
			Console.WriteLine(Info[InfoItem.copyright]);
			Console.WriteLine(new string('-', Console.BufferWidth - 1));
			Console.WriteLine();
		}

		private void PrintHelp()
		{
			PrintHeader();

			Console.WriteLine($"\n{Info[InfoItem.filename]}" +
				 (HaveCommands ? " command" : "") +
				 (HaveOptions ? " [options]" : ""));

			Console.WriteLine("\nDescription");
			Console.WriteLine($"\t{Info[InfoItem.description]}");
			Console.WriteLine("\nList of available commands:");
			foreach (var cmd in Commands)
				Console.WriteLine($"\t{cmd.Value.AliasList[0]}\t\t{cmd.Value.Description}");

			Console.WriteLine("\nOptions list");
			foreach (var opt in Options)
				Console.WriteLine($"\t{opt.Value.AliasList[0]}\t\t{opt.Value.OptionType} ({opt.Value.DefaultInfo()})\t{opt.Value.Description}");
		}
	}
}

