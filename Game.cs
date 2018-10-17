using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Snake
{
	enum FieldObject {
		SNAKE_PART,
		FOOD,
		EMPTY,
		WALL
	}


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

		public Position Add(Position other) {
			return new Position(y + other.y, x + other.x);
		}

		public static readonly Position UP = new Position(-1, 0);
		public static readonly Position DOWN = new Position(1, 0);
		public static readonly Position LEFT = new Position(0, -1);
		public static readonly Position RIGHT = new Position(0, 1);
	}

	class Game
	{
		public static string[,] Field;
		public static Queue<Position> Snake = new Queue<Position>();
		public static int score = 0;
		public static Action moveHandler = null;
		public static bool stop = false;
		public static Position head = new Position(0, 0);
		public static char[] GameOver = new char[10] { 'G', 'a', 'm', 'e', ' ', 'o', 'v', 'e', 'r', '!' };

		public static Dictionary<System.ConsoleKey, Position> directionSwitcher = new Dictionary<System.ConsoleKey, Position>
		{
			{System.ConsoleKey.UpArrow, Position.UP },
			{System.ConsoleKey.DownArrow, Position.DOWN },
			{System.ConsoleKey.LeftArrow, Position.LEFT },
			{System.ConsoleKey.RightArrow, Position.RIGHT },
		};

		public static Position direction;

		public static void FindPath(ConsoleKey key)
		{
			//Position direction;
			if (directionSwitcher.ContainsKey(key))
			{
				directionSwitcher.TryGetValue(key, out direction);
			}

			Position nextPosition = head.Add(direction);
			string nextPositionSymbol = Field[nextPosition.y, nextPosition.x];

			if (nextPositionSymbol == GetFieldObjectSymbol(FieldObject.FOOD)) {
				Eat(nextPosition);
			} else if (nextPositionSymbol == GetFieldObjectSymbol(FieldObject.EMPTY)) {
				Move(nextPosition);
			} else {
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

		private static void SetFieldObject(Position position, FieldObject fieldObject) {
			Field[position.y, position.x] = GetFieldObjectSymbol(fieldObject);
		}

		public static void Eat(Position position)
		{
			SetFieldObject(position, FieldObject.EMPTY);
			GenerateFood(Field);
			head = position;
			Snake.Enqueue(position);
			score+=100;
			UpdateField(Field);
		}

		public static void Move(Position pos)
		{
			Snake.Enqueue(pos);
			var oldPos = Snake.Peek();
			Snake.Dequeue();
			SetFieldObject(oldPos, FieldObject.EMPTY);
			head = pos;
		}

		public static void Play(string[,] Field)
		{
			System.ConsoleKey key = System.ConsoleKey.UpArrow;
			while(!stop)
			{
				Console.Clear();
				UpdateField(Field);
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
			var part1 = new Position(h+2, w);
			Snake.Enqueue(part1);
			var part2 = new Position(h+1, w);
			Snake.Enqueue(part2);
			var part3 = new Position(h, w);
			Snake.Enqueue(part3);
			head = part3;
			UpdateField(Field);
			GenerateFood(Field);
			Play(Field);
		}

		public static void UpdateField(string[,] Field)
		{
			foreach (Position part in Snake)
			{
				SetFieldObject(part, FieldObject.SNAKE_PART);
			}
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

		public static bool IsSnake(Position position)
		{
			return Field[position.y, position.x] == GetFieldObjectSymbol(FieldObject.SNAKE_PART);
		}

		public static void GenerateFood(string[,] Field)
		{
			Random random = new Random();
			var rows = Field.GetLength(0);
			var cols = Field.GetLength(1);
			bool check = true;
			do {
				int y = random.Next(1, rows-1);
				int x = random.Next(1, cols-1);
				var pos = new Position(y, x);
				if (!IsSnake(pos))
				{
					SetFieldObject(pos, FieldObject.FOOD);
					check = false;
				}
			} while(check);
		}

		public static string GetFieldObjectSymbol(FieldObject fieldObject) {
			switch (fieldObject) {
				case FieldObject.SNAKE_PART:
					return "><";
				case FieldObject.FOOD:
					return "[]";
				default:
					return "  ";
			}
		}
	}
}