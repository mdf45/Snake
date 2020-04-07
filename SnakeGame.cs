using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using static System.Console;
using static System.Threading.Thread;

namespace Snake
{
    class SnakeGame
    {
        static Random rand = new Random();

        public static String CurrentUser = Environment.UserName;
        public static String ExploreName = "SnakeData";

        private static readonly String Path = @$"C:\Users\{CurrentUser}\Documents\{ExploreName}\";

        private static readonly String _Names = @$"C:\Users\{CurrentUser}\Documents\{ExploreName}\NamesData.json";
        private static readonly String _BestNames = @$"C:\Users\{CurrentUser}\Documents\{ExploreName}\BestNamesData.json";

        private static readonly String _Scores = @$"C:\Users\{CurrentUser}\Documents\{ExploreName}\ScoresData.json";
        private static readonly String _BestScores = @$"C:\Users\{CurrentUser}\Documents\{ExploreName}\BestScoresData.json";

        private static readonly String _Modes = @$"C:\Users\{CurrentUser}\Documents\{ExploreName}\ModesData.json";

        private static readonly String _isEasy = @$"C:\Users\{CurrentUser}\Documents\{ExploreName}\ComplexityData.json";

        private static readonly String _BST = @$"C:\Users\{CurrentUser}\Documents\{ExploreName}\BestScoreTextData.json";

        private static readonly Int32 X = 80;
        private static readonly Int32 Y = 20;

        public static String[] Names = new String[1000];
        public static String[] BestNames = new String[1000];
        public static String[] NBS = new String[1000];

        public static UInt64[] Scores = new UInt64[1000];
        public static UInt64[] BestScores = new UInt64[1000];
        public static UInt64[] SBS = new UInt64[1000];

        public static Boolean[] Modes = new Boolean[1000];
        public static Boolean[] MD = new Boolean[1000];

        private static Int16 IndexOfNames = 0;

        protected static int origRow;
        protected static int origCol;

        public static Boolean isEasy = false;

        private static String BST = "";

        protected static void Draw(string s, int x, int y)
        {
            try
            {
                SetCursorPosition(origCol + x, origRow + y);
                Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Clear();
                WriteLine(e.Message);
            }
        }
        static void AnimLoading(String Text, int x, int y)
        {
            x = x - Text.Length / 2;
            int tX = x;
            double Proc;
            double len = Text.Length * 1.0;
            Char[] TextArray = Text.ToCharArray();
            for (int i = 0; i < TextArray.Length + 1; i++)
            {
                Proc = i / len * 100;
                Draw("                                 ", x, y + 1);
                ForegroundColor = ConsoleColor.DarkYellow;
                if (i < TextArray.Length)
                    Draw(Char.ToString(TextArray[i]), tX, y);
                tX++;
                ForegroundColor = ConsoleColor.Cyan;
                Draw($"Loading {Math.Round(Proc, MidpointRounding.AwayFromZero)}%", 60 - $"Loading {Math.Round(Proc, MidpointRounding.AwayFromZero)}%".Length / 2, y + 1);
                if (tX < x + x / 2)
                    Sleep(rand.Next(100, 300));
                else
                    Sleep(40);
            }
        }
        static void AnimateHelpMenu()
        {
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("A/LEFT");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Move left");
            Sleep(150);
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("D/RIGHT");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Move right");
            Sleep(150);
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("W/UP");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Move up");
            Sleep(150);
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("S/DOWN");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Move down");
            Sleep(150);
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("SPACE");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("ON/OFF Superpower");
            Sleep(150);
            ForegroundColor = ConsoleColor.DarkGray;
            Write("#");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Complexity is Easy");
            Sleep(150);
            ForegroundColor = ConsoleColor.Red;
            Write("#");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Complexity is Hard");
            Sleep(150);
            ForegroundColor = ConsoleColor.Cyan;
            Write("#");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Superpower is Active");
        }

        static void AnimateHelpMenuForWelcome()
        {
            SetCursorPosition(X + 10, Y / 2 - 4);
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("A/LEFT");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Move left");
            Sleep(150);
            SetCursorPosition(X + 10, Y / 2 - 3);
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("D/RIGHT");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Move right");
            Sleep(150);
            SetCursorPosition(X + 10, Y / 2 - 2);
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("W/UP");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Move up");
            Sleep(150);
            SetCursorPosition(X + 10, Y / 2 - 1);
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("S/DOWN");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Move down");
            Sleep(150);
            SetCursorPosition(X + 10, Y / 2);
            ForegroundColor = ConsoleColor.DarkMagenta;
            Write("SPACE");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("ON/OFF Superpower");
            Sleep(150);
            SetCursorPosition(X + 10, Y / 2 + 1);
            ForegroundColor = ConsoleColor.DarkGray;
            Write("#");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Complexity is Easy");
            Sleep(150);
            SetCursorPosition(X + 10, Y / 2 + 2);
            ForegroundColor = ConsoleColor.Red;
            Write("#");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Complexity is Hard");
            Sleep(150);
            SetCursorPosition(X + 10, Y / 2 + 3);
            ForegroundColor = ConsoleColor.Cyan;
            Write("#");
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");
            ForegroundColor = ConsoleColor.DarkYellow;
            WriteLine("Superpower is Active");
        }
        static void Anim1(String Text, int x, int y)
        {
            int tX = x;
            Char[] TextArray = Text.ToCharArray();
            for (int i = 0; i < TextArray.Length; i++)
            {
                Draw(Char.ToString(TextArray[i]), tX, y);
                tX++;
                Sleep(40);
            }
        }

        static void Anim2(String Text, int x, int y)
        {
            int tX = x;
            Char[] TextArray = Text.ToCharArray();
            for (int i = 0; i < TextArray.Length; i++)
            {
                if (i % 2 == 0)
                {
                    Draw(Char.ToString(TextArray[i]), tX, y);
                    tX += 2;
                }

                Sleep(15);
            }
            tX = x + TextArray.Length - 1;
            for (int i = TextArray.Length - 1; i > 0; i--)
            {
                if (i % 2 != 0)
                {
                    Draw(Char.ToString(TextArray[i]), tX, y);
                    tX -= 2;
                }

                Sleep(15);
            }
        }

        static void Anim3(String Text, int x, int y)
        {
            int tX = x;
            Char[] TextArray = Text.ToCharArray();
            Anim2(Text, x, y);
            while (!KeyAvailable)
            {
                Draw(Text, x, y);
                Sleep(400);
                for (int i = 0; i < TextArray.Length; i++)
                {
                    Draw(" ", tX, y);
                    tX++;
                }
                tX = x;
                Sleep(400);
            }
            Draw(Text, x, y);
        }

        static void Anim4(String Text, int x, int y)
        {
            int tX = x;
            Char[] TextArray = Text.ToCharArray();
            Anim1(Text, x, y);
            while (!KeyAvailable)
            {
                Draw(Text, x, y);
                Sleep(400);
                for (int i = 0; i < TextArray.Length; i++)
                {
                    Draw(" ", tX, y);
                    tX++;
                }
                tX = x;
                Sleep(400);
            }
            Draw(Text, x, y);
        }
        static void StartGame()
        {
            origRow = CursorTop;
            origCol = CursorLeft;
            for (int i = 0; i < X; i++)
            {
                int j = X - 1;
                Draw("#", i, 0);
                if (j > 0)
                    Draw("#", j - i, Y - 1);
                Sleep(20);
            }
            for (int i = 0; i < Y; i++)
            {
                int j = Y - 1;
                Draw("#", 0, i);
                if (j > 0)
                    Draw("#", X - 1, j - i);
                Sleep(20);
            }
        }
        static void WelcomeGame(String text, String name, String PressAnyKeyText)
        {
            Int32 position = (X - 2) / 2 - text.Length / 2;

            ForegroundColor = ConsoleColor.Cyan;
            Anim1(text, position, Y / 2 - 1);
            position = (X - 2) / 2 - name.Length / 2;
            ForegroundColor = ConsoleColor.Green;
            Anim1(name, position, Y / 2);
            position = (X - 2) / 2 - PressAnyKeyText.Length / 2;
            AnimateHelpMenuForWelcome();
            ForegroundColor = ConsoleColor.Red;
            Anim3(PressAnyKeyText, position, Y / 2 + 1);
            ForegroundColor = ConsoleColor.Magenta;
        }
        static void Begin(Int32 speed)
        {
            Clear();
            for (int i = 0; i < X; i++)
            {
                int j = X - 1;
                Draw("#", i, 0);
                if (j > 0)
                    Draw("#", j - i, Y - 1);
                Sleep(speed);
            }
            for (int i = 0; i < Y; i++)
            {
                int j = Y - 1;
                Draw("#", 0, i);
                if (j > 0)
                    Draw("#", X - 1, j - i);
                Sleep(speed);
            }
        }

        static void ChangeColor()
        {
            for (int i = 0; i < X; i++)
            {
                int j = X - 1;
                Draw("#", i, 0);
                if (j > 0)
                    Draw("#", j - i, Y - 1);
            }
            for (int i = 0; i < Y; i++)
            {
                int j = Y - 1;
                Draw("#", 0, i);
                if (j > 0)
                    Draw("#", X - 1, j - i);
            }
        }

        static void Die(UInt64 Score)
        {
            String text = "Game over!";
            String _Retry = "Enter - retry";
            String _BackToMenu = "Esc - back to menu";
            String score;
            if (Score < 2)
            {
                score = $"You scored {Score} point";
            }
            else
                score = $"You scored {Score} points";

            Int32 position = (X - 2) / 2 - score.Length / 2;

            ForegroundColor = ConsoleColor.DarkRed;
            Begin(5);
            ForegroundColor = ConsoleColor.DarkYellow;
            Draw(score, position, Y / 2 - 1);
            ForegroundColor = ConsoleColor.DarkGreen;
            position = (X - 2) / 2 - _Retry.Length / 2;
            Draw(_Retry, position, Y / 2 + 2);
            ForegroundColor = ConsoleColor.DarkMagenta;
            position = (X - 2) / 2 - _BackToMenu.Length / 2;
            Draw(_BackToMenu, position, Y / 2 + 1);
            ForegroundColor = ConsoleColor.Red;
            position = (X - 2) / 2 - text.Length / 2;
            Anim4(text, position, Y / 2 - 2);
        }
        static void SnakeMotionEasy(Int32 xPosition, Int32 yPosition, Int32 Speed)
        {
            ConsoleKeyInfo MotionKey;

            Int32[] xSnake = new Int32[1600];
            Int32[] ySnake = new Int32[1600];

            UInt16 ScoreApple = 0;
            Int16 step = 1;
            Int32 xApple = rand.Next(1, X - 1);
            Int32 yApple = rand.Next(1, Y - 1);
            Int32 _Speed = Speed;
            ForegroundColor = ConsoleColor.Green;
            Draw("@", xApple, yApple);
            UInt32 SnakeLength = 3;
            UInt64 temp;
            Boolean isLeft = false, isTop = false, isRight = true, isBottom = false;
            Boolean isDead = false;
            Boolean isTake = false;
            Boolean isMove = false;
            Boolean isSnaked = false;
            Boolean isSpaced = false;
            Boolean __temp;
            String SnakeHead = "O", Snake = "o";
            String ScoreText = $"Score: {Scores[IndexOfNames]}";
            String ScoreAppleText = $"Apples: {ScoreApple}";
            String _temp;
            BST = "";

            for (int i = 0; i < SnakeLength + 1; i++)
            {
                xSnake[i] = xPosition - i;
                ySnake[i] = yPosition;
            }

            void CreateApple()
            {
                xApple = rand.Next(1, X - 1);
                yApple = rand.Next(1, Y - 1);

                for (uint i = SnakeLength + 1; i > 0; i--)
                {
                    if (xApple == xSnake[i] && yApple == ySnake[i])
                    {
                        xApple = rand.Next(1, X - 1);
                        yApple = rand.Next(1, Y - 1);

                        for (uint j = SnakeLength + 1; j > 0; j--)
                        {
                            if (xApple == xSnake[i] && yApple == ySnake[i])
                            {
                                xApple = rand.Next(1, X - 1);
                                yApple = rand.Next(1, Y - 1);
                            }
                        }
                    }
                }
                if (Speed > 40)
                {
                    if (isSpaced)
                    {
                        _Speed -= 3;
                        Speed = _Speed / 2;
                    }
                    if (!isSpaced)
                    {
                        Speed -= 3;
                        _Speed = Speed;
                    }
                }
                isTake = false;
            }

            void move(Int32 Speed)
            {
                for (uint i = SnakeLength + 1; i > 0; i--)
                {
                    xSnake[i] = xSnake[i - 1];
                    ySnake[i] = ySnake[i - 1];
                    if (i == 1)
                    {
                        ForegroundColor = ConsoleColor.Yellow;
                        Draw(SnakeHead, xSnake[i], ySnake[i]);
                    }
                    else if (i == SnakeLength + 1)
                    {
                        Draw(" ", xSnake[i], ySnake[i]);
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.DarkYellow;
                        Draw(Snake, xSnake[i], ySnake[i]);
                    }

                }
                if (isLeft)
                {
                    xSnake[0] -= step;
                }
                if (isRight)
                {
                    xSnake[0] += step;
                }
                if (isTop)
                {
                    ySnake[0] -= step;
                }
                if (isBottom)
                {
                    ySnake[0] += step;
                }
                for (uint i = SnakeLength + 1; i > 0; i--) // if Head pos == Body pos then Game Over
                {
                    if (xSnake[1] == xSnake[i] && ySnake[1] == ySnake[i] && i > 1)
                    {
                        isSnaked = true;
                    }
                }
                isMove = true;
                Sleep(Speed);
            }
            while (true)
            {
                CursorVisible = false;
                while (!KeyAvailable)
                {
                    ForegroundColor = ConsoleColor.DarkYellow;
                    move(Speed);
                    if (isSnaked)
                    {
                        Die(Scores[IndexOfNames]);
                        if (Scores[IndexOfNames] > BestScores[IndexOfNames])
                        {
                            BestScores[IndexOfNames] = Scores[IndexOfNames];
                            BestNames[IndexOfNames] = Names[IndexOfNames];
                            Modes[IndexOfNames] = isEasy;

                            File.WriteAllText(_BestScores, JsonConvert.SerializeObject(BestScores));
                            File.WriteAllText(_BestNames, JsonConvert.SerializeObject(BestNames));
                            File.WriteAllText(_Modes, JsonConvert.SerializeObject(Modes));

                            for (int i = 0; i < NBS.Length; i++)
                            {
                                NBS[i] = BestNames[i];
                            }
                            for (int i = 0; i < SBS.Length; i++)
                            {
                                SBS[i] = BestScores[i];
                            }
                            for (int i = 0; i < MD.Length; i++)
                            {
                                MD[i] = Modes[i];
                            }

                            for (int i = 0; i < NBS.Length - 1; i++)
                            {
                                for (int j = 0; j < NBS.Length - i - 1; j++)
                                {
                                    if (SBS[j + 1] > SBS[j])
                                    {
                                        temp = SBS[j + 1];
                                        _temp = NBS[j + 1];
                                        __temp = MD[j + 1];
                                        SBS[j + 1] = SBS[j];
                                        NBS[j + 1] = NBS[j];
                                        MD[j + 1] = MD[j];
                                        SBS[j] = temp;
                                        NBS[j] = _temp;
                                        MD[j] = __temp;
                                    }
                                }
                            }
                            for (int i = 0; i < NBS.Length; i++)
                            {
                                if (NBS[i] == null)
                                    break;
                                if (MD[i])
                                    BST += $"\t{NBS[i]} - {SBS[i]} points (Easy)\n";
                                else
                                    BST += $"\t{NBS[i]} - {SBS[i]} points (Hard)\n";
                            }
                            File.WriteAllText(_BST, JsonConvert.SerializeObject(BST));
                        }
                        Scores[IndexOfNames] = 0;
                        File.WriteAllText(_Scores, JsonConvert.SerializeObject(Scores));
                        isDead = true;
                        break;
                    }
                    else if (xSnake[0] >= X - 1)
                    {
                        xSnake[0] = 1;
                    }
                    else if (ySnake[0] >= Y - 1)
                    {
                        ySnake[0] = 1;
                    }
                    else if (xSnake[0] <= 0)
                    {
                        xSnake[0] = 78;
                    }
                    else if (ySnake[0] <= 0)
                    {
                        ySnake[0] = 18;
                    }
                    if (!isTake)
                    {
                        if (xSnake[1] == xApple && ySnake[1] == yApple)
                        {
                            Scores[IndexOfNames] += SnakeLength;
                            ScoreApple++;
                            isTake = true;
                            SnakeLength += 1;
                            ScoreAppleText = $"Apples: {ScoreApple}";
                            ScoreText = $"Score: {Scores[IndexOfNames]}";
                        }
                    }
                    if (isTake)
                    {
                        CreateApple();
                    }
                    ForegroundColor = ConsoleColor.Green;
                    Draw("@", xApple, yApple);
                    ForegroundColor = ConsoleColor.DarkGreen;
                    Draw("                  ", (X - 2) / 2 - ScoreText.Length / 2, Y + 4);
                    Draw(ScoreAppleText, (X - 2) / 2 - ScoreText.Length / 2, Y + 4);
                    ForegroundColor = ConsoleColor.DarkCyan;
                    Draw("                  ", (X - 2) / 2 - ScoreText.Length / 2, Y + 3);
                    Draw(ScoreText, (X - 2) / 2 - ScoreText.Length / 2, Y + 3);
                }

                if (isDead)
                {
                    break;
                }

                MotionKey = ReadKey(true);

                if (MotionKey.Key == ConsoleKey.Escape)
                {
                    break;
                }

                switch (MotionKey.Key)
                {
                    case ConsoleKey.Spacebar:
                        if (!isSpaced)
                        {
                            ForegroundColor = ConsoleColor.Cyan;
                            ChangeColor();
                            _Speed = Speed;
                            if (Speed > 20)
                                Speed /= 2;
                            isSpaced = true;
                        }
                        else if (isSpaced)
                        {
                            if (isEasy)
                                ForegroundColor = ConsoleColor.DarkGray;
                            else
                                ForegroundColor = ConsoleColor.Red;
                            ChangeColor();
                            Speed = _Speed;
                            isSpaced = false;
                        }
                        break;
                    case ConsoleKey.A:
                        if (!isLeft && !isRight && isMove)
                        {
                            isLeft = true;
                            isRight = false;
                            isTop = false;
                            isBottom = false;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (!isLeft && !isRight && isMove)
                        {
                            isLeft = true;
                            isRight = false;
                            isTop = false;
                            isBottom = false;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.D:
                        if (!isLeft && !isRight && isMove)
                        {
                            isLeft = false;
                            isRight = true;
                            isTop = false;
                            isBottom = false;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (!isLeft && !isRight && isMove)
                        {
                            isLeft = false;
                            isRight = true;
                            isTop = false;
                            isBottom = false;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.W:
                        if (!isTop && !isBottom && isMove)
                        {
                            isLeft = false;
                            isRight = false;
                            isTop = true;
                            isBottom = false;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (!isTop && !isBottom && isMove)
                        {
                            isLeft = false;
                            isRight = false;
                            isTop = true;
                            isBottom = false;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.S:
                        if (!isTop && !isBottom && isMove)
                        {
                            isLeft = false;
                            isRight = false;
                            isTop = false;
                            isBottom = true;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (!isTop && !isBottom && isMove)
                        {
                            isLeft = false;
                            isRight = false;
                            isTop = false;
                            isBottom = true;
                            isMove = false;
                        }
                        break;
                }
            }
        }
        static void SnakeMotionHard(Int32 xPosition, Int32 yPosition, Int32 Speed)
        {
            ConsoleKeyInfo MotionKey;

            Int32[] xSnake = new Int32[1600];
            Int32[] ySnake = new Int32[1600];

            UInt16 ScoreApple = 0;
            Int16 step = 1;
            Int32 xApple = rand.Next(1, X - 1);
            Int32 yApple = rand.Next(1, Y - 1);
            Int32 _Speed = Speed;
            ForegroundColor = ConsoleColor.Green;
            Draw("@", xApple, yApple);
            UInt32 SnakeLength = 3;
            UInt64 temp;
            Boolean isLeft = false, isTop = false, isRight = true, isBottom = false;
            Boolean isDead = false;
            Boolean isTake = false;
            Boolean isMove = false;
            Boolean isSnaked = false;
            Boolean isSpaced = false;
            Boolean __temp;
            String SnakeHead = "O", Snake = "o";
            String ScoreText = $"Score: {Scores[IndexOfNames]}";
            String ScoreAppleText = $"Apples: {ScoreApple}";
            String _temp;
            BST = "";

            for (int i = 0; i < SnakeLength + 1; i++)
            {
                xSnake[i] = xPosition - i;
                ySnake[i] = yPosition;
            }

            void CreateApple()
            {
                xApple = rand.Next(1, X - 1);
                yApple = rand.Next(1, Y - 1);

                for (uint i = SnakeLength + 1; i > 0; i--)
                {
                    if (xApple == xSnake[i] && yApple == ySnake[i])
                    {
                        xApple = rand.Next(1, X - 1);
                        yApple = rand.Next(1, Y - 1);

                        for (uint j = SnakeLength + 1; j > 0; j--)
                        {
                            if (xApple == xSnake[i] && yApple == ySnake[i])
                            {
                                xApple = rand.Next(1, X - 1);
                                yApple = rand.Next(1, Y - 1);
                            }
                        }
                    }
                }
                if (Speed > 40)
                {
                    if (isSpaced)
                    {
                        _Speed -= 3;
                        Speed = _Speed / 2;
                    }
                    if (!isSpaced)
                    {
                        Speed -= 3;
                        _Speed = Speed;
                    }
                }
                isTake = false;
            }

            void move(Int32 Speed)
            {
                if (isLeft)
                {
                    xSnake[0] -= step;
                }
                if (isRight)
                {
                    xSnake[0] += step;
                }
                if (isTop)
                {
                    ySnake[0] -= step;
                }
                if (isBottom)
                {
                    ySnake[0] += step;
                }
                for (uint i = SnakeLength + 1; i > 0; i--)
                {
                    xSnake[i] = xSnake[i - 1];
                    ySnake[i] = ySnake[i - 1];
                    if (i == 1)
                    {
                        ForegroundColor = ConsoleColor.Yellow;
                        Draw(SnakeHead, xSnake[i], ySnake[i]);
                    }
                    else if (i == SnakeLength + 1)
                    {
                        Draw(" ", xSnake[i], ySnake[i]);
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.DarkYellow;
                        Draw(Snake, xSnake[i], ySnake[i]);
                    }

                }
                for (uint i = SnakeLength + 1; i > 0; i--) // if Head pos == Body pos then Game Over
                {
                    if (xSnake[1] == xSnake[i] && ySnake[1] == ySnake[i] && i > 1)
                    {
                        isSnaked = true;
                    }
                }
                isMove = true;
                Sleep(Speed);
            }
            while (true)
            {
                CursorVisible = false;
                while (!KeyAvailable)
                {
                    ForegroundColor = ConsoleColor.DarkYellow;
                    move(Speed);
                    if (xSnake[1] <= 0 || xSnake[1] >= X - 1 || ySnake[1] <= 0 || ySnake[1] >= Y - 1 || isSnaked)
                    {
                        Die(Scores[IndexOfNames]);
                        if (Scores[IndexOfNames] > BestScores[IndexOfNames])
                        {
                            BestScores[IndexOfNames] = Scores[IndexOfNames];
                            BestNames[IndexOfNames] = Names[IndexOfNames];
                            Modes[IndexOfNames] = isEasy;

                            File.WriteAllText(_BestScores, JsonConvert.SerializeObject(BestScores));
                            File.WriteAllText(_BestNames, JsonConvert.SerializeObject(BestNames));
                            File.WriteAllText(_Modes, JsonConvert.SerializeObject(Modes));

                            for (int i = 0; i < NBS.Length; i++)
                            {
                                NBS[i] = BestNames[i];
                            }
                            for (int i = 0; i < SBS.Length; i++)
                            {
                                SBS[i] = BestScores[i];
                            }
                            for (int i = 0; i < MD.Length; i++)
                            {
                                MD[i] = Modes[i];
                            }

                            for (int i = 0; i < NBS.Length - 1; i++)
                            {
                                for (int j = 0; j < NBS.Length - i - 1; j++)
                                {
                                    if (SBS[j + 1] > SBS[j])
                                    {
                                        temp = SBS[j + 1];
                                        _temp = NBS[j + 1];
                                        __temp = MD[j + 1];
                                        SBS[j + 1] = SBS[j];
                                        NBS[j + 1] = NBS[j];
                                        MD[j + 1] = MD[j];
                                        SBS[j] = temp;
                                        NBS[j] = _temp;
                                        MD[j] = __temp;
                                    }
                                }
                            }
                            for (int i = 0; i < NBS.Length; i++)
                            {
                                if (NBS[i] == null)
                                    break;
                                if (MD[i])
                                    BST += $"\t{NBS[i]} - {SBS[i]} points (Easy)\n";
                                else
                                    BST += $"\t{NBS[i]} - {SBS[i]} points (Hard)\n";
                            }
                            File.WriteAllText(_BST, JsonConvert.SerializeObject(BST));
                        }
                        Scores[IndexOfNames] = 0;
                        File.WriteAllText(_Scores, JsonConvert.SerializeObject(Scores));
                        isDead = true;
                        break;
                    }
                    if (!isTake)
                    {
                        if (xSnake[1] == xApple && ySnake[1] == yApple)
                        {
                            Scores[IndexOfNames] += SnakeLength;
                            ScoreApple++;
                            isTake = true;
                            SnakeLength += 1;
                            ScoreAppleText = $"Apples: {ScoreApple}";
                            ScoreText = $"Score: {Scores[IndexOfNames]}";
                        }
                    }
                    if (isTake)
                    {
                        CreateApple();
                    }
                    ForegroundColor = ConsoleColor.Green;
                    Draw("@", xApple, yApple);
                    ForegroundColor = ConsoleColor.DarkGreen;
                    Draw("                  ", (X - 2) / 2 - ScoreText.Length / 2, Y + 4);
                    Draw(ScoreAppleText, (X - 2) / 2 - ScoreText.Length / 2, Y + 4);
                    ForegroundColor = ConsoleColor.DarkCyan;
                    Draw("                  ", (X - 2) / 2 - ScoreText.Length / 2, Y + 3);
                    Draw(ScoreText, (X - 2) / 2 - ScoreText.Length / 2, Y + 3);
                }

                if (isDead)
                {
                    break;
                }

                MotionKey = ReadKey(true);

                if (MotionKey.Key == ConsoleKey.Escape)
                {
                    break;
                }

                switch (MotionKey.Key)
                {
                    case ConsoleKey.Spacebar:
                        if (!isSpaced)
                        {
                            ForegroundColor = ConsoleColor.Cyan;
                            ChangeColor();
                            _Speed = Speed;
                            if (Speed > 20)
                                Speed /= 2;
                            isSpaced = true;
                        }
                        else if (isSpaced)
                        {
                            if (isEasy)
                                ForegroundColor = ConsoleColor.DarkGray;
                            else
                                ForegroundColor = ConsoleColor.Red;
                            ChangeColor();
                            Speed = _Speed;
                            isSpaced = false;
                        }
                        break;
                    case ConsoleKey.A:
                        if (!isLeft && !isRight && isMove)
                        {
                            isLeft = true;
                            isRight = false;
                            isTop = false;
                            isBottom = false;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (!isLeft && !isRight && isMove)
                        {
                            isLeft = true;
                            isRight = false;
                            isTop = false;
                            isBottom = false;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.D:
                        if (!isLeft && !isRight && isMove)
                        {
                            isLeft = false;
                            isRight = true;
                            isTop = false;
                            isBottom = false;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (!isLeft && !isRight && isMove)
                        {
                            isLeft = false;
                            isRight = true;
                            isTop = false;
                            isBottom = false;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.W:
                        if (!isTop && !isBottom && isMove)
                        {
                            isLeft = false;
                            isRight = false;
                            isTop = true;
                            isBottom = false;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        if (!isTop && !isBottom && isMove)
                        {
                            isLeft = false;
                            isRight = false;
                            isTop = true;
                            isBottom = false;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.S:
                        if (!isTop && !isBottom && isMove)
                        {
                            isLeft = false;
                            isRight = false;
                            isTop = false;
                            isBottom = true;
                            isMove = false;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (!isTop && !isBottom && isMove)
                        {
                            isLeft = false;
                            isRight = false;
                            isTop = false;
                            isBottom = true;
                            isMove = false;
                        }
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            Title = "Snake - Open Source project by Ivan Rezinkin";

            DirectoryInfo dirInfo = new DirectoryInfo(Path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            if (File.Exists(_Names))
                Names = JsonConvert.DeserializeObject<String[]>(File.ReadAllText(_Names));
            if (File.Exists(_BestNames))
                BestNames = JsonConvert.DeserializeObject<String[]>(File.ReadAllText(_BestNames));
            if (File.Exists(_Scores))
                Scores = JsonConvert.DeserializeObject<UInt64[]>(File.ReadAllText(_Scores));
            if (File.Exists(_BestScores))
                BestScores = JsonConvert.DeserializeObject<UInt64[]>(File.ReadAllText(_BestScores));
            if (File.Exists(_Modes))
                Modes = JsonConvert.DeserializeObject<Boolean[]>(File.ReadAllText(_Modes));
            if (File.Exists(_isEasy))
                isEasy = JsonConvert.DeserializeObject<Boolean>(File.ReadAllText(_isEasy));
            if (File.Exists(_BST))
                BST = JsonConvert.DeserializeObject<String>(File.ReadAllText(_BST));

            ConsoleKeyInfo keyInfo;

            Int16 choice;
            String text = "Welcome to the game!";
            String PressAnyKeyText = "Press Enter to play!";
            Int32 x, y;
            Int32 speed = 180;

            CursorVisible = false;

            ForegroundColor = ConsoleColor.Green;

            Anim1("Hello my dear friend and welcome to the snake game", (X - 20) - "Hello my dear friend and welcome to the snake game".Length / 2, Y - 7);


            AnimLoading("OooooooooooooooooooooooooooooooooooooooooooooooooO", X - 20, Y - 5);
            Sleep(300);
            Clear();

            CursorVisible = true;

            ForegroundColor = ConsoleColor.Cyan;
            Anim1("Welcome, what is your name?: ", 0, 0);
            ForegroundColor = ConsoleColor.Green;
            for (short i = 0; i < Names.Length; i++)
            {
                if (Names[i] == null)
                {
                    IndexOfNames = i;
                    break;
                }
            }
            while (true)
            {
                Names[IndexOfNames] = ReadLine();
                if (Names[IndexOfNames].Length > 1)
                {
                    for (short i = 0; i < Names.Length; i++)
                    {
                        if (Names[IndexOfNames] == Names[i] && i != IndexOfNames)
                        {
                            Names[IndexOfNames] = null;
                            IndexOfNames = i;
                            break;
                        }
                        if (Names[i] == null)
                            break;
                    }
                    break;
                }
                else
                {
                    ForegroundColor = ConsoleColor.Red;
                    Draw("                               ", 0, 1);
                    Anim1("Try again enter your name: ", 0, 1);
                    ForegroundColor = ConsoleColor.Green;
                }
            }
            File.WriteAllText(_Names, JsonConvert.SerializeObject(Names));
            back:
            Clear();
            CursorVisible = true;
            ForegroundColor = ConsoleColor.Cyan;
            Write($"Okay, ");
            ForegroundColor = ConsoleColor.Green;
            Write(Names[IndexOfNames]);
            ForegroundColor = ConsoleColor.Cyan;
            WriteLine(". What are you want?");
            Sleep(150);
            while (true)
            {
                ForegroundColor = ConsoleColor.Yellow;
                WriteLine("1. Play");
                Sleep(150);
                WriteLine("2. Change complexity");
                Sleep(150);
                WriteLine("3. Watch best scores");
                Sleep(150);
                WriteLine("4. Change nickname");
                Sleep(150);
                WriteLine("5. Help");
                Sleep(150);
                WriteLine("6. Exit");
                Sleep(150);
                try
                {
                    ForegroundColor = ConsoleColor.Cyan;
                    Anim1("Enter your choise: ", 0, 7);
                    ForegroundColor = ConsoleColor.Green;
                    choice = Int16.Parse(ReadLine());
                }
                catch (Exception ex)
                {
                    Clear();
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine(ex.Message);
                    continue;
                }
                switch (choice)
                {
                    case 1:
                        CursorVisible = false;
                        ForegroundColor = ConsoleColor.Magenta;
                        Clear();
                        StartGame();
                        WelcomeGame(text, Names[IndexOfNames], PressAnyKeyText);
                        while (true)
                        {
                            keyInfo = ReadKey(true);
                            if (keyInfo.Key == ConsoleKey.Enter)
                            {
                                if (isEasy)
                                    ForegroundColor = ConsoleColor.DarkGray;
                                else
                                    ForegroundColor = ConsoleColor.Red;
                                x = rand.Next(X / 4, X - X / 4); y = rand.Next(Y / 4, Y - (Y / 4));
                                Begin(10);
                                if (isEasy)
                                    SnakeMotionEasy(x, y, speed);
                                else
                                    SnakeMotionHard(x, y, speed);
                                while (true)
                                {
                                    keyInfo = ReadKey(true);
                                    if (keyInfo.Key == ConsoleKey.Escape)
                                    {
                                        goto back;
                                    }
                                    else if (keyInfo.Key == ConsoleKey.Enter)
                                    {
                                        if (isEasy)
                                            ForegroundColor = ConsoleColor.DarkGray;
                                        else
                                            ForegroundColor = ConsoleColor.Red;
                                        x = rand.Next(X / 4, X - X / 4); y = rand.Next(Y / 4, Y - Y / 4);
                                        Begin(0);
                                        if (isEasy)
                                            SnakeMotionEasy(x, y, speed);
                                        else
                                            SnakeMotionHard(x, y, speed);
                                    }
                                }
                            }
                        }
                    case 2:
                        CursorVisible = false;
                        ForegroundColor = ConsoleColor.DarkGray;
                        Clear();
                        Write("Choice complexity ( ");
                        Write("Currently is ");
                        if (isEasy)
                        {
                            ForegroundColor = ConsoleColor.Green;
                            Write("Easy");
                        }
                        else
                        {
                            ForegroundColor = ConsoleColor.Red;
                            Write("Hard");
                        }
                        ForegroundColor = ConsoleColor.DarkGray;
                        WriteLine(" )");
                        Sleep(150);
                        WriteLine();
                        ForegroundColor = ConsoleColor.White;
                        if (isEasy)
                            Write(" o");
                        else
                            Write("  ");
                        ForegroundColor = ConsoleColor.Green;
                        WriteLine(" Click Space so change complexity to Easy");
                        Sleep(150);
                        ForegroundColor = ConsoleColor.White;
                        if (!isEasy)
                            Write(" o");
                        else
                            Write("  ");
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine(" Click Enter so change complexity to Hard");
                        Sleep(150);
                        WriteLine();
                        ForegroundColor = ConsoleColor.Cyan;
                        WriteLine("Click Esc to back in main menu");
                        while (true)
                        {
                            keyInfo = ReadKey(true);
                            if (keyInfo.Key == ConsoleKey.Escape)
                            {
                                goto back;
                            }
                            if (keyInfo.Key == ConsoleKey.Spacebar)
                            {
                                isEasy = true;
                                File.WriteAllText(_isEasy, JsonConvert.SerializeObject(isEasy));
                                ForegroundColor = ConsoleColor.White;
                                Draw("o", 1, 2);
                                Draw(" ", 1, 3);
                                ForegroundColor = ConsoleColor.Green;
                                Anim1("Easy", 33, 0);
                            }
                            if (keyInfo.Key == ConsoleKey.Enter)
                            {
                                isEasy = false;
                                File.WriteAllText(_isEasy, JsonConvert.SerializeObject(isEasy));
                                ForegroundColor = ConsoleColor.White;
                                Draw(" ", 1, 2);
                                Draw("o", 1, 3);
                                ForegroundColor = ConsoleColor.Red;
                                Anim1("Hard", 33, 0);
                            }
                        }
                    case 3:
                        CursorVisible = false;
                        if (File.Exists(_BST))
                        {
                            BST = JsonConvert.DeserializeObject<String>(File.ReadAllText(_BST));
                        }
                        ForegroundColor = ConsoleColor.Magenta;
                        Clear();
                        if (BST != null && !BST.Equals(""))
                        {
                            WriteLine("Best scores: ");
                            ForegroundColor = ConsoleColor.Blue;
                            WriteLine(BST);
                        }
                        else
                        {
                            WriteLine($"Best scores: ");
                            ForegroundColor = ConsoleColor.Blue;
                            WriteLine("\tList is empty\n");
                        }
                        ForegroundColor = ConsoleColor.Cyan;
                        WriteLine("Click Esc to back in main menu");
                        while (true)
                        {
                            keyInfo = ReadKey(true);
                            if (keyInfo.Key == ConsoleKey.Escape)
                            {
                                goto back;
                            }
                        }
                    case 4:
                        Clear();
                        ForegroundColor = ConsoleColor.Cyan;
                        Write("Currently name is ");
                        ForegroundColor = ConsoleColor.Green;
                        Write(Names[IndexOfNames]);
                        ForegroundColor = ConsoleColor.Cyan;
                        WriteLine(",( 1 - to back )");
                        Anim1("Enter new name: ", 0, 1);
                        Int16 _name = IndexOfNames;
                        for (short i = 0; i < Names.Length; i++)
                        {
                            if (Names[i] == null)
                            {
                                IndexOfNames = i;
                                break;
                            }
                        }
                        while (true)
                        {
                            ForegroundColor = ConsoleColor.Green;
                            Names[IndexOfNames] = ReadLine();
                            if (Names[IndexOfNames].Equals("1"))
                            {
                                Names[IndexOfNames] = null;
                                IndexOfNames = _name;
                                break;
                            }
                            else if (Names[IndexOfNames].Length > 1)
                            {
                                for (short i = 0; i < Names.Length; i++)
                                {
                                    if (Names[IndexOfNames] == Names[i] && i != IndexOfNames)
                                    {
                                        Names[IndexOfNames] = null;
                                        IndexOfNames = i;
                                        break;
                                    }
                                    if (Names[i] == null)
                                        break;
                                }
                                break;
                            }
                            else
                            {
                                ForegroundColor = ConsoleColor.Red;
                                Draw("                               ", 0, 1);
                                Anim1("Try again enter your name: ", 0, 1);
                                ForegroundColor = ConsoleColor.Green;
                            }
                        }
                        File.WriteAllText(_Names, JsonConvert.SerializeObject(Names));
                        goto back;
                    case 5:
                        Clear();
                        CursorVisible = false;
                        AnimateHelpMenu();
                        WriteLine();
                        Sleep(500);
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine("Click Esc to back in main menu");
                        while (true)
                        {
                            keyInfo = ReadKey(true);
                            if (keyInfo.Key == ConsoleKey.Escape)
                                goto back;
                        }
                    case 6:
                        Clear();
                        ForegroundColor = ConsoleColor.Magenta;
                        WriteLine("Exit...");
                        break;
                    default:
                        Clear();
                        ForegroundColor = ConsoleColor.Red;
                        Write($"Oops... ");
                        ForegroundColor = ConsoleColor.Green;
                        Write(Names[IndexOfNames]);
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine(", please retry");
                        continue;
                }
                break;
            }
            ReadKey();
        }
    }
}
