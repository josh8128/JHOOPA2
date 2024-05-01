using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHOOPA2
{
    /// <summary>
    /// The Testing class implements methods to test the SevensOut and ThreeOrMore games.
    /// </summary>
    public class Testing
    {
        /// <summary>
        /// This method tests the SevensOut game.
        /// It creates a SevensOut game object, plays the game, and verifies the total score.
        /// </summary>
        public void TestSevensOut()
        {
            Console.WriteLine("Testing SevensOut Game...");

            try
            {
                // Create SevensOut game object
                string filePath = "GameStatistics.json";
                Statistics stats = new Statistics(filePath);
                SevensOut sevensOutTest = new SevensOut(stats);
                sevensOutTest.Play();

                // Verify total score is 7
                Debug.Assert(sevensOutTest.GetTotalScore() == 7, "Total score should be 7 in SevensOut Game.");

                Console.WriteLine("SevensOut Game test passed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while testing SevensOut Game: {ex.Message}");
            }
        }

        /// <summary>
        /// This method tests the ThreeOrMore game.
        /// It creates a ThreeOrMore game object, plays the game, and verifies the total score.
        /// </summary>
        public void TestThreeOrMore()
        {
            Console.WriteLine("Testing ThreeOrMore Game...");

            try
            {
                // Create ThreeOrMore game object
                string filePath = "GameStatistics.json";
                Statistics stats = new Statistics(filePath);
                ThreeOrMore threeOrMoreTest = new ThreeOrMore(stats);
                threeOrMoreTest.Play();

                // Verify total score is set and added correctly
                int totalScore = threeOrMoreTest.GetTotalScore();
                Debug.Assert(totalScore >= 20, $"Total score should be greater than or equal to 20 in ThreeOrMore Game. Current score: {totalScore}");

                Console.WriteLine("ThreeOrMore Game test passed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while testing ThreeOrMore Game: {ex.Message}");
            }
        }
    }
}
