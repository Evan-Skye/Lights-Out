/*StartScene.cs
 * The initial scene. Creates and handles the menu component.
 * 
 * Revision History:
 *      04.Dec.2020, Evan Skye: Scaffolded & Written.
 *      09.Dec.2020, Evan Skye: Background image updated.
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
    class StartScene : GameScene
    {
        private MenuComponent menu;

        public MenuComponent Menu { get => menu; set => menu = value; }

        private Vector2 position;
        private string[] menuItems = { "Start Game", "Help", "High Score", "Credits", "Quit" };
        SpriteFont regularFont;
        SpriteFont highlightFont;

        public StartScene(Game game, SpriteBatch spriteBatch, SpriteFont regularFont, SpriteFont highlightFont) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.regularFont = regularFont;
            this.highlightFont = highlightFont;
            tex = game.Content.Load<Texture2D>("Backgrounds/title");
            position = new Vector2(Shared.boundary.X / 2 - tex.Width / 2, tex.Height / 2);

            menu = new MenuComponent(game, spriteBatch, regularFont, highlightFont, menuItems);
            this.Components.Add(menu);
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
