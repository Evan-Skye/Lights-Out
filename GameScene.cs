﻿/*GameScene.cs
 * A class that's essentially an interface for every other game scene.
 * Mostly handles the Hide & Show methods, and ensures that the Draw & Update methods work correctly.
 * 
 * Revision History:
 *      04.Dec.2020, Evan Skye: Scaffolded & Written.
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
    public abstract class GameScene : DrawableGameComponent
    {
        private List<GameComponent> components;
        public List<GameComponent> Components { get => components; set => components = value; }

        protected SpriteBatch spriteBatch;
        protected Texture2D tex;


        public virtual void Show()
        {
            this.Enabled = true;
            this.Visible = true;
        }

        public virtual void Hide()
        {
            this.Enabled = false;
            this.Visible = false;
        }


        protected GameScene(Game game) : base(game)
        {
            components = new List<GameComponent>();
            Hide();
        }



        public override void Draw(GameTime gameTime)
        {
            DrawableGameComponent comp = null;
            foreach (GameComponent item in components)
            {
                if (item is DrawableGameComponent)
                {
                    comp = (DrawableGameComponent)item;
                    if (comp.Visible)
                    {
                        comp.Draw(gameTime);
                    }
                }
            }

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent item in components)
            {
                if (item.Enabled)
                {
                    item.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }
    }
}