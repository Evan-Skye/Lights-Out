using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace LightsOut
{
    class Arrow : DrawableGameComponent
    {
        public string direction;
        private int placement;
        public int letterIndex = 0;

        const int HEIGHT = 90;
        const int WIDTH = 70;
        const int LOWEST_INDEX = 0;
        const int HIGHEST_INDEX = 51;

        SpriteBatch spriteBatch;
        private Texture2D tex;

        private Texture2D texUp;
        private Texture2D texUpSelected;
        private Texture2D texDown;
        private Texture2D texDownSelected;

        public Rectangle destinationRect;

        public int Placement { get => placement; set => placement = value; }

        public Arrow(Game game, SpriteBatch spriteBatch, string direction, int placement, Rectangle destinationRect) : base(game)
        {
            this.direction = direction;
            this.placement = placement;
            this.spriteBatch = spriteBatch;
            texUp = game.Content.Load<Texture2D>("Images/arrowUp");
            texDown = game.Content.Load<Texture2D>("Images/arrowDown");
            this.destinationRect = destinationRect;

            if (direction == "up") tex = texUp;
            else tex = texDown;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, destinationRect, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }


        public int Increment()
        {
            if(direction == "up")
            {
                if(letterIndex == LOWEST_INDEX)
                {
                    letterIndex = 51;
                }

                else
                {
                    letterIndex--;
                }
            }

            if(direction == "down")
            {
                if(letterIndex == HIGHEST_INDEX)
                {
                    letterIndex = LOWEST_INDEX;
                }

                else
                {
                    letterIndex++;
                }
            }

            return letterIndex;
        }

    }
}
