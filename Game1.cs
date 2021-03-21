/*Game1.cs
 * The main class for the game. Swaps the scenes and is the root of all recursive update & draw functions.
 * 
 * Revision History:
 *      04.Dec.2020, Evan Skye: Created & Scaffolded.
 *      08.Dec.2020, Evan Skye: Added SoundEffects & Songs.
 *      09.Dec.2020, Evan Skye: Added High Score Scene support.
 *      12.Dec.2020, Evan Skye: Added File IO support.
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace LightsOut
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private bool gameOver = false;
        private int score = 0;

        const int SUPERFLUOUS_VALUE = 0;
        const float REASONABLE_VOLUME = 0.1f;

        const int START_SCENE = 0;
        const int HELP_SCENE = 1;
        const int HIGH_SCORE_SCENE = 2;
        const int CREDITS_SCENE = 3;
        const int QUIT = 4;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static GameTime gameTime;

        Song menuSong;

        SoundEffect menuAccept;
        SoundEffect menuCancel;

        SpriteFont regularFont;
        SpriteFont highlightFont;

        StartScene startScene;
        PlayScene playScene;
        HelpScene helpScene;
        HighScoreScene highScoreScene;
        CreditsScene creditsScene;
        GameOverScene gameOverScene;
        NameEntryScene nameEntryScene;

        List<HighScore> highScores;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            


        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;
            base.Initialize();


        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Shared.boundary = new Vector2(graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);

            //load menu song
            menuSong = this.Content.Load<Song>("Music/songTitle");

            //load sound effects
            menuAccept = this.Content.Load<SoundEffect>("Music/menuAccept");
            menuCancel = this.Content.Load<SoundEffect>("Music/menuCancel");

            //load fonts
            regularFont = this.Content.Load<SpriteFont>("Fonts/RegularFont");
            highlightFont = this.Content.Load<SpriteFont>("Fonts/HighlightFont");

            //instantiate and load startScene
            startScene = new StartScene(this, spriteBatch, regularFont, highlightFont);
            this.Components.Add(startScene);
            startScene.Show();
            MediaPlayer.Play(menuSong);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = REASONABLE_VOLUME;

            //instantiate other scenes


            helpScene = new HelpScene(this, spriteBatch);
            this.Components.Add(helpScene);

            creditsScene = new CreditsScene(this, spriteBatch);
            this.Components.Add(creditsScene);

            //file IO for high scores
            highScores = Shared.PopulateScoreList();
            highScoreScene = new HighScoreScene(this, spriteBatch, regularFont, highlightFont, highScores);
            this.Components.Add(highScoreScene);

            gameOverScene = new GameOverScene(this, spriteBatch, SUPERFLUOUS_VALUE, regularFont, highlightFont, highScores);
            this.Components.Add(gameOverScene);

            nameEntryScene = new NameEntryScene(this, spriteBatch, SUPERFLUOUS_VALUE, regularFont, highlightFont, highScores, gameOverScene);
            this.Components.Add(nameEntryScene);

            playScene = new PlayScene(this, spriteBatch, Game1.gameTime, regularFont, nameEntryScene, gameOverScene);
            this.Components.Add(playScene);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            int selectedIndex = 0;
            KeyboardState ks = Keyboard.GetState();

            if (startScene.Enabled)
            {
                selectedIndex = startScene.Menu.SelectedIndex;
                if (selectedIndex == START_SCENE && ks.IsKeyDown(Keys.Enter))
                {
                    startScene.Hide();
                    playScene.Show(gameTime);
                    menuAccept.Play();

                }
                if (selectedIndex == HELP_SCENE && ks.IsKeyDown(Keys.Enter))
                {
                    startScene.Hide();
                    helpScene.Show();
                    menuAccept.Play();
                }
                if (selectedIndex == HIGH_SCORE_SCENE && ks.IsKeyDown(Keys.Enter))
                {
                    startScene.Hide();
                    highScoreScene.scores = Shared.PopulateScoreList();
                    highScoreScene.Show();
                    menuAccept.Play();
                }
                if (selectedIndex == CREDITS_SCENE && ks.IsKeyDown(Keys.Enter))
                {
                    startScene.Hide();
                    creditsScene.Show();
                    menuAccept.Play();
                }
                if (selectedIndex == QUIT && ks.IsKeyDown(Keys.Enter))
                {
                    Exit();
                }
            }

            if(playScene.Enabled)
            {
                if (nameEntryScene.NeedToShow)
                {
                    playScene.Hide();
                    nameEntryScene.Show();
                }

                //else
                //{
                //    gameOverScene.Show();
                //}
            }

            if (nameEntryScene.Enabled)
            {
                if (gameOverScene.NeedToShow)
                {
                    nameEntryScene.Hide();
                    gameOverScene.Show();
                }
            }

            if (gameOverScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    gameOverScene.Hide();
                    startScene.Show();
                    menuCancel.Play();
                    MediaPlayer.Volume = REASONABLE_VOLUME;
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(menuSong);
                }

                else if (ks.IsKeyDown(Keys.Enter) || ks.IsKeyDown(Keys.Space))
                {
                    gameOverScene.Hide();
                    playScene.Show(gameTime);
                }

            }

            if (helpScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    helpScene.Hide();
                    startScene.Show();
                    menuCancel.Play();
                }
            }

            if (creditsScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    creditsScene.Hide();
                    startScene.Show();
                    menuCancel.Play();
                }
            }

            if (highScoreScene.Enabled)
            {
                if (ks.IsKeyDown(Keys.Escape))
                {
                    highScoreScene.Hide();
                    startScene.Show();
                    menuCancel.Play();
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
