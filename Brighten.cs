/*Brighten.cs
 * The class that handles the beginning of scene animations for the lightboxes.
 * 
 * Revision History:
 *      13.Dec.2020, Evan Skye: Scaffolded & Written.
 */

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LightsOut
{
    class Brighten : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Rectangle destinationRect;
        private List<Rectangle> frames;
        private int frameIndex = -1;
        private int delay = 2;
        private int delayCounter;

        private const int LENGTH = 150;
        private const int WIDTH = 150;

        private const int ROW = 4;
        private const int COL = 4;
        private const int ROW_OFFSET = 2;
        private const int INVALID_INDEX = -1;
        private const int INIT_INDEX = 0;
        private const int NO_DELAY = 0;

        public Rectangle DestinationRect { get => destinationRect; set => destinationRect = value; }

        /// <summary>
        /// The constructor for the object. Initializes the values, hides the component and
        /// parses the spritesheet into frames for use later.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="tex">The spritesheet</param>
        /// <param name="destinationRect">Where the animation will be drawn</param>
        public Brighten(Game game, SpriteBatch spriteBatch, Texture2D tex,
            Rectangle destinationRect) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.destinationRect = destinationRect;

            Hide();

            CreateFrames();
        }

        /// <summary>
        /// Hides the Animated LightBox
        /// </summary>
        public void Hide()
        {
            this.Visible = false;
            this.Enabled = false;
        }

        /// <summary>
        /// Resets the frame counter and enables the object.
        /// </summary>
        public void Start()
        {
            frameIndex = INVALID_INDEX;
            this.Visible = true;
            this.Enabled = true;
        }


        /// <summary>
        /// Parses the spritesheet into frames.
        /// </summary>
        private void CreateFrames()
        {
            frames = new List<Rectangle>();
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    int x = j * WIDTH;
                    int y = i * LENGTH;
                    Rectangle r = new Rectangle(x, y, WIDTH, LENGTH);
                    frames.Add(r);
                }
            }

        }

        /// <summary>
        /// Draws each frame after being given the logic by update.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            if (frameIndex >= INIT_INDEX)
            {
                spriteBatch.Draw(tex, DestinationRect, frames[frameIndex], Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Handles delay for drawing the frames and queues up the next frame to be drawn.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            delayCounter++;
            if (delayCounter > delay)
            {
                frameIndex++;
                //my spritesheet is 2 away from a complete square, so there are only 14 valid frames.
                //as such, the row offset is 3, since the count is always one higher than the final
                //valid index, and I'm missing the last 2 from the row as well.
                if (frameIndex > ((ROW * COL) - ROW_OFFSET))
                {
                    frameIndex = INVALID_INDEX;
                    Hide();
                }
                delayCounter = NO_DELAY;
            }

            base.Update(gameTime);
        }
    }
}
