/*CreditsScene.cs
 * The class that shows the people Credited for contributions to the game (Mainly me).
 * 
 * Revision History:
 *      04.Dec.2020, Evan Skye: Scaffolded.
 *      09.Dec.2020, Evan Skye: Image updated.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LightsOut
{
    class CreditsScene : GameScene
    {
        public CreditsScene(Game game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
            tex = Game.Content.Load<Texture2D>("Backgrounds/credits");
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
