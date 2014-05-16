using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DodgyBear
{
    public class Score
    {
        int counter;
        public int playerScore;
        public int killedTeddys;
        public int survived;
        Vector2 scorePosition;
        SpriteFont scoreFont;

        int framesPerSecond = 60;
        int scorePerSecond = 5;
        int scorePerKill = 15;

        HighScores scores;

        public Score(ContentManager Content, string ScoreFont, int x, int y)
        {
            scorePosition.X = 10;
            scorePosition.Y = 10;
            Load(Content, ScoreFont);

            scores = new HighScores();
        }

        #region Properties

        public HighScores Scores
        {
            get {return scores; }
        }

        #endregion


        #region Public Methods

        public void Update()
        {
            counter++;
            if (counter == framesPerSecond)
            {
                playerScore += scorePerSecond;
                counter = 0;
                survived++;
            }
        }

        public void KillBonus()
        {
            playerScore += scorePerKill;
            killedTeddys++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(scoreFont, "Score: " + playerScore, scorePosition, Color.White);
        }

        public void Reset()
        {
            playerScore = 0;
            counter = 0;
            survived = 0;
            killedTeddys = 0;
        }

        #region High Score Behavior


        public void loadHighScores()
        {
            FileStream input = null;

            try
            {
                input = File.OpenRead("HighScores.dat");
                BinaryFormatter formatter = new BinaryFormatter();
                HighScores fileScores = (HighScores)formatter.Deserialize(input);

                scores = fileScores;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {

                if (input != null)
                {
                    input.Close();
                }
            }

        }

        public void saveHighScores()
        {
            FileStream output = null;

            try
            {
                output = File.OpenWrite("HighScores.dat");
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(output, scores);               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (output != null)
                {
                    output.Close();
                }
            }
        }

        public bool AddScore (int score)
        {
            bool added = scores.AddScore(score);

            return added;
        }

        #endregion
        #endregion


        #region Private Methods

        private void Load(ContentManager Content, string ScoreFont)
        {
            scoreFont = Content.Load<SpriteFont>(ScoreFont);
        }

        #endregion

    }
}
