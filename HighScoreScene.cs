/*HighScoreScene.cs
 * A class that's designed to show the High Scores saved on file via the Scene.
 * 
 * Revision History:
 *      04.Dec.2020, Evan Skye: Scaffolded.
 *      09.Dec.2020, Evan Skye: Background updated.
 *      12.Dec.2020, Evan Skye: Strings writen and formatted to screen.
 */


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace LightsOut
{
    class HighScoreScene : GameScene
    {
        SpriteFont regularFont;
        SpriteFont highlightFont;
        public List<HighScore> scores;

        const int DISPLAY_NAME_X = 305;
        const int DISPLAY_SCORE_X = 420;
        const int SCORE_X = 490;

        const int DISPLAY_Y = 120;
        const int INIT_Y = 160;
        const int PADDING = 20;

        Vector2 titleNamePosition = new Vector2(DISPLAY_NAME_X, DISPLAY_Y);
        Vector2 titleScorePosition = new Vector2(DISPLAY_SCORE_X, DISPLAY_Y);

        Vector2 namePosition;
        Vector2 scorePosition;

        public HighScoreScene(Game game, SpriteBatch spriteBatch, SpriteFont regularFont, 
            SpriteFont highlightFont, List<HighScore> scores) : base(game)
        {
            this.spriteBatch = spriteBatch;
            tex = Game.Content.Load<Texture2D>("Backgrounds/highscore");
            this.regularFont = regularFont;
            this.highlightFont = highlightFont;
            
            //sorts list the way we learned how to in entity framework
            this.scores = scores.OrderByDescending(a => a.Score).ToList();
        }

        public override void Draw(GameTime gameTime)
        {


            spriteBatch.Begin();
            spriteBatch.Draw(tex, Vector2.Zero, Color.White);

            //draws the headings
            spriteBatch.DrawString(highlightFont, "Name", titleNamePosition, Color.Yellow);
            spriteBatch.DrawString(highlightFont, "Score", titleScorePosition, Color.Yellow);

            //draws each high score in the list, properly padded.
            //the name & score positions are edited such that they line up on the left/right sides.
            for (int i = 0; i < 10; i++)
            {
                Vector2 dimension = regularFont.MeasureString(scores[i].Score.ToString());
                namePosition = new Vector2(DISPLAY_NAME_X, INIT_Y + PADDING * i);
                scorePosition = new Vector2(SCORE_X - dimension.X, INIT_Y + PADDING * i);
                spriteBatch.DrawString(regularFont, scores[i].Name, namePosition, Color.White);
                spriteBatch.DrawString(regularFont, scores[i].Score.ToString(), scorePosition, Color.White); 
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
