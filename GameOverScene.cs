/*GameOverScene.cs
 * The class that shows the Game Over scene and its options.
 * 
 * Revision History:
 *      12.Dec.2020, Evan Skye: Scaffolded.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace LightsOut
{
    public class GameOverScene : GameScene
    {
        Game game;
        private int score;
        private bool needToShow = false;
        private Texture2D texName;
        SpriteFont regularFont;
        SpriteFont highlightFont;
        List<HighScore> scores;
        private Vector2 dimension;
        private Vector2 centredString;

        private bool isHighScore = false;


        const int LOWEST_SCORE = 9;
        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 480;


        public GameOverScene(Game game, SpriteBatch spriteBatch, int score,
            SpriteFont regularFont, SpriteFont highlightFont, List<HighScore> scores) : base(game)
        {
            this.spriteBatch = spriteBatch;

            tex = game.Content.Load<Texture2D>("Backgrounds/gameover");
            texName = game.Content.Load<Texture2D>("Backgrounds/nameentry");
            this.game = game;
            this.score = score;
            this.regularFont = regularFont;
            this.highlightFont = highlightFont;
            this.scores = scores;
        }

        public int Score { get => score; set => score = value; }
        public bool NeedToShow { get => needToShow; set => needToShow = value; }


        public override void Show()
        {
            needToShow = false;

            //if (score > scores[LOWEST_SCORE].Score)
            //{
            //    scores.RemoveAt(LOWEST_SCORE);
            //    //message = "HIGH SCORE!";
            //    Hide();
            //}

            base.Show();
        }

        public override void Draw(GameTime gameTime)
        {


            spriteBatch.Begin();
            spriteBatch.Draw(tex, Vector2.Zero, Color.White);
            //spriteBatch.DrawString(highlightFont, message, centredString, Color.Yellow);
            spriteBatch.End();

            //dimension = highlightFont.MeasureString(message);
            //centredString = new Vector2(WINDOW_WIDTH / 2 - dimension.X / 2, WINDOW_HEIGHT / 2 - dimension.Y / 2);


            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
