using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace JHOOPA2
{
    /// <summary>
    /// The Statistics class allows for managing the game statistics.
    /// </summary>
    public class Statistics
    {
        private Dictionary<string, GameStats> gameStats;
        private string filePath;

        /// <summary>
        /// Initialises a new instance of the Statistics class with the specified file path.
        /// </summary>
        /// <param name="filePath">The file path to load statistics from and save statistics to.</param>
        public Statistics(string filePath)
        {
            this.filePath = filePath;
            gameStats = new Dictionary<string, GameStats>();
            LoadFromFile(filePath);
        }

        /// <summary>
        /// Gets the statistics for the specified game type.
        /// </summary>
        /// <param name="gameType">The type of the game.</param>
        /// <returns>The statistics for the specified game type.</returns>
        public GameStats GetGameStats(string gameType)
        {
            if (gameStats.ContainsKey(gameType))
                return gameStats[gameType];
            else
                return new GameStats(); // Return empty stats if not found
        }

        /// <summary>
        /// Saves the game statistics to a file.
        /// </summary>
        /// <param name="filePath">The file path to save the statistics to.</param>
        public void SaveToFile(string filePath)
        {
            try
            {
                string jsonFile = JsonSerializer.Serialize(gameStats, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving statistics to file: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads the game statistics from a file.
        /// </summary>
        /// <param name="filePath">The file path to load the statistics from.</param>
        public void LoadFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    string jsonFile = File.ReadAllText(filePath);
                    gameStats = JsonSerializer.Deserialize<Dictionary<string, GameStats>>(jsonFile);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while loading statistics from file: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the statistics for the specified game type with the given total scores.
        /// </summary>
        /// <param name="gameType">The type of the game.</param>
        /// <param name="totalScores">The total scores achieved in the game.</param>
        public void UpdateStatistics(string gameType, int[] totalScores)
        {
            // Calculate the maximum total score
            int maxTotalScore = Math.Max(totalScores[0], totalScores[1]);

            // Update number of plays
            if (gameStats.ContainsKey(gameType))
                gameStats[gameType].NumberOfPlays++;
            else
                gameStats[gameType] = new GameStats { NumberOfPlays = 1 };

            // Update high score if the current total score is higher
            if (maxTotalScore > gameStats[gameType].HighScore)
            {
                gameStats[gameType].HighScore = maxTotalScore;
            }

            // Save statistics to file
            SaveToFile(filePath);
        }
    }

    /// <summary>
    /// This class represents the statistics of a game (number of plays and high score)
    /// </summary>
    public class GameStats
    {
        /// <summary>
        /// Getter and setter for the number of plays.
        /// </summary>
        public int NumberOfPlays { get; set; }
        /// <summary>
        /// Getter and setter for the highest score achieved.
        /// </summary>
        public int HighScore { get; set; }
    }
}
