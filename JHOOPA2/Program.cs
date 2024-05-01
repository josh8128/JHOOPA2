using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHOOPA2
{
    /// <summary>
    /// The class "Program" implements a menu-driven console application for playing two dice games: SevensOut and ThreeOrMore.
    /// The user can choose to play either game against another player or against the computer.
    /// The program also provides options to view game statistics and run testing.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The Main method is the entry point of the program.
        /// It displays a menu to the user and allows them to choose from different options.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("Menu:");
                    Console.WriteLine("1. Play Sevens Out");
                    Console.WriteLine("2. Play Three or More");
                    Console.WriteLine("3. View statistics");
                    Console.WriteLine("4. Run testing");
                    Console.WriteLine("5. Exit");

                    Console.Write("Enter your choice: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            PlaySevensOut();
                            break;
                        case "2":
                            PlayThreeOrMore();
                            break;
                        case "3":
                            ViewStatistics();
                            break;
                        case "4":
                            RunTesting();
                            break;
                        case "5":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// This method allows the user to play the SevensOut game.
        /// The user can choose to play against another player or against the computer.
        /// </summary>
        static void PlaySevensOut()
        {
            Console.WriteLine("\nDo you want to play against another player or the computer?");
            Console.WriteLine("1. Another player");
            Console.WriteLine("2. Computer");

            Console.Write("Enter your choice: ");

            string playChoice = Console.ReadLine();

            if (playChoice == "1")
            {
                try
                {
                    string filePath = "GameStatistics.json";
                    Statistics stats = new Statistics(filePath);
                    SevensOut sevensOut = new SevensOut(stats);
                    sevensOut.Play();
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error accessing file: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }
            else if (playChoice == "2")
            {
                try
                {
                    string filePath = "GameStatistics.json";
                    Statistics stats = new Statistics(filePath);
                    SevensOutComputer sevensOutComputer = new SevensOutComputer(stats);
                    sevensOutComputer.Play();
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error accessing file: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        /// <summary>
        /// This method allows the user to play the ThreeOrMore game.
        /// The user can choose to play against another player or against the computer.
        /// </summary>
        static void PlayThreeOrMore()
        {
            Console.WriteLine("\nDo you want to play against another player or the computer?");
            Console.WriteLine("1. Another player");
            Console.WriteLine("2. Computer");

            Console.Write("Enter your choice: ");

            string playChoice = Console.ReadLine();

            if (playChoice == "1")
            {
                try
                {
                    string filePath = "GameStatistics.json";
                    Statistics stats = new Statistics(filePath);
                    ThreeOrMore threeOrMore = new ThreeOrMore(stats);
                    threeOrMore.Play();
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error accessing file: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }
            else if (playChoice == "2")
            {
                try
                {
                    string filePath = "GameStatistics.json";
                    Statistics stats = new Statistics(filePath);
                    ThreeOrMoreComputer threeOrMoreComputer = new ThreeOrMoreComputer(stats);
                    threeOrMoreComputer.Play();
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error accessing file: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }

        /// <summary>
        /// This method allows the user to view game statistics.
        /// </summary>
        static void ViewStatistics()
        {
            try
            {
                string filePath = "GameStatistics.json";
                Statistics stats = new Statistics(filePath);
                stats.LoadFromFile("GameStatistics.json");

                Console.WriteLine("Game Statistics:");
                Console.WriteLine($"SevensOut: Plays: {stats.GetGameStats("SevensOut").NumberOfPlays}, High Score: {stats.GetGameStats("SevensOut").HighScore}");
                Console.WriteLine($"ThreeOrMore: Plays: {stats.GetGameStats("ThreeOrMore").NumberOfPlays}, High Score: {stats.GetGameStats("ThreeOrMore").HighScore}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error accessing file: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// This method runs testing for both SevensOut and ThreeOrMore games.
        /// </summary>
        static void RunTesting()
        {
            try
            {
                Testing testing = new Testing();
                testing.TestSevensOut();
                testing.TestThreeOrMore();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}