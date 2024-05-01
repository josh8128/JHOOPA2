using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHOOPA2
{
    /// <summary>
    /// The Die class rolls a six sided dice for use in the game class
    /// </summary>
    public class Die
    {
        private Random random;
        /// <summary>
        /// Gets the current die value of the die.
        /// </summary>
        public int dieValue { get; private set; }

        /// <summary>
        /// Initialises a new instance of the Die class and rolls it.
        /// </summary>
        public Die()
        {
            random = new Random();
            Roll();
        }

        /// <summary>
        /// Rolls the die and returns the die value.
        /// </summary>
        /// <returns>The value of the die after rolling.</returns>
        public int Roll()
        {
            dieValue = random.Next(1, 7);
            return dieValue;
        }
    }
}