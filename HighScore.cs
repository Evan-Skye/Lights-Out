/*HighScore.cs
 * A class that's used to store information about the high scores.
 * 
 * Revision History:
 *      12.Dec.2020, Evan Skye: Scaffolded & Written.
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightsOut
{
    public class HighScore
    {
        private string name;
        private int score;

        public HighScore(string name, int score)
        {
            this.name = name;
            this.score = score;
        }

        public string Name { get => name; set => name = value; }
        public int Score { get => score; set => score = value; }
    }
}
