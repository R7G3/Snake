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

		public static void FindPath(ConsoleKey key)
		{
			if (key == ConsoleKey.UpArrow)
			{
				int nY = head.Y-1; int nX = head.X;
				if (nY > -1)
				{
					if (Field[head.Y - 1, head.X] == "[]")
					{
						Eat(nY, nX);
					}
					else if (Field[head.Y-1, head.X] == "  ")
					{
						Move(nY, nX);
					}
					else
					{
						EndGame();
					}
				}
			}
			if (key == ConsoleKey.DownArrow)
			{
				int nY = head.Y+1; int nX = head.X; var border = Field.GetLength(0);
				if (nY < border)
				{
					if (Field[head.Y + 1, head.X] == "[]")
					{
						Eat(nY, nX);
					}
					else if (Field[head.Y+1, head.X] == "  ")
					{
						Move(nY, nX);
					}
					else
					{
						EndGame();
					}
				}
			}
			if (key == ConsoleKey.LeftArrow)
			{
				int nY = head.Y; int nX = head.X-1;
				if (nX > -1)
				{
					if (Field[head.Y, head.X - 1] == "[]")
					{
						Eat(nY, nX);
					}
					else if (Field[head.Y, head.X - 1] == "  ")
					{
						Move(nY, nX);
					}
					else
					{
						EndGame();
					}
				}
			}
			if (key == ConsoleKey.RightArrow)
			{
				int nY = head.Y; int nX = head.X+1; var border = Field.GetLength(1);
				if (nX < border)
				{
					if (Field[head.Y, head.X + 1] == "[]")
					{
						Eat(nY, nX);
					}
					else if (Field[head.Y, head.X + 1] == "  ")
					{
						Move(nY, nX);
					}
					else
					{
						EndGame();
					}
				}
				else
				{
					EndGame();
				}
			}
			if (key == ConsoleKey.Escape)
			{
				EndGame();
			}
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
			System.ConsoleKey key = 0;
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