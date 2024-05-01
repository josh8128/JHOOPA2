using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHOOPA2
{
    /// <summary>
    /// Represents a base class for the two dice games.
    /// </summary>
    public abstract class Game
    {
        // Array of dice objects
        protected Die[] dice;
        // The number of plays for each game
        protected int numberofPlays;
        // Array used to store total scores
        protected int[] totalScores;
        // Index used for the current player
        protected int currentPlayerIndex;
        // The statistics object used to record the game's results
        protected Statistics statistics;

        /// <summary>
        /// Initialises a new instance of the "Game" class.
        /// </summary>
        /// <param name="stats">The statistics object to record game results.</param>
        public Game(Statistics stats)
        {
            statistics = stats;
        }

        /// <summary>
        /// Gets the total score of the current player.
        /// </summary>
        /// <returns>The total score of the current player.</returns>
        public int GetTotalScore()
        {
            return totalScores[currentPlayerIndex];
        }

        /// <summary>
        /// Abstract method representing the gameplay of the game.
        /// </summary>
        public abstract void Play();
    }

    /// <summary>
    /// Represents the "Sevens Out" dice game.
    /// </summary>
    public class SevensOut : Game
    {
        /// <summary>
        /// Initialises a new instance of the "SevensOut" class.
        /// </summary>
        /// <param name="stats">The statistics object to record game results.</param>
        public SevensOut(Statistics stats) : base(stats) { }

        /// <summary>
        /// Implements the gameplay logic for the "SevensOut" game.
        /// </summary>
        public override void Play()
        {
            totalScores = new int[2];
            dice = new Die[2];

            for (int i = 0; i < dice.Length; i++)
            {
                dice[i] = new Die();
            }

            while (true)
            {
                Console.WriteLine($"Player {currentPlayerIndex + 1}, press Enter to roll the dice...");
                Console.ReadLine();

                int roll1 = dice[0].Roll();
                int roll2 = dice[1].Roll();
                int sum = roll1 + roll2;

                Console.WriteLine($"You rolled {roll1} and {roll2}. Total: {sum}");

                // Ends game if player rolls a 7 and updates the game statistics
                if (sum == 7)
                {
                    Console.WriteLine($"Player {currentPlayerIndex + 1} hit 7! Game over.");
                    statistics.UpdateStatistics("SevensOut", totalScores);
                    break;
                }
                // Continues the game and adds the current roll total to the overall total
                // Ends current player's turn and switches to the other player
                else
                {
                    totalScores[currentPlayerIndex] += sum;
                    if (roll1 == roll2)
                        totalScores[currentPlayerIndex] += sum;

                    Console.WriteLine($"Player {currentPlayerIndex + 1} current score: {totalScores[currentPlayerIndex]}");
                    currentPlayerIndex = (currentPlayerIndex + 1) % 2; // Switch player
                }
            }
        }
    }

    /// <summary>
    /// Represents the "Three or More" dice game.
    /// </summary>
    public class ThreeOrMore : Game
    {
        /// <summary>
        /// Initialises a new instance of the "ThreeOrMore" class.
        /// </summary>
        /// <param name="stats">The statistics object to record game results.</param>
        public ThreeOrMore(Statistics stats) : base(stats) { }

        /// <summary>
        /// Implements the gameplay logic for the "ThreeOrMore" game.
        /// </summary>
        public override void Play()
        {
            totalScores = new int[2];
            dice = new Die[5];

            for (int i = 0; i < dice.Length; i++)
            {
                dice[i] = new Die();
            }

            currentPlayerIndex = 0;

            while (totalScores[currentPlayerIndex] < 20)
            {
                Console.WriteLine($"Player {currentPlayerIndex + 1}, press Enter to roll the dice...");
                Console.ReadLine();

                int[] rolls = new int[5];
                for (int i = 0; i < 5; i++)
                {
                    rolls[i] = dice[i].Roll();
                    Console.WriteLine($"Die {i + 1}: {rolls[i]}");
                }

                Array.Sort(rolls);

                // Counts the how often each dice roll appears (the frequency)
                Dictionary<int, int> frequency = new Dictionary<int, int>();
                foreach (int roll in rolls)
                {
                    if (!frequency.ContainsKey(roll))
                        frequency[roll] = 0;
                    frequency[roll]++;
                }

                int score = 0;
                bool hasTwoOfAKind = false;
                bool hasThreeOfAKind = false;
                bool hasFourOfAKind = false;
                bool hasFiveOfAKind = false;

                // Working out what dice combination the player has
                foreach (var pair in frequency)
                {
                    if (pair.Value >= 2)
                    {
                        hasTwoOfAKind = true;
                        if (pair.Value >= 3)
                        {
                            hasThreeOfAKind = true;
                            if (pair.Value >= 4)
                            {
                                hasFourOfAKind = true;
                                if (pair.Value == 5)
                                    hasFiveOfAKind = true;
                            }
                        }
                    }
                }

                // Calculating and updating the player's score depending on what combination they rolled
                if (hasFiveOfAKind)
                {
                    score = 12; // 5 of a kind scores 12 points
                    totalScores[currentPlayerIndex] += score;
                    Console.WriteLine($"Player {currentPlayerIndex + 1} got three of a kind! Score: {score}. Total: {totalScores[currentPlayerIndex]}");
                }
                else if (hasFourOfAKind)
                {
                    score = 6; // 4 of a kind scores 6 points
                    totalScores[currentPlayerIndex] += score;
                    Console.WriteLine($"Player {currentPlayerIndex + 1} got three of a kind! Score: {score}. Total: {totalScores[currentPlayerIndex]}");
                }
                else if (hasThreeOfAKind)
                {
                    score = 3; // 3 of a kind scores 3 points
                    totalScores[currentPlayerIndex] += score;
                    Console.WriteLine($"Player {currentPlayerIndex + 1} got three of a kind! Score: {score}. Total: {totalScores[currentPlayerIndex]}");
                }
                else if (hasTwoOfAKind)
                {
                    Console.WriteLine($"Player {currentPlayerIndex + 1} got two of a kind. Rerolling...\n");

                    // Check to see if the player wants to reroll all or only non-matching dice
                    Console.Write("Do you want to reroll all dice (Y/N)? ");
                    bool rerollAll = Console.ReadLine().Trim().ToUpper() == "Y";
                    rolls = RerollDice(rolls, rerollAll, frequency);
                }
                else
                {
                    Console.WriteLine($"Player {currentPlayerIndex + 1} didn't get three, four, or five of a kind. Rerolling...");
                }

                currentPlayerIndex = (currentPlayerIndex + 1) % 2; // Switch player
            }
            // Displays the winner of the game and updates the game's statistics
            Console.WriteLine($"Player {currentPlayerIndex + 1} reached 20 points. Game over.");
            statistics.UpdateStatistics("ThreeOrMore", totalScores);
        }
        /// <summary>
        /// Rerolls the dice based on player choice.
        /// </summary>
        /// <param name="rolls">The array of dice rolls.</param>
        /// <param name="rerollAll">Whether to reroll all dice or only the non-matching ones.</param>
        /// <param name="frequency">Frequency of dice values.</param>
        /// <returns>The updated array of dice rolls.</returns>
        private int[] RerollDice(int[] rolls, bool rerollAll, Dictionary<int, int> frequency)
        {
            if (rerollAll)
            {
                // Reroll all dice
                for (int i = 0; i < rolls.Length; i++)
                {
                    rolls[i] = dice[i].Roll();
                    Console.WriteLine($"Die {i + 1}: {rolls[i]}");
                }
            }
            else
            {
                int twoOfAKindValue = 0;
                foreach (var pair in frequency)
                {
                    if (pair.Value == 2)
                    {
                        twoOfAKindValue = pair.Key;
                        break;
                    }
                }

                // Reroll only the non-matching dice
                for (int i = 0; i < rolls.Length; i++)
                {
                    if (rolls[i] != twoOfAKindValue)
                        rolls[i] = dice[i].Roll();
                    Console.WriteLine($"Die {i + 1}: {rolls[i]}");
                }
            }
            return rolls;
        }
    }

    /// <summary>
    /// Represents the player and computer controlled version of the "SevensOut" dice game.
    /// </summary>
    public class SevensOutComputer : Game
    {
        /// <summary>
        /// Initialises a new instance of the "SevensOutComputer" class.
        /// </summary>
        /// <param name="stats">The statistics object to record game results.</param>
        public SevensOutComputer(Statistics stats) : base(stats) { }

        /// <summary>
        /// Implements the gameplay logic for the computer controlled version of the "SevensOut" game.
        /// </summary>
        public override void Play()
        {
            totalScores = new int[2];
            dice = new Die[2];

            for (int i = 0; i < dice.Length; i++)
            {
                dice[i] = new Die();
            }

            while (true)
            {
                // Player's turn
                Console.WriteLine("Player's turn. Press Enter to roll the dice...");
                Console.ReadLine();

                int roll1 = dice[0].Roll();
                int roll2 = dice[1].Roll();
                int sum = roll1 + roll2;

                Console.WriteLine($"You rolled {roll1} and {roll2}. Total: {sum}");

                // Ends game if player rolls a 7 and updates the game statistics
                if (sum == 7)
                {
                    Console.WriteLine("Player hit 7! Game over.");
                    statistics.UpdateStatistics("SevensOut", totalScores);
                    break;
                }
                // Continues the game and adds the current roll total to the overall total
                else
                {
                    totalScores[0] += sum;
                    if (roll1 == roll2)
                        totalScores[0] += sum;

                    Console.WriteLine("Player's current score: " + totalScores[0]);

                    // Computer's turn
                    Console.WriteLine("\nComputer's turn...");
                    int computerRoll1 = dice[0].Roll();
                    int computerRoll2 = dice[1].Roll();
                    int computerSum = computerRoll1 + computerRoll2;

                    Console.WriteLine($"Computer rolled {computerRoll1} and {computerRoll2}. Total: {computerSum}");

                    // Ends the game if computer rolls a 7 and updates the game statistics
                    if (computerSum == 7)
                    {
                        Console.WriteLine("Computer hit 7! Game over.");
                        statistics.UpdateStatistics("SevensOut", totalScores);
                        break;
                    }
                    // Continues the game and adds the current roll total to the overall total
                    else
                    {
                        totalScores[1] += computerSum;
                        if (computerRoll1 == computerRoll2)
                            totalScores[1] += computerSum;

                        Console.WriteLine("Computer's current score: " + totalScores[1]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Represents the player and computer controlled version of the "ThreeOrMore" dice game.
    /// </summary>
    public class ThreeOrMoreComputer : Game
    {
        /// <summary>
        /// Initialises a new instance of the "ThreeOrMoreComputer" class.
        /// </summary>
        /// <param name="stats">The statistics object to record game results.</param>
        public ThreeOrMoreComputer(Statistics stats) : base(stats) { }

        /// <summary>
        /// Implements the gameplay logic for the player and computer controlled version of the "ThreeOrMore" game.
        /// </summary>
        public override void Play()
        {
            totalScores = new int[2];
            dice = new Die[5];

            for (int i = 0; i < dice.Length; i++)
            {
                dice[i] = new Die();
            }

            currentPlayerIndex = 0;

            while (totalScores[currentPlayerIndex] < 20 && totalScores[1] < 20)
            {
                // Player's turn
                Console.WriteLine($"Player {currentPlayerIndex + 1}, press Enter to roll the dice...");
                Console.ReadLine();

                int[] rolls = new int[5];
                for (int i = 0; i < 5; i++)
                {
                    rolls[i] = dice[i].Roll();
                    Console.WriteLine($"Die {i + 1}: {rolls[i]}");
                }

                Array.Sort(rolls);

                // Counts the how often each dice roll appears (the frequency)
                Dictionary<int, int> frequency = new Dictionary<int, int>();
                foreach (int roll in rolls)
                {
                    if (!frequency.ContainsKey(roll))
                        frequency[roll] = 0;
                    frequency[roll]++;
                }

                int score = 0;
                bool hasTwoOfAKind = false;
                bool hasThreeOfAKind = false;
                bool hasFourOfAKind = false;
                bool hasFiveOfAKind = false;

                // Working out what dice combination the player has
                foreach (var pair in frequency)
                {
                    if (pair.Value >= 2)
                    {
                        hasTwoOfAKind = true;
                        if (pair.Value >= 3)
                        {
                            hasThreeOfAKind = true;
                            if (pair.Value >= 4)
                            {
                                hasFourOfAKind = true;
                                if (pair.Value == 5)
                                    hasFiveOfAKind = true;
                            }
                        }
                    }
                }

                // Calculating and updating the player's score depending on what combination they rolled
                if (hasFiveOfAKind)
                {
                    score = 12; // 5 of a kind scores 12 points
                    totalScores[currentPlayerIndex] += score;
                    Console.WriteLine($"Player {currentPlayerIndex + 1} got three of a kind! Score: {score}. Total: {totalScores[currentPlayerIndex]}");
                }
                else if (hasFourOfAKind)
                {
                    score = 6; // 4 of a kind scores 6 points
                    totalScores[currentPlayerIndex] += score;
                    Console.WriteLine($"Player {currentPlayerIndex + 1} got three of a kind! Score: {score}. Total: {totalScores[currentPlayerIndex]}");
                }
                else if (hasThreeOfAKind)
                {
                    score = 3; // 3 of a kind scores 3 points
                    totalScores[currentPlayerIndex] += score;
                    Console.WriteLine($"Player {currentPlayerIndex + 1} got three of a kind! Score: {score}. Total: {totalScores[currentPlayerIndex]}");
                }
                else if (hasTwoOfAKind)
                {
                    Console.WriteLine($"Player {currentPlayerIndex + 1} got two of a kind. Rerolling...\n");

                    // Check to see if the player wants to reroll all or only non-matching dice
                    Console.Write("Do you want to reroll all dice (Y/N)? ");
                    bool rerollAll = Console.ReadLine().Trim().ToUpper() == "Y";
                    rolls = RerollDice(rolls, rerollAll, frequency);
                }
                else
                {
                    Console.WriteLine($"Player {currentPlayerIndex + 1} didn't get three, four, or five of a kind. Rerolling...\n");
                }

                // Check if the player has won and update statistics if they have
                if (totalScores[currentPlayerIndex] >= 20)
                {
                    Console.WriteLine($"Player {currentPlayerIndex + 1} reached 20 points. Player {currentPlayerIndex + 1} wins!");
                    statistics.UpdateStatistics("ThreeOrMore", totalScores);
                    break;
                }

                // Computer's turn
                int[] computerRolls = new int[5];
                Console.WriteLine(" ");
                for (int i = 0; i < 5; i++)
                {
                    computerRolls[i] = dice[i].Roll();
                    Console.WriteLine($"Computer's Die {i + 1}: {computerRolls[i]}");
                }

                Array.Sort(computerRolls);

                // Counts the how often each dice roll appears (the frequency)
                Dictionary<int, int> computerFreq = new Dictionary<int, int>();
                foreach (int roll in computerRolls)
                {
                    if (!computerFreq.ContainsKey(roll))
                        computerFreq[roll] = 0;
                    computerFreq[roll]++;
                }

                int computerScore = 0;
                bool computerHasTwoOfAKind = false;
                bool computerHasThreeOfAKind = false;
                bool computerHasFourOfAKind = false;
                bool computerHasFiveOfAKind = false;

                // Working out what dice combination the computer has
                foreach (var pair in computerFreq)
                {
                    if (pair.Value >= 2)
                    {
                        computerHasTwoOfAKind = true;
                        if (pair.Value >= 3)
                        {
                            computerHasThreeOfAKind = true;
                            if (pair.Value >= 4)
                            {
                                computerHasFourOfAKind = true;
                                if (pair.Value == 5)
                                    computerHasFiveOfAKind = true;
                            }
                        }
                    }


                }

                // Calculating and updating the computer's score depending on what combination they rolled
                if (computerHasFiveOfAKind)
                {
                    computerScore = 12; // 5 of a kind scores 12 points
                    totalScores[1] += computerScore;
                    Console.WriteLine($"Computer got five of a kind! Score: {computerScore}. Total: {totalScores[1]}");
                }
                else if (computerHasFourOfAKind)
                {
                    computerScore = 6; // 4 of a kind scores 6 points
                    totalScores[1] += computerScore;
                    Console.WriteLine($"Computer got four of a kind! Score: {computerScore}. Total: {totalScores[1]}");
                }
                else if (computerHasThreeOfAKind)
                {
                    computerScore = 3; // 3 of a kind scores 3 points
                    totalScores[1] += computerScore;
                    Console.WriteLine($"Computer got three of a kind! Score: {computerScore}. Total: {totalScores[1]}");
                }
                else if (computerHasTwoOfAKind)
                {
                    Console.WriteLine($"Computer got two of a kind. Rerolling...\n");
                    // Rerolls the computer's dice
                    bool rerollAll = true;
                    computerRolls = RerollDice(computerRolls, rerollAll, frequency);
                }
                else
                {
                    Console.WriteLine("Computer didn't get three, four, or five of a kind. Rerolling...");
                }

                // Check if the computer has won and update statistics if they have
                if (totalScores[1] >= 20)
                {
                    Console.WriteLine($"Computer reached 20 points. Computer wins!");
                    statistics.UpdateStatistics("ThreeOrMore", totalScores);
                    break;
                }
            }
        }
        /// <summary>
        /// Rerolls the player and computer's dice based on the current rolls.
        /// </summary>
        /// <param name="rolls">The array of dice rolls.</param>
        /// <param name="rerollAll">Whether to reroll all dice or only the non-matching ones.</param>
        /// <param name="frequency">Frequency of dice values.</param>
        /// <returns>The updated array of dice rolls.</returns>
        private int[] RerollDice(int[] rolls, bool rerollAll, Dictionary<int, int> frequency)
        {
            if (rerollAll)
            {
                for (int i = 0; i < rolls.Length; i++)
                {
                    rolls[i] = dice[i].Roll();
                    Console.WriteLine($"Die {i + 1}: {rolls[i]}");
                }
            }
            else
            {
                int twoOfAKindValue = 0;
                foreach (var pair in frequency)
                {
                    if (pair.Value == 2)
                    {
                        twoOfAKindValue = pair.Key;
                        break;
                    }
                }

                // Reroll only the non-matching dice
                for (int i = 0; i < rolls.Length; i++)
                {
                    if (rolls[i] != twoOfAKindValue)
                        rolls[i] = dice[i].Roll();
                    Console.WriteLine($"Die {i + 1}: {rolls[i]}");
                }
            }
            return rolls;
        }
    }
}