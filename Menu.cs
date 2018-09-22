using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
	class Menu
	{
		public static void Show(IDictionary<string, Action> menuEntries, List<string> menuList, System.ConsoleKey key, Action handler, ref bool exit)
		{
			string coursor=" ";
			int position = 0;
			string input="";

			while(!exit)
			{
				Console.Clear();
				foreach (string item in menuEntries.Keys)
				{
					if (menuList.IndexOf(item)==position)
					{
						coursor = ">";
					}
					else
					{
						coursor = " ";
					}
					Console.WriteLine("{0}{1}", coursor, item);
				}

				if (menuList[0] == "Scale of field")
				{
					Console.WriteLine("\nESC for back");
				}

				key = Console.ReadKey().Key;
				if (key == ConsoleKey.UpArrow)
				{
					if (position>0)
						position--;
				}
				else if (key == ConsoleKey.DownArrow)
				{
					if (position<menuList.Count-1)
						position++;
				}
				else if (key == ConsoleKey.Enter)
				{
					Console.Clear();
					input = menuEntries.ElementAt(position).Key;
					menuEntries.TryGetValue(input, out handler);
					handler();
				}
				else if (key == ConsoleKey.Escape)
				{
					break;
				}
			};
		}
	}
}