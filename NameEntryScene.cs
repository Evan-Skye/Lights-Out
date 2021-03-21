using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace LightsOut
{
    public class NameEntryScene : GameScene
    {
        private int score;
        private static int scoreToSave;

        private bool needToShow;

        private SpriteFont regularFont;
        private SpriteFont highlightFont;

        List<HighScore> scores;
        static List<HighScore> scoresToSave;
        HighScore newScore;

        static string[] letters = new string[52]{ "A", "B", "C", "D", "E", "F", "G", "H" ,"I", "J", "K", "L", "M",
                "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
                "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};

        private Game game;

        private static Arrow[,] arrows;
        private static LetterSelector[] selectors;

        const int ARROWS_PER_LETTER = 2;
        const int NUMBER_OF_LETTERS = 6;

        const int INIT_Y = 200;
        const int LETTER_Y = 280;
        const int ARROW_HEIGHT = 70;
        const int ARROW_PADDED_HEIGHT = 140;

        const int INIT_X = 100;
        const int LETTER_X = 135;
        const int LETTER_WIDTH = 100;
        const int ARROW_WIDTH = 90;
        const int ARROW_PADDED_WIDTH = 100;

        const int INIT_INDEX = 0;
        const int TOP_ROW = 0;
        const int BOTTOM_ROW = 1;

        GameOverScene gameOverScene;

        int currentX, currentY;
        Rectangle srcRect;
        Vector2 srcVector;
        string direction;

        int currentIndex;
        static bool isFinished = false;

        Rectangle buttonRect;
        Texture2D buttonTex;

        MouseState msState;
        MouseState oldMsState;

        public int Score { get => score; set => score = value; }
        public bool NeedToShow { get => needToShow; set => needToShow = value; }

        public NameEntryScene(Game game, SpriteBatch spriteBatch, int score,
            SpriteFont regularFont, SpriteFont highlightFont, List<HighScore> scores,
            GameOverScene gameOverScene) : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
            this.score = score;
            this.regularFont = regularFont;
            this.highlightFont = highlightFont;
            this.scores = scores;
            this.tex = game.Content.Load<Texture2D>("Backgrounds/nameentry");
            this.buttonRect = new Rectangle(340, 410, 122, 55);
            this.buttonTex = game.Content.Load<Texture2D>("Images/done");
            this.gameOverScene = gameOverScene;
        }

        public override void Hide()
        {
            base.Hide();
        }

        public override void Show()
        {
            this.Enabled = true;
            this.Visible = true;

            //arrows per letter = columns
            //number of letters = rows

            if(score <= scores[9].Score)
            {
                this.needToShow = false;
                gameOverScene.NeedToShow = true;
            }

            else
            {
                selectors = new LetterSelector[NUMBER_OF_LETTERS];
                currentX = LETTER_X;
                for (int i = 0; i < NUMBER_OF_LETTERS; i++)
                {
                    srcVector = new Vector2(currentX, LETTER_Y);
                    selectors[i] = new LetterSelector(game, spriteBatch, i, srcVector, highlightFont);
                    currentX += LETTER_WIDTH;
                }

                scoreToSave = score;
                scoresToSave = scores;

                arrows = new Arrow[ARROWS_PER_LETTER, NUMBER_OF_LETTERS];
                currentX = INIT_X;
                currentY = INIT_Y;

                //initializes every arrow in the array.
                for (int i = 0; i < ARROWS_PER_LETTER; i++)
                {
                    if (i == TOP_ROW)
                    {
                        direction = "up";
                    }

                    else
                    {
                        direction = "down";
                    }


                    for (int j = 0; j < NUMBER_OF_LETTERS; j++)
                    {
                        srcRect = new Rectangle(currentX, currentY, ARROW_WIDTH, ARROW_HEIGHT);
                        arrows[i, j] = new Arrow(game, spriteBatch, direction, j, srcRect);

                        currentX += ARROW_PADDED_WIDTH;

                    }

                    currentY += ARROW_PADDED_HEIGHT;
                    currentX = INIT_X;

                }



                base.Show(); 
            }
        }

        private static void ArrowClick(Arrow arrow, MouseState msState, MouseState oldMsState)
        {
            Point mouseLocation = new Point(msState.X, msState.Y);

            //check if mouse.x & mouse.y are within the arrow's boundary rectangle
            if (arrow.destinationRect.Contains(mouseLocation))
            {
                if (Shared.IsValidClick(msState.LeftButton, oldMsState.LeftButton))
                {
                    int currentIndex = arrow.Placement;
                    string currentDirection = arrow.direction;
                    selectors[currentIndex].letterIndex = arrow.Increment();

                    if (arrow.direction == "up")
                    {
                        arrows[BOTTOM_ROW, currentIndex].letterIndex = selectors[currentIndex].letterIndex;
                    }

                    else
                    {
                        arrows[TOP_ROW, currentIndex].letterIndex = selectors[currentIndex].letterIndex;
                    }
                }
            }
        }

        private static void ButtonClick(Rectangle srcRect, MouseState msState, MouseState oldMsState)
        {
            Point mouseLocation = new Point(msState.X, msState.Y);

            //check if mouse.x & mouse.y are within the arrow's boundary rectangle
            if (srcRect.Contains(mouseLocation))
            {
                if (Shared.IsValidClick(msState.LeftButton, oldMsState.LeftButton))
                {
                    string playerName = "";

                    foreach(LetterSelector letter in selectors)
                    {
                        playerName += letters[letter.letterIndex];
                    }

                    scoresToSave.RemoveAt(9);
                    HighScore newScore = new HighScore(playerName, scoreToSave);
                    scoresToSave.Add(newScore);
                    Shared.SaveToFile(scoresToSave);
                    isFinished = true;
                    
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            msState = Mouse.GetState();

            foreach (Arrow arrow in arrows)
            {
                ArrowClick(arrow, msState, oldMsState);
            }

            ButtonClick(buttonRect, msState, oldMsState);

            if (isFinished)
            {
                gameOverScene.NeedToShow = true;
            }

            oldMsState = msState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, Vector2.Zero, Color.White);
            spriteBatch.Draw(buttonTex, buttonRect, Color.White);
            spriteBatch.End();

            foreach (Arrow arrow in arrows)
            {
                arrow.Draw(gameTime);
            }

            foreach (LetterSelector letter in selectors)
            {
                letter.Draw(gameTime);
            }



            base.Draw(gameTime);
        }
    }
}
