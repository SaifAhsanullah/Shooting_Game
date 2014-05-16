using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DodgyBear
{
        public class GameOverMenu
        {
            MenuButton replayButton;
            Texture2D sprite;
            Rectangle drawRectangle;
            SpriteFont font;
            Vector2 scorePosition;
            Vector2 bearsKilled;
            Vector2 secondsSurvived;
            Vector2[] highScoreLocations = new Vector2[10];

            GameState gameState;


            public GameOverMenu(ContentManager contentManager, int windowWidth, int windowHeight, GameState state)
            {

                //Game Over Stuff
                this.sprite = contentManager.Load<Texture2D>("GameOver");
                drawRectangle = new Rectangle(windowWidth/2 - sprite.Width / 2, windowHeight/4 - ( (int) (sprite.Height / 1.2) ) , sprite.Width, sprite.Height);

                gameState = state;

                //Replay button Stuff
                replayButton = new MenuButton(contentManager, "play", "play_hl", ( (windowWidth / 2) - 20), (int)(windowHeight / 1.25), 4, state);

                //font stuff
                font = contentManager.Load<SpriteFont>("ScoreFont");

                //Stats Display Stuff
                scorePosition.X = (drawRectangle.X)+ windowWidth / 6;
                scorePosition.Y = (drawRectangle.Y) + (int)(windowHeight/2.80);

                bearsKilled.X = scorePosition.X;
                bearsKilled.Y = scorePosition.Y - 25;

                secondsSurvived.X = scorePosition.X;
                secondsSurvived.Y = scorePosition.Y - 50;

                for (int i = 0; i < 10; i++)
                {
                    highScoreLocations[i].X = scorePosition.X - 25;
                    highScoreLocations[i].Y = scorePosition.Y  + (50 + (25 * i + 1));
                }

            }

            public void Update(MouseState mouseState, SpriteBatch spriteBatch, ContentManager contentManager, SoundBank soundBank)
            {
                replayButton.Update(mouseState, spriteBatch, contentManager, soundBank);
            }

            public void Draw(SpriteBatch spriteBatch, Score score)
            {
                replayButton.Draw(spriteBatch);
                spriteBatch.Draw(sprite, drawRectangle, Color.White);

                string playerScore = "Score: " + score.playerScore.ToString();
                string killString = "Kills:  " + score.killedTeddys.ToString();
                string survivedString = "Time: " + score.survived.ToString();
                spriteBatch.DrawString(font, killString, bearsKilled, Color.White);
                spriteBatch.DrawString(font, playerScore, scorePosition, Color.White);
                spriteBatch.DrawString(font, survivedString, secondsSurvived, Color.White);

                for (int i = 9; i >= 0; i--)
                {
                    spriteBatch.DrawString(font, "High Score " + (i + 1) + ": " + score.Scores.getHighScore(i), highScoreLocations[i], Color.White);
                }

                    
            }
        }
}

