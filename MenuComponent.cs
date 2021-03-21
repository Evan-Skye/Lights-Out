/*MenuComponent.cs
 * A class that I use to handle the menu, primarily for scene selection.
 * 
 * Revision History:
 *      04.Dec.2020, Evan Skye: Scaffolded & Written.
 */


//dec 4 written

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace LightsOut
{
    public class MenuComponent : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private SpriteFont regularFont, highlightFont;
        private List<string> menuItems;
        private int selectedIndex;

        const int MENU_OFFSET = 1;
        const int FIRST_INDEX = 0;
        const int TOO_LOW_INDEX = -1;

        public int SelectedIndex { get => selectedIndex; set => selectedIndex = value; }

        private Vector2 position;
        private Color regularColor = Color.White;
        private Color highlightColor = Color.Yellow;
        private SoundEffect tick;

        private KeyboardState oldKbState;

        public MenuComponent(Game game, SpriteBatch spriteBatch, SpriteFont regularFont,
            SpriteFont highlightFont, string[] menuItems) : base(game)
        {
            this.spriteBatch = spriteBatch;
            this.regularFont = regularFont;
            this.highlightFont = highlightFont;

            tick = game.Content.Load<SoundEffect>("Music/menuTick");

            this.menuItems = menuItems.ToList<string>();
            
            position = new Vector2((Shared.boundary.X / 2) - 70, Shared.boundary.Y / 2 - 30);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 tempPos = position;
            spriteBatch.Begin();

            for (int i = 0; i < menuItems.Count; i++)
            {
                if (selectedIndex == i)
                {
                    spriteBatch.DrawString(highlightFont, menuItems[i], tempPos, highlightColor);
                    tempPos.Y += highlightFont.LineSpacing;
                }
                else
                {
                    spriteBatch.DrawString(regularFont, menuItems[i], tempPos, regularColor);
                    tempPos.Y += regularFont.LineSpacing;
                }

            }

            spriteBatch.End();


            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kbState = Keyboard.GetState();

            if (Shared.IsValidKeyPress(kbState, oldKbState, Keys.Down))
            {
                tick.Play();
                selectedIndex++;
                if (selectedIndex == menuItems.Count)
                {
                    selectedIndex = FIRST_INDEX;
                }
            }

            if (Shared.IsValidKeyPress(kbState, oldKbState, Keys.Up))
            {
                tick.Play();
                selectedIndex--;
                if (selectedIndex == TOO_LOW_INDEX)
                {
                    selectedIndex = menuItems.Count - MENU_OFFSET;
                }
            }

            oldKbState = kbState;

            base.Update(gameTime);
        }
    }
}
