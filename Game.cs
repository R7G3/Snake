using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
	class SnakePart
	{
		public int Y { get; set; }
		public int X { get; set; }
		public static readonly string Symbol = "><";
	}

	class Food
	{
		public int Y { get; set; }
		public int X { get; set; }
		public static readonly string Symbol = "[]";
	}

	class HeadPart
	{
		public int Y { get; set; }
		public int X { get; set; }
	}

	class Position
	{
		public int y { get; set; }
		public int x { get; set; }

		public Position(int y, int x)
		{
			this.y = y;
			this.x = x;
		}
	}

	class Game
	{
		public static string[,] Field;
		public static Queue<SnakePart> Snake = new Queue<SnakePart>();
		public static int score = 0;
		public static Action moveHandler = null;
		public static bool stop = false;
		public static HeadPart head = new HeadPart { Y = 0, X = 0 };
		public static Food food = new Food();
		public static char[] GameOver = new char[10] { 'G', 'a', 'm', 'e', ' ', 'o', 'v', 'e', 'r', '!' };

		private static Position position = new Position(0, 0);

		public static Action switcher = null;
		public static Dictionary<System.ConsoleKey, Action> directionSwitcher = new Dictionary<System.ConsoleKey, Action>
		{
			{System.ConsoleKey.UpArrow, () => {
					position.y += -1; position.x += 0;
					} },
			{System.ConsoleKey.DownArrow, () => {
					position.y += 1; position.x += 0;
					} },
			{System.ConsoleKey.LeftArrow, () => {
					position.y += 0; position.x += -1;
					} },
			{System.ConsoleKey.RightArrow, () => {
					position.y += 0; position.x += 1;
					} }
		};

		public static void FindPath(ConsoleKey key)
		{
			directionSwitcher.TryGetValue(key, out switcher);
			switcher();
			if (Field[position.y, position.x] == Food.Symbol)
			{
				Eat(position.y, position.x);
			}
			else if (Field[head.Y - 1, head.X] == "  ")
			{
				Move(position.y, position.x);
			}
			else
			{
				EndGame();
			}
		}

		public static void EndGame()
		{
			var Y = Field.GetLength(0);
			var X = Field.GetLength(1);
			Console.SetCursorPosition(X - 5, Y/2);
			for (int i=0; i<10; i++)
			{
				Console.Write(GameOver[i]);
				Thread.Sleep(50);
			}
			Thread.Sleep(500);

			Snake.Clear();
			moveHandler = null;
			score = 0;
			stop = true;
		}

		public static void Eat(int nY, int nX)
		{
			Field[food.Y, food.X] = " ";
			GenerateFood(Field, food);
			head.Y = nY;
			head.X = nX;
			SnakePart npart = new SnakePart { Y = nY, X = nX };
			Snake.Enqueue(npart);
			score+=100;
			UpdateField(Field, food);
		}

		public static void Move(int nY, int nX)
		{
			SnakePart npart = new SnakePart { Y = nY, X = nX };
			Snake.Enqueue(npart);
			Snake.Dequeue();
			head.Y = nY;
			head.X = nX;
		}

		public static void Play(string[,] Field)
		{
			System.ConsoleKey key = System.ConsoleKey.UpArrow;
			while(!stop)
			{
				Console.Clear();
				UpdateField(Field, food);
				PrintField(Field, score, Settings.speed);
				Thread.Sleep(Settings.delay/2);

				if (Console.KeyAvailable == true)
				{
					key = Console.ReadKey().Key;
					FindPath(key);
					Thread.Sleep(Settings.delay/2);
				}
				else
				{
					FindPath(key);
					Thread.Sleep(Settings.delay/2);
				}
			};
		}

		public static void Initialization()
		{
			Settings.delay = Settings.GetDelay();
			stop = false;
			Field = new string [Settings.y, Settings.x];
			FillField(Field);
			int w = Settings.x / 2;
			int h = Settings.y / 2;
			SnakePart part1 = new SnakePart { Y = h+2, X = w };
			Snake.Enqueue(part1);
			SnakePart part2 = new SnakePart { Y = h+1, X = w };
			Snake.Enqueue(part2);
			SnakePart part3 = new SnakePart { Y = h, X = w };
			Snake.Enqueue(part3);
			position.y = part3.Y;
			position.x = part3.X;
			head.Y = h;
			head.X = w;
			UpdateField(Field, food);
			GenerateFood(Field, food);
			Play(Field);
		}

		public static void UpdateField(string[,] Field, Food food)
		{
			FillField(Field); 
			foreach (SnakePart parts in Snake)
			{
				Field[parts.Y, parts.X] = SnakePart.Symbol;
			}
			Field[food.Y, food.X] = Food.Symbol;
		}

		public static void FillField(string[,] Field)
		{
			var rows = Field.GetLength(0);
			var cols = Field.GetLength(1);
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					if(i == 0 || i == rows-1)
					{
						Field[i, j] = "░░";
					}
					else if (j == 0 || j == cols-1)
					{
						Field[i, j] = "░░";
					}
					else
					{
						Field[i, j] = "  ";
					}
				}
			}
		}

		public static void PrintField(string[,] Field, int score, int speed)
		{
			var rows = Field.GetLength(0);
			var cols = Field.GetLength(1);
			Console.Clear();
			Console.WriteLine("Speed: {0}\tScores: {1}", speed, score);
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					Console.Write(Field[i, j]);
				}
				Console.WriteLine();
			}
		}

		public static void GenerateFood(string[,] Field, Food Food)
		{
			Random random = new Random();
			var rows = Field.GetLength(0);
			var cols = Field.GetLength(1);
			bool check = true;
			do{
				int y = random.Next(1, rows-1);
				int x = random.Next(1, cols-1);
				if (Field[y,x]!=SnakePart.Symbol)
				{
					Food.Y=y;
					Food.X=x;
					check = false;
				}
			}while(check);
		}
	}
}