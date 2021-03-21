using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace LightsOut
{
    class LetterSelector :  DrawableGameComponent
    {
        Game game;
        SpriteBatch spriteBatch;
        public int letterIndex;
        public int placement;
        private Vector2 destinationVector;
        SpriteFont font;
        string[] letters;

        public LetterSelector(Game game, SpriteBatch spriteBatch, 
            int placement, Vector2 destinationVector, SpriteFont font) : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.placement = placement;
            this.destinationVector = destinationVector;
            letterIndex = 0;
            this.font = font;
            letters = new string[52]{ "A", "B", "C", "D", "E", "F", "G", "H" ,"I", "J", "K", "L", "M",
                "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
                "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};

        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, letters[letterIndex], destinationVector, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
