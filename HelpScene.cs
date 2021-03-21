/*HelpScene.cs
 * The class that shows the controls and premise of the game.
 * 
 * Revision History:
 *      04.Dec.2020, Evan Skye: Scaffolded.
 *      09.Dec.2020, Evan Skye: Image updated.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace LightsOut
{
    class HelpScene : GameScene
    {
        public HelpScene(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
            tex = Game.Content.Load<Texture2D>("Backgrounds/help");
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, Vector2.Zero, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
