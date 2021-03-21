/*Shared.cs
 * A class that I use to store the window boundary and some helper methods.
 * 
 * Revision History:
 *      04.Dec.2020, Evan Skye: Scaffolded & Written.
 *      12.Dec.2020, Evan Skye: Methods written.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace LightsOut
{
    public class Shared
    {
        //boundary of the window.
        public static Vector2 boundary;

        /// <summary>
        /// A method to ensure ever keypress is unique & the key isn't being held down.
        /// </summary>
        /// <param name="current">Current KeyboardState</param>
        /// <param name="old">Last Update's KeyboardState</param>
        /// <param name="keyToCheck">Key we're checking validity for</param>
        /// <returns></returns>
        public static bool IsValidKeyPress(KeyboardState current, KeyboardState old, Keys keyToCheck)
        {
            if (current.IsKeyDown(keyToCheck) && old.IsKeyUp(keyToCheck)) return true;
            else return false;
        }

        /// <summary>
        /// A method to ensure every click being registered is unique & the button isn't being held down.
        /// </summary>
        /// <param name="current">Current ButtonState of the Mouse</param>
        /// <param name="old">Last Update's ButtonState of the Mouse</param>
        /// <returns></returns>
        public static bool IsValidClick(ButtonState current, ButtonState old)
        {
            if (current == ButtonState.Pressed && old == ButtonState.Released) return true;
            else return false;
        }

        /// <summary>
        /// Reads the scores in from a file. If none exists, it creates some.
        /// </summary>
        /// <returns>List of High Scores.</returns>
        public static List<HighScore> PopulateScoreList()
        {
            string json = "";
            string directory = Directory.GetCurrentDirectory();
            string filePath = directory + @"\scores.json";

            List<HighScore> scores = new List<HighScore>();

            if (File.Exists(filePath))
            {
                //reads the file in line by line as long as there are still lines to read.
                string line;

                using (StreamReader reader = new StreamReader(filePath))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        json += line;

                    }                
                }

                //parses json to a list
                scores = JsonConvert.DeserializeObject<List<HighScore>>(json);

            }

            else
            {
                //default scores I wrote to inspire people to beat them, named after my friends.
                //the lowest obtainable score you can get in game is 200, so you will always beat me.

                HighScore topScore = new HighScore("Lexi", 1000);
                HighScore secondScore = new HighScore("Luke", 900);
                HighScore thirdScore = new HighScore("Isaac", 800);
                HighScore fourthScore = new HighScore("Ema", 700);
                HighScore fifthScore = new HighScore("Lucas", 600);
                HighScore sixthScore = new HighScore("Anders", 500);
                HighScore seventhScore = new HighScore("Sarah", 400);
                HighScore eighthScore = new HighScore("Abel", 300);
                HighScore ninthScore = new HighScore("Rose", 200);
                HighScore tenthScore = new HighScore("Evan", 100);


                //adds the scores to the list
                scores.Add(topScore);
                scores.Add(secondScore);
                scores.Add(thirdScore);
                scores.Add(fourthScore);
                scores.Add(fifthScore);
                scores.Add(sixthScore);
                scores.Add(seventhScore);
                scores.Add(eighthScore);
                scores.Add(ninthScore);
                scores.Add(tenthScore);

                //creates and saves to a file, so that you can open the game, close it and still have it work.
                SaveToFile(scores);
            }

            return scores;

        }

        /// <summary>
        /// Converts the info to JSON and saves it to a JSON file within the directory of the game.
        /// </summary>
        /// <param name="scores">List of High Scores on record</param>
        public static void SaveToFile(List<HighScore> scores)
        {
            List<HighScore> sortedScores = scores.OrderByDescending(a => a.Score).ToList();
            string json = JsonConvert.SerializeObject(sortedScores, Formatting.Indented);
            string directory = Directory.GetCurrentDirectory();
            string filePath = directory + @"\scores.json";

            using (StreamWriter sw = File.CreateText(filePath))
            {
                sw.Write(json);
            }

        }

    }
}
