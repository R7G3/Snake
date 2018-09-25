using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
	class Program
	{
		public static IDictionary<string, Action> menuEntries = new Dictionary<string, Action>
		{
			{ "Start", () => {
					Game.Initialization(key);
					handler = null;
				} },
			{ "Settings", () => Menu.Show(settingsEntries, setMenu, key, handler, ref exit) },
			{ "About", AboutCommand },
			{ "Exit", ExitCommand }
		};

		public static IDictionary<string, Action> settingsEntries = new Dictionary<string, Action>
		{
			{"Scale of field", () => Settings.Size(key, sizes) },
			{"Speed", () => Settings.Speed(key) },
		};

		public static void AboutCommand()
		{
			Console.Clear();
			Console.WriteLine("Vadim \"RAGE\" Trofimov\n2018");
			Console.ReadKey();
		}

		public static void ExitCommand()
		{
			exit = true;
		}

		public static Action handler = null;
		public static bool exit = false;
		public static System.ConsoleKey key = 0;
		public static List<string> mainMenu = new List<string>() {"Start", "Settings", "About", "Exit"};
		public static List<string> setMenu = new List<string>() {"Scale of field", "Speed"};
		public static List<string> sizes = new List<string>() {"Small", "Medium", "Large", "Full"};

		static void Main(string[] args)
		{
			Console.CursorVisible = false;
			Console.ForegroundColor = ConsoleColor.Yellow;
			Menu.Show(menuEntries, mainMenu, key, handler, ref exit);
		}
	}
}