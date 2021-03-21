/*LightBox.cs
 * The class that represents the main object in the PlayScene.
 * 
 * Revision History:
 *      09.Dec.2020, Evan Skye: Scaffolded & Written.
 *      13.Dec.2020, Evan Skye: Added animation support via Brighten.cs
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace LightsOut
{
    class LightBox : DrawableGameComponent
    {
        private Game game;
        private SpriteBatch spriteBatch;
        Brighten brighten;
        public Rectangle destinationRect;
        private Texture2D currentTex;
        private Texture2D lightOff;
        private Texture2D lightOn;
        private Texture2D lightSpriteSheet;
        private SoundEffect switchOn;
        private SoundEffect switchOff;
        private bool isOn = false;

        public bool IsOn { get => isOn; set => isOn = value; }

        public LightBox(Game game, SpriteBatch spriteBatch, Rectangle destinationRect) : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.destinationRect = destinationRect;
            this.lightOn = game.Content.Load<Texture2D>("Images/lightOn");
            this.lightOff = game.Content.Load<Texture2D>("Images/lightOff");
            this.switchOn = game.Content.Load<SoundEffect>("Music/switchOn");
            this.switchOff = game.Content.Load<SoundEffect>("Music/switchOff");

            lightSpriteSheet = game.Content.Load<Texture2D>("Images/LightBoxSpriteSheet");
            brighten = new Brighten(game, spriteBatch, lightSpriteSheet, destinationRect);
            game.Components.Add(brighten);

        }

        /// <summary>
        /// Enables the light as long as it's off. Plays a sound effect. Fired by player.
        /// </summary>
        public void On()
        {
            if (!isOn)
            {
                isOn = true;
                currentTex = lightOn;
                switchOn.Play();
            }

        }

        /// <summary>
        /// Enables the light without a security check or sound effect. Used only by the backend.
        /// </summary>
        public void OnSilent()
        {
            isOn = true;
            brighten.Start();
            currentTex = lightOn;
        }

        /// <summary>
        /// Disables the light. Used only by the backend.
        /// </summary>
        /// <returns>If the light was successfully disabled.</returns>
        public bool Off()
        {
            if (isOn)
            {
                isOn = false;
                currentTex = lightOff;
                switchOff.Play();
                return true;
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(currentTex, destinationRect, Color.White);
            spriteBatch.End();


            base.Draw(gameTime);
        }


    }
}
