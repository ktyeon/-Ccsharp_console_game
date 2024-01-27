using System;
using System.Threading;

class Program
{
    static void Main()
    {
        // Set console window properties
        Console.Title = "Simple Console Game";
        Console.WindowHeight = 14;
        Console.WindowWidth = 28;
        Console.BufferWidth = 28;

        // Ask user for nickname
        Console.WriteLine("Enter your nickname:");
        string nickname = Console.ReadLine();

        // Greet the player and prompt to start the game
        Console.WriteLine($"Hello, {nickname}! Press Enter to start the game...");

        // Wait for the Enter key to be pressed
        while (Console.ReadKey().Key != ConsoleKey.Enter) { }

        // Start the game
        GameManager.RunGame(nickname);
    }
}

class GameManager
{
    // Main game loop
    public static void RunGame(string nickname)
    {
        // Initialize game state
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
    // Player and enemy positions, random generator, game speed
    static int playerPositionX;
    static int playerPositionY;
    static int enemyPositionX;
    static int enemyPositionY;
    static Random random;
    static int gameSpeed;

    // Properties for game state
    public static int Lives { get; private set; }
    public static int Score { get; private set; }
    public static int FirstAttemptScore { get; private set; }
    public static int SecondAttemptScore { get; private set; }
    public static int ThirdAttemptScore { get; private set; }
    public static string Nickname { get; private set; }

    // Initialize game state
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

    // Play the game
    public static void Play()
    {
        while (Lives > 0)
        {
            // Update game state
            Update();
            // Draw game elements
            Draw();
            // Pause to control game speed
            Thread.Sleep(gameSpeed);

            // Check for game over
            if (Lives <= 0)
            {
                // Display game over message after all lives are used
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

    // Update game state
    public static void Update()
    {
        // Check for player input
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

        // Move the enemy down
        enemyPositionY++;

        // Check if the enemy reached the bottom
        if (enemyPositionY >= Console.WindowHeight)
        {
            // Score points and reset enemy position
            Score += 10;
            enemyPositionX = random.Next(0, Console.WindowWidth);
            enemyPositionY = 0;

            // Increase speed every 60 points and display a message
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

        // Check for collision with the player
        if (playerPositionX == enemyPositionX && playerPositionY == enemyPositionY)
        {
            // Display game over message and prompt to restart
            Console.Clear();
            Console.WriteLine($"Game Over, {Nickname}! Final Score: {Score}");
            Console.WriteLine("Press Enter to restart the game...");

            // Wait for the Enter key to be pressed
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

            // Restart the game by resetting variables
            playerPositionX = Console.WindowWidth / 2;
            playerPositionY = Console.WindowHeight - 2;
            enemyPositionX = random.Next(0, Console.WindowWidth);
            enemyPositionY = 0;
            Score = 0;
            gameSpeed = 100;
            Lives--;
        }
    }

    // Draw game elements
    public static void Draw()
    {
        // Clear the console
        Console.Clear();

        // Draw the player
        Console.SetCursorPosition(playerPositionX, playerPositionY);
        Console.Write("/\\");

        // Draw the enemy
        Console.SetCursorPosition(enemyPositionX, enemyPositionY);
        Console.Write("*");

        // Display the score and game speed
        Console.SetCursorPosition(0, Console.WindowHeight - 1);
        Console.Write($"Score: {Score}  Speed: {1000 / gameSpeed}");

        // Display remaining lives
        Console.SetCursorPosition(Console.WindowWidth - 15, 0);
        Console.Write($"Lives: {Lives}");
    }
}
