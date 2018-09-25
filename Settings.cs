using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
	class Settings
	{
		public static int delay;
		public static int x=16;
		public static int y=8;

		public const int MAX_DELAY_IN_MS = 1000;
		public const int SPEED_STEP = 100;
		public static int speed = 6;
		public static int GetDelay()
		{
			return MAX_DELAY_IN_MS - (speed * SPEED_STEP);
		}

		public static void Speed(System.ConsoleKey key)
		{
			while (true)
			{
				Console.Clear();
				Console.Write("<");
				for (int i=1; i<10; i++)
				{
					if (i == speed)
					{
						Console.Write(speed);
					}
					else
					{
						Console.Write("=");
					}
				}
				Console.Write(">");
				Console.WriteLine("\n\nFor save - Enter, for back - ESC");

				key = Console.ReadKey().Key;
				if (key == ConsoleKey.LeftArrow)
				{
					if (speed>1)
					{speed--;}
				}
				else if (key == ConsoleKey.RightArrow)
				{
					if (speed<9)
					{speed++;}
				}
				else if (key == ConsoleKey.Enter)
				{
					Console.WriteLine("Saved speed: {0}", speed);
					Thread.Sleep(1000);
					break;
				}
				else if (key == ConsoleKey.Escape)
				{
					break;
				}
			};
		}

		public static void Size(System.ConsoleKey key, List<string> sizes)
		{
			int position = 0;
			while(true)
			{
				Console.Clear();
				for (int i = 0; i<4; i++)
				{
					if (i == position)
					{
						Console.Write("[{0}]", sizes[i]);
					}
					else
					{
						Console.Write(" {0} ", sizes[i]);
					}
				}
				Console.WriteLine("\n\nFor save - Enter, for back - ESC");

				key = Console.ReadKey().Key;
				if (key == ConsoleKey.LeftArrow)
				{
					if (position>0)
					{position--;}
				}
				else if (key == ConsoleKey.RightArrow)
				{
					if (position<3)
					{position++;}
				}
				else if (key == ConsoleKey.Enter)
				{
					if (position == 0)
					{
						y = 8; x = 16;
					}
					else if (position == 1)
					{
						y = 10; x = 20;
					}
					else if (position == 2)
					{
						y = 20; x = 40;
					}
					else if (position == 3)
					{
						y = 22; x = 79;
					}
					Console.WriteLine("Saved size: {0}", sizes[position]);
					Console.WriteLine("Debug! {0}x{1}", y, x);
					Thread.Sleep(1000);
					break;
				}
				else if (key == ConsoleKey.Escape)
				{
					break;
				}
			};
		}
	}
}