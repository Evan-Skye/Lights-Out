/*PlayScene.cs
 * The class that controls the scene where all of the Gameplay happens.
 * 
 * Revision History:
 *      04.Dec.2020, Evan Skye: Scaffolded.
 *      09.Dec.2020, Evan Skye: LightBox Generation code written.
 *      12.Dec.2020, Evan Skye: Game Logic written.
 *      13.Dec.2020, Evan Skye: Game Over Scene transition implemented.
 */

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace LightsOut
{
    public class PlayScene : GameScene
    {
        #region var declaration

        private const int INIT_Y = 5;
        private const int LIGHTBOX_HEIGHT = 150;
        private const int LIGHTBOX_PADDED_HEIGHT = 160;

        private const int INIT_X = 170;
        private const int LIGHTBOX_WIDTH = 150;
        private const int LIGHTBOX_PADDED_WIDTH = 160;

        private const int SCORE_INTERVAL = 5;
        private const int SCORE_TIME_MULTIPLIER = 3;

        private const int NUM_LIGHTS = 9;
        private const int LIGHTS_IN_ROW = 3;
        private const int LIGHTS_IN_COL = 3;
        private const float INTERVAL = 15f;
        private const float INIT_DELAY = 2f;
        private const float GAME_OVER_SONG_LENGTH = 3f;

        private const int SPEED_THRESHOLD_TIER_ONE = 2;
        private const int SPEED_THRESHOLD_TIER_TWO = 4;
        private const int SPEED_THRESHOLD_TIER_THREE = 6;
        private const float SPEED_REMOVAL_TIER_ONE = 0.5f;
        private const float SPEED_REMOVAL_TIER_TWO = 0.25f;
        private const float SPEED_REMOVAL_TIER_THREE = 0.1f;
        private const float SPEED_REMOVAL_TIER_FOUR = 0.05f;

        private const int DEFAULT_VALUE = 0;

        private const float REASONABLE_VOLUME = 0.25f;

        Game game;

        NameEntryScene nameEntryScene;
        GameOverScene gameOverScene;

        Song levelSong;
        Song gameOverSong;

        SoundEffect systemBoot;
        SoundEffect sparkLong;
        SoundEffect sparkMedium;
        SoundEffect sparkShort;

        bool gameOver;
        bool testAllOff;
        bool allOff = false;

        int currentX, currentY;
        int rowToFlip, colToFlip;
        int score;
        int toFlip;
        int timesSpedUp = 0;

        float delay;
        float gameStartTime;
        float gameElapsedTime;
        float gameEndTime;
        float endElapsedTime;
        float timeLastTurnedOff = 0f;

        Rectangle srcRect;

        Texture2D background;
        LightBox[,] lights;
        Random random = new Random();
        SpriteFont regularFont;

        MouseState msState;
        MouseState oldMsState;

        #endregion

        /// <summary>
        /// Constructor for the PlayScene.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        /// <param name="regularFont">Font to write the score in</param>
        public PlayScene(Game game, SpriteBatch spriteBatch, GameTime gameTime,
            SpriteFont regularFont, NameEntryScene nameEntryScene, GameOverScene gameOverScene) : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            levelSong = game.Content.Load<Song>("Music/songInGame");
            this.background = game.Content.Load<Texture2D>("Backgrounds/play");
            this.regularFont = regularFont;
            this.systemBoot = game.Content.Load<SoundEffect>("Music/systemBoot");
            this.gameOverSong = game.Content.Load<Song>("Music/songGameOver");
            this.sparkLong = game.Content.Load<SoundEffect>("Music/sparkLong");
            this.sparkMedium = game.Content.Load<SoundEffect>("Music/sparkMedium");
            this.sparkShort = game.Content.Load<SoundEffect>("Music/sparkShort");
            this.nameEntryScene = nameEntryScene;
            this.gameOverScene = gameOverScene;
            
        }
        public override void Initialize()
        {
            base.Initialize();
        }

        public void Show(GameTime gameTime)
        {
            MediaPlayer.Play(levelSong);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = REASONABLE_VOLUME;

            lights = new LightBox[LIGHTS_IN_ROW, LIGHTS_IN_COL];
            currentX = INIT_X;
            currentY = INIT_Y;

            //initializes every lightbox in the array.
            for (int i = 0; i < LIGHTS_IN_COL; i++)
            {
                for (int j = 0; j < LIGHTS_IN_ROW; j++)
                {
                    srcRect = new Rectangle(currentX, currentY, LIGHTBOX_WIDTH, LIGHTBOX_HEIGHT);
                    lights[i, j] = new LightBox(game, spriteBatch, srcRect);

                    currentX += LIGHTBOX_PADDED_WIDTH;

                }

                currentY += LIGHTBOX_PADDED_HEIGHT;
                currentX = INIT_X;

            }

            //set all dynamic variables to their default states.
            score = DEFAULT_VALUE;
            delay = INIT_DELAY;
            timesSpedUp = DEFAULT_VALUE;
            timeLastTurnedOff = DEFAULT_VALUE;
            gameElapsedTime = DEFAULT_VALUE;
            endElapsedTime = DEFAULT_VALUE;
            gameEndTime = DEFAULT_VALUE;
            allOff = false;
            gameOver = false;
            TurnOn();

            //sets the start time of the game to a variable, to be used as a reference later
            gameStartTime = (float)gameTime.TotalGameTime.TotalSeconds;

            base.Show();
        }

        /// <summary>
        /// Turns on every light when the game starts. (will play the animation when I implement tomorrow)
        /// </summary>
        private void TurnOn()
        {
            systemBoot.Play();
            foreach (LightBox light in lights)
            {
                light.OnSilent();
            }
        }

        /// <summary>
        /// The method that controls the speed at which the lights turn off.
        /// </summary>
        /// <param name="gameTime"></param>
        private void TurnOff(GameTime gameTime)
        {
            bool turnOffSuccessful = false;

            //if it's been long enough since the last speed up, it speeds up again
            if (gameElapsedTime > (INTERVAL * timesSpedUp))
            {
                //all of these do similar things. every individual speed tier has a specific number
                //of iterations that it goes through to simulate a difficulty curve. it plays a different
                //sound effect when you enter a new speed tier than it does when you're within it.
                //eventually you hit a speedcap where the tiles stop increasing in speed, at 20 disabling per second.

                if (timesSpedUp < SPEED_THRESHOLD_TIER_ONE)
                {
                    if (timesSpedUp != DEFAULT_VALUE) sparkShort.Play();
                    delay -= SPEED_REMOVAL_TIER_ONE;

                }

                else if (timesSpedUp < SPEED_THRESHOLD_TIER_TWO)
                {
                    if (timesSpedUp == SPEED_THRESHOLD_TIER_ONE) sparkMedium.Play();
                    else sparkShort.Play();
                    delay -= SPEED_REMOVAL_TIER_TWO;
                }

                else if (timesSpedUp < SPEED_THRESHOLD_TIER_THREE)
                {
                    if (timesSpedUp == SPEED_THRESHOLD_TIER_TWO) sparkMedium.Play();
                    else sparkShort.Play();
                    delay -= SPEED_REMOVAL_TIER_THREE;
                }

                else
                {
                    if (timesSpedUp == SPEED_REMOVAL_TIER_THREE) sparkLong.Play();
                    else sparkShort.Play();
                    if (delay > SPEED_REMOVAL_TIER_FOUR) delay -= SPEED_REMOVAL_TIER_FOUR;
                }

                timesSpedUp++;

            }

            //checks if the difference between the time elapsed during the game and the time when the last light was turned off
            //is greater than the required delay between light shut offs, and that all the lights aren't off already
            if (gameElapsedTime - timeLastTurnedOff > delay && !allOff)
            {
                do
                {
                    //selects a random number between 0 and 8 and sets it to the int
                    toFlip = random.Next(0, NUM_LIGHTS);

                    //integer dividing a number and moduloing the same number will give you its
                    //coordinates on a grid. this works for any square grid, as long as you're
                    //diving & moduloing by the square root of the square. was really excited to find this.
                    rowToFlip = toFlip / LIGHTS_IN_ROW;
                    colToFlip = toFlip % LIGHTS_IN_COL;

                    //LightBox.Off returns a bool, letting you know if it worked.
                    turnOffSuccessful = lights[rowToFlip, colToFlip].Off();

                //stays within the loop until you successfully turn off a lightbox.
                } while (!turnOffSuccessful);

                timeLastTurnedOff = gameElapsedTime;

            }

        }

        /// <summary>
        /// A psuedo-event handler for LightBoxes. If the box has been clicked with a unique click, it fires the On function.
        /// </summary>
        /// <param name="lightBox">The lightbox being evaluated</param>
        /// <param name="msState">The current state of the mouse</param>
        /// <param name="oldMsState">The state of the mouse from the previous update</param>
        private static void LightBoxClick(LightBox lightBox, MouseState msState, MouseState oldMsState)
        {
            Point mouseLocation = new Point(msState.X, msState.Y);

            //check if mouse.x & mouse.y are within the lightbox's boundary rectangle
            if (lightBox.destinationRect.Contains(mouseLocation))
            {
                if (Shared.IsValidClick(msState.LeftButton, oldMsState.LeftButton))
                {
                    lightBox.On();
                }
            }
        }

        /// <summary>
        /// The score is a coefficient of the time elapsed within the game, updating 3 times per second, 
        /// and increasing in intervals of 5.
        /// </summary>
        /// <param name="gameElapsedTime">The number of seconds elapsed since the game started</param>
        private void ScoreUpdate(float gameElapsedTime)
        {
            score = ((int)(gameElapsedTime * SCORE_TIME_MULTIPLIER)) * SCORE_INTERVAL;
        }

        /// <summary>
        /// Calls the in-game Game Over functionality, and then the Game Over scene. 
        /// </summary>
        /// <param name="gameTime"></param>
        private void GameOver(GameTime gameTime)
        {
            gameOver = true;

            //plays the gameoversong a single time. 
            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(gameOverSong);

            nameEntryScene.Score = score;
            gameEndTime = (float)gameTime.TotalGameTime.TotalSeconds;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, Vector2.Zero, Color.White);

            //draws the score to the screen in the top left corner.
            spriteBatch.DrawString(regularFont, " Score: " + score.ToString(), Vector2.Zero, Color.White);
            spriteBatch.End();

            //calls each lightbox's draw function
            foreach (LightBox light in lights)
            {
                light.Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            //only runs this code if the game isn't over
            if (!gameOver)
            {
                //keeps the elapsed time updated
                gameElapsedTime = (float)gameTime.TotalGameTime.TotalSeconds - gameStartTime;
                ScoreUpdate(gameElapsedTime);
                TurnOff(gameTime);
                msState = Mouse.GetState();

                //I was scared that setting allOff to true might mess up my logic, so I used a buffer.
                //if any of the lights are still on, allOff is set to false and the game keeps going.
                testAllOff = true;
                foreach (LightBox light in lights)
                {
                    LightBoxClick(light, msState, oldMsState);
                    if (light.IsOn == true)
                    {
                        testAllOff = false;
                    }

                    light.Update(gameTime);
                }

                allOff = testAllOff;
                if (allOff)
                {
                    GameOver(gameTime);
                }

                oldMsState = msState;

                base.Update(gameTime);
            }

            else
            {
                //waits for the game over song to finish playing before sending the user to the scene
                if (endElapsedTime > GAME_OVER_SONG_LENGTH)
                {
                    nameEntryScene.NeedToShow = true; 
                }

                endElapsedTime = (float)gameTime.TotalGameTime.TotalSeconds - gameEndTime;
            }

        }

    }
}
