using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;


namespace DodgyBear
{
    public class HighScoreTable
    {
        #region Fields

        //Texture2D background;

        //Rectangle drawRectangle;
        //SpriteFont font;
        HighScores highScores;

        //Storage
        StorageContainer storageContainer;
        const string CONTAINER_NAME = "Dodgy Bear";
        const string FILE_PATH = "HighScores";
        public List<string> scoreStrings = new List<string>();
        #endregion

        #region Constructor

        public HighScoreTable(ContentManager contentManager, int windowWidth, int windowHeight, int score)
        {
            //font = spriteFont;

            //background = contentManager.Load<Texture2D>("highScoreBackground");

            highScores = GetHighScores();

            bool scoreAdded = highScores.AddScore(score);


            List <int>scores = highScores.Scores;

            for (int i = 0; i < scores.Count; i++ )
            {
                scoreStrings.Add("High Score " + (i + 1) + ": " + scores[i]);
            }

                if (scoreAdded)
                {
                    SaveHighScores();
                }

        }

        #endregion


        public void ReceiveScores(Score score)
        {
            highScores = GetHighScores();
            bool scoreAdded = highScores.AddScore(score.score);
            List<int> scores = highScores.Scores;
            for (int i = 0; i < scores.Count; i++)
            {
                scoreStrings.Add("High Score " + (i + 1) + ": " + scores[i]);
            }

            if(scoreAdded)
            {
                SaveHighScores();
            }

        }



        #region Get Scores

        private HighScores GetHighScores()
        {
            //open the default windows storage device
            IAsyncResult result = StorageDevice.BeginShowSelector(null, null);

            result.AsyncWaitHandle.WaitOne();

            StorageDevice storageDevice = StorageDevice.EndShowSelector(result);

            result.AsyncWaitHandle.Close();


            //Opens the Dodgy Bear Container
            result = storageDevice.BeginOpenContainer(CONTAINER_NAME, null, null);

            result.AsyncWaitHandle.WaitOne();

            storageContainer = storageDevice.EndOpenContainer(result);

            result.AsyncWaitHandle.Close();

            //Read High Scores from the file
            BinaryFormatter formatter = new BinaryFormatter();
            Stream reader = storageContainer.OpenFile(FILE_PATH, FileMode.OpenOrCreate);

            highScores = null;
            try
            {
                if(reader.Length > 0)
                {
                    highScores = (HighScores)formatter.Deserialize(reader);
                }
                else
                {
                    highScores = new HighScores();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
            }

            return highScores;
            
        }

        #endregion

        #region Save Scores
        private void SaveHighScores()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream writer = storageContainer.OpenFile(FILE_PATH, FileMode.Create);

            try
            {
                formatter.Serialize(writer, highScores);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                writer.Close();
            }

        }
        #endregion

        #region Draw
        //public void Draw(SpriteBatch spriteBatch)
        //{
            
        //    for (int index = 0; index < scores.Count; index++)
        //    {
                
        //        switch (index)
        //        {
        //            case 0:
        //            {
        //                Vector2 score1;
        //                score1 = scorePosition;
        //                score1.Y = scorePosition.Y - 50;
        //                spriteBatch.DrawString(font, ("High Score " + (index + 1) + ": " + scores[index]), score1, Color.White);
        //                break;
        //            }
        //            case 1:
        //            {
        //                Vector2 score2;
        //                score2 = scorePosition;
        //                score2.Y = scorePosition.Y - 60;
        //                spriteBatch.DrawString(font, ("High Score " + (index + 1) + ": " + scores[index]), score2, Color.White);
        //                break;
        //            }
        //            case 2:
        //            {
        //                Vector2 score3;
        //                score3 = scorePosition;
        //                score3.Y = scorePosition.Y - 70;
        //                spriteBatch.DrawString(font, ("High Score " + (index + 1) + ": " + scores[index]), score3, Color.White);
        //                break;
        //            }
        //            case 3:
        //            {
        //                Vector2 score4;
        //                score4 = scorePosition;
        //                score4.Y = scorePosition.Y - 80;
        //                spriteBatch.DrawString(font, ("High Score " + (index + 1) + ": " + scores[index]), score4, Color.White);
        //                break;
        //            }
        //            case 4:
        //            {
        //                Vector2 score5;
        //                score5 = scorePosition;
        //                score5.Y = scorePosition.Y - 90;
        //                spriteBatch.DrawString(font, ("High Score " + (index + 1) + ": " + scores[index]), score5, Color.White);
        //                break;
        //            }
        //            case 5:
        //            {
        //                Vector2 score6;
        //                score6 = scorePosition;
        //                score6.Y = scorePosition.Y - 100;
        //                spriteBatch.DrawString(font, ("High Score " + (index + 1) + ": " + scores[index]), score6, Color.White);
        //                break;
        //            }
        //            case 6:
        //            {
        //                Vector2 score7;
        //                score7 = scorePosition;
        //                score7.Y = scorePosition.Y - 110;
        //                spriteBatch.DrawString(font, ("High Score " + (index + 1) + ": " + scores[index]), score7, Color.White);
        //                break;
        //            }
        //            case 7:
        //            {
        //                Vector2 score8;
        //                score8 = scorePosition;
        //                score8.Y = scorePosition.Y - 120;
        //                spriteBatch.DrawString(font, ("High Score " + (index + 1) + ": " + scores[index]), score8, Color.White);
        //                break;
        //           }
        //           case 8:
        //           {
        //                Vector2 score9;
        //                score9 = scorePosition;
        //                score9.Y = scorePosition.Y - 130;
        //                spriteBatch.DrawString(font, ("High Score " + (index + 1) + ": " + scores[index]), score9, Color.White);
        //                break;
        //           }
        //           case 9:
        //           {
        //                Vector2 score10;
        //                score10 = scorePosition;
        //                score10.Y = scorePosition.Y - 140;
        //                spriteBatch.DrawString(font, ("High Score " + (index + 1) + ": " + scores[index]), score10, Color.White);
        //                break;
        //           }
        //        }
                    
                    
                    

        //    }
        //}
        #endregion
    }
}
