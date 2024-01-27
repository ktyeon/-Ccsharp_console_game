using System;
using System.Threading;

class Program
{
    static void Main()
    {
        Console.Title = "Simple Console Game";
        Console.WindowHeight = 14;
        Console.WindowWidth = 28;
        Console.BufferWidth = 28;

        Console.WriteLine("Enter your nickname:");
        string nickname = Console.ReadLine();

        Console.WriteLine($"Hello, {nickname}! Press Enter to start the game...");

        // Wait for the Enter key to be pressed
        while (Console.ReadKey().Key != ConsoleKey.Enter) { }

        GameManager.RunGame(nickname);
    }
}

class GameManager
{
    public static void RunGame(string nickname)
    {
        Game.Initialize(nickname);

        // Game loop until lives run out
        while (Game.Lives > 0)
        {
            // Play the game
            Game.Play();
        }

        // Display game over message after all lives are used
        Console.Clear();
        Console.WriteLine($"Game Over, {Game.Nickname}! All lives used.");
        Console.WriteLine($"Final Score: {Game.Score}");
        Console.WriteLine($"First Attempt Score: {Game.FirstAttemptScore}");
        Console.WriteLine($"Second Attempt Score: {Game.SecondAttemptScore}");
        Console.WriteLine($"Third Attempt Score: {Game.ThirdAttemptScore}");
        Console.WriteLine("Press Enter to exit...");
        Console.ReadLine();
    }
}

class Game
{
    static int playerPositionX;
    static int playerPositionY;
    static int enemyPositionX;
    static int enemyPositionY;
    static Random random;
    static int gameSpeed;

    public static int Lives { get; private set; }
    public static int Score { get; private set; }
    public static int FirstAttemptScore { get; private set; }
    public static int SecondAttemptScore { get; private set; }
    public static int ThirdAttemptScore { get; private set; }
    public static string Nickname { get; private set; }

    public static void Initialize(string nickname)
    {
        playerPositionX = Console.WindowWidth / 2;
        playerPositionY = Console.WindowHeight - 2;
        enemyPositionX = 0;
        enemyPositionY = 0;
        random = new Random();
        Score = 0;
        gameSpeed = 100;
        Lives = 3;
        Nickname = nickname;
    }

    public static void Play()
    {
        while (Lives > 0)
        {
            Update();
            Draw();
            Thread.Sleep(gameSpeed);

            if (Lives <= 0)
            {
                Console.Clear();
                Console.WriteLine($"Game Over, {Nickname}! All lives used.");
                Console.WriteLine($"Final Score: {Score}");
                Console.WriteLine($"First Attempt Score: {FirstAttemptScore}");
                Console.WriteLine($"Second Attempt Score: {SecondAttemptScore}");
                Console.WriteLine($"Third Attempt Score: {ThirdAttemptScore}");
                Console.WriteLine("Press Enter to exit...");
                Console.ReadLine();
            }
        }
    }

    public static void Update()
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo key = Console.ReadKey(false);
            if (key.Key == ConsoleKey.LeftArrow && playerPositionX > 0)
            {
                playerPositionX--;
            }
            else if (key.Key == ConsoleKey.RightArrow && playerPositionX < Console.WindowWidth - 1)
            {
                playerPositionX++;
            }
            else if (key.Key == ConsoleKey.UpArrow && playerPositionY > 0)
            {
                playerPositionY--;
            }
            else if (key.Key == ConsoleKey.DownArrow && playerPositionY < Console.WindowHeight - 1)
            {
                playerPositionY++;
            }
        }

        enemyPositionY++;

        if (enemyPositionY >= Console.WindowHeight)
        {
            Score += 10;
            enemyPositionX = random.Next(0, Console.WindowWidth);
            enemyPositionY = 0;

            if (Score % 60 == 0 && gameSpeed > 10)
            {
                gameSpeed -= 10;
                Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2);
                Console.Write("Speed Up!");
                Thread.Sleep(500);
                Console.SetCursorPosition(Console.WindowWidth / 2 - 5, Console.WindowHeight / 2);
                Console.Write("          ");
            }
        }

        if (playerPositionX == enemyPositionX && playerPositionY == enemyPositionY)
        {
            Console.Clear();
            Console.WriteLine($"Game Over, {Nickname}! Final Score: {Score}");
            Console.WriteLine("Press Enter to restart the game...");

            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

            // Save scores for each attempt
            if (Lives == 3)
            {
                FirstAttemptScore = Score;
            }
            else if (Lives == 2)
            {
                SecondAttemptScore = Score;
            }
            else if (Lives == 1)
            {
                ThirdAttemptScore = Score;
            }

            playerPositionX = Console.WindowWidth / 2;
            playerPositionY = Console.WindowHeight - 2;
            enemyPositionX = random.Next(0, Console.WindowWidth);
            enemyPositionY = 0;
            Score = 0;
            gameSpeed = 100;
            Lives--;
        }
    }

    public static void Draw()
    {
        Console.Clear();

        Console.SetCursorPosition(playerPositionX, playerPositionY);
        Console.Write("/\\");

        Console.SetCursorPosition(enemyPositionX, enemyPositionY);
        Console.Write("*");

        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write($"Score: {Score}  Speed: {1000 / gameSpeed}");

        Console.SetCursorPosition(Console.WindowWidth - 15, 0);
        Console.Write($"Lives: {Lives}");
    }
}
