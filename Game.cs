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
		public string Symbol = "X";
	}

	class Food
	{
		public int Y { get; set; }
		public int X { get; set; }
		public string Symbol = "¤";
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
		public static string SSnake = "X";
		public static string SFood="¤";
		public static Action moveHandler = null;
		public static bool stop = false;
		public static HeadPart head = new HeadPart { Y = 0, X = 0 };
		public static Food food = new Food();

		public static IDictionary<System.ConsoleKey, Action> Direction = new Dictionary<System.ConsoleKey, Action>
		{
			{ ConsoleKey.UpArrow, MoveUP },
			{ ConsoleKey.DownArrow, MoveDOWN },
			{ ConsoleKey.LeftArrow, MoveLEFT },
			{ ConsoleKey.RightArrow, MoveRIGHT },
			{ ConsoleKey.Escape, EndGame }
		};

		public static void EndGame()
		{
			Snake.Clear();
			Console.WriteLine("YOU LOOSE! Scores: {0}", score); //debug
			Thread.Sleep(1000); //debug
			moveHandler = null;
			score = 0;
			stop = true;
		}

		public static void MoveUP()
		{
			int nY = head.Y-1;
			int nX = head.X;
			if (nY > -1) //-1 wtf?!
			{
				if (Field[head.Y - 1, head.X] == "¤")
				{
					Field[food.Y, food.X] = " ";
					GenerateFood(Field, SSnake, food);
					head.Y = nY;
					head.X = nX;
					SnakePart npart = new SnakePart { Y = nY, X = nX };
					Snake.Enqueue(npart);
					score+=100;
					UpdateField(Field, food);
				}
				else if (Field[head.Y-1, head.X] == " ")
				{
					SnakePart npart = new SnakePart { Y = nY, X = nX };
					Snake.Enqueue(npart);
					Snake.Dequeue();
					head.Y = nY;	// update info about head
					head.X = nX;
				}
				else
				{
					EndGame();
				}
			}
			else
			{
				EndGame(); //мб писать о проигрыше в центре экрана через set coursor position?
			}
		}

		public static void MoveDOWN()
		{
			int nY = head.Y+1;
			int nX = head.X;
			var Border = Field.GetLength(0);
			if (nY < Border)
			{
				if (Field[head.Y + 1, head.X] == "¤")
				{
					Field[food.Y, food.X] = " ";
					GenerateFood(Field, SSnake, food);
					head.Y = nY;
					head.X = nX;
					SnakePart npart = new SnakePart { Y = nY, X = nX };
					Snake.Enqueue(npart);
					score+=100;
					UpdateField(Field, food);
				}
				else if (Field[head.Y+1, head.X] == " ")
				{
					SnakePart npart = new SnakePart { Y = nY, X = nX };
					Snake.Enqueue(npart);
					Snake.Dequeue();
					head.Y = nY;
					head.X = nX;
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

		public static void MoveLEFT()
		{
			int nY = head.Y;
			int nX = head.X-1;
			if (nX > -1)
			{
				if (Field[head.Y, head.X - 1] == "¤")
				{
					Field[food.Y, food.X] = " ";
					GenerateFood(Field, SSnake, food);
					head.Y = nY;
					head.X = nX;
					SnakePart npart = new SnakePart { Y = nY, X = nX };
					Snake.Enqueue(npart);
					score+=100;
					UpdateField(Field, food);
				}
				else if (Field[head.Y, head.X - 1] == " ")
				{
					SnakePart npart = new SnakePart { Y = nY, X = nX };
					Snake.Enqueue(npart);
					Snake.Dequeue();
					head.Y = nY;
					head.X = nX;
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

		public static void MoveRIGHT()
		{
			int nY = head.Y;
			int nX = head.X+1;
			var Border = Field.GetLength(1);
			if (nX < Border)
			{
				if (Field[head.Y, head.X + 1] == "¤")
				{
					Field[food.Y, food.X] = " ";
					GenerateFood(Field, SSnake, food);
					head.Y = nY;
					head.X = nX;
					SnakePart npart = new SnakePart { Y = nY, X = nX };
					Snake.Enqueue(npart);
					score+=100;
					UpdateField(Field, food);
				}
				else if (Field[head.Y, head.X + 1] == " ")
				{
					SnakePart npart = new SnakePart { Y = nY, X = nX };
					Snake.Enqueue(npart);
					Snake.Dequeue();
					head.Y = nY;
					head.X = nX;
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

		public static void FindPath(ConsoleKey key, IDictionary<System.ConsoleKey, Action> Direction)
		{
			if (key == ConsoleKey.UpArrow)
			{
				int nY = head.Y-1; int nX = head.X;
				if (nY > -1)
				{
					if (Field[head.Y - 1, head.X] == "¤")
					{
						Eat(nY, nX);
					}
					else if (Field[head.Y-1, head.X] == " ")
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
					if (Field[head.Y + 1, head.X] == "¤")
					{
						Eat(nY, nX);
					}
					else if (Field[head.Y+1, head.X] == " ")
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
					if (Field[head.Y, head.X - 1] == "¤")
					{
						Eat(nY, nX);
					}
					else if (Field[head.Y, head.X - 1] == " ")
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
					if (Field[head.Y, head.X + 1] == "¤")
					{
						Eat(nY, nX);
					}
					else if (Field[head.Y, head.X + 1] == " ")
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
		}

		public static void Eat(int nY, int nX)
		{
			Field[food.Y, food.X] = " ";
			GenerateFood(Field, SSnake, food);
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

		public static void Play(SnakePart part, string[,] Field, Food food, int speed, int delay, System.ConsoleKey key)
		{
			while(!stop)
			{
				Console.Clear();
				UpdateField(Field, food);
				PrintField(Field, score, speed);
				Thread.Sleep(delay/2);

				if (Console.KeyAvailable == true)
				{
					//Console.Beep(300, 50);
					//Console.Beep(250, 50);
					//Console.Beep(200, 300);
					key = Console.ReadKey().Key;
					Direction.TryGetValue(key, out moveHandler);
					if (moveHandler != null)
					{
						Thread.Sleep(delay/2);
						moveHandler();
					}
				}
				else
				{
					if (moveHandler != null)
					{
						Thread.Sleep(delay/2);
						moveHandler();
					}
				}
				//Timer timer = new Timer(EmptyMethod, null, 0, delay);//maybe don't need kagbe?)
			};
		}

		/*public static void newResizeArray<T>(ref T[,] arr, int y, int x, int dimension)
		{
			T[,] newArray = new T[y, x, dimension];
			Array.Copy(arr, newArray, arr.Length);
			arr = newArray;
		}*/

		public static void Initialization(int x, int y, int speed, int delay, System.ConsoleKey key)
		{
			stop = false;
			Field = new string [y, x];
			if (Field.GetLength(0) != y)
			{
				//Array.Resize<string>(ref Field, y, x); //how to resize 2d array? :c
			}
			FillField(Field);
			int w = x / 2;
			int h = y / 2;
			SnakePart part1 = new SnakePart { Y = h+2, X = w };
			Snake.Enqueue(part1);
			SnakePart part2 = new SnakePart { Y = h+1, X = w };
			Snake.Enqueue(part2);
			SnakePart part3 = new SnakePart { Y = h, X = w };
			Snake.Enqueue(part3);
			head.Y = h;
			head.X = w;
			UpdateField(Field, food);
			GenerateFood(Field, SSnake, food);
			Play(part1, Field, food, speed, delay, key);
		}

		public static void CheckTurn() //maybe can delete?
		{
			//
		}
		
		public static void EmptyMethod(Object obj) //maybe can delete?
		{
			//yeah baby, it's so cool! (-_-)
		}

		public static void UpdateField(string[,] Field, Food food)
		{
			FillField(Field); 
			foreach (SnakePart parts in Snake)
			{
				//Console.ForegroundColor = ConsoleColor.Green;
				Field[parts.Y, parts.X] = parts.Symbol;
			}
			//Console.ForegroundColor = ConsoleColor.Cyan;
			Field[food.Y, food.X] = food.Symbol;
			//Console.ForegroundColor = ConsoleColor.Yellow;
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
						Field[i, j] = "░";
					}
					else if (j == 0 || j == cols-1)
					{
						Field[i, j] = "░";
					}
					else
					{
						Field[i, j] = " ";
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

		public static void GenerateFood(string[,] Field, string Snake, Food Food)
        {
            Random random = new Random();
            var rows = Field.GetLength(0);
			var cols = Field.GetLength(1);
            bool check = true;
            do{
                int y = random.Next(1, rows-1);
                int x = random.Next(1, cols-1);
                if (Field[y,x]!=SSnake)
                {
					Food.Y=y;
					Food.X=x;
                    check = false;
                }
            }while(check);
        }

		public static void OLDFillField(string[,] Field)
		{
			var rows = Field.GetLength(0);
			var cols = Field.GetLength(1);
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < cols; j++)
				{
					Field[i, j] = " ";
				}
			}
		}

		public static void OLDPrintField(string[,] Field, int score, int speed)
		{
			var rows = Field.GetLength(0);
			var cols = Field.GetLength(1);
			var fieldSize = rows * cols;

			Console.WriteLine("Speed: {0}\tScores: {1}", speed, score);
            Console.Write("|");
            for(int i=0; i<cols; i++)
			{
				Console.Write("-");
			}
            Console.Write("|");
			Console.WriteLine();
			
			for (int row = 0; row < rows; ++row)
			{
				Console.Write("|");
				for (int col = 0; col < cols; ++col)
				{
					Console.Write("{0}", Field[row, col]);
				}
				Console.Write("|\n");
			}

            Console.Write("|");
            for(int i=0; i<cols; i++)
			{
				Console.Write("-");
			}
            Console.Write("|\n");
            Console.WriteLine("Use arrow-buttons for moving");
		}
	}
}