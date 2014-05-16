using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DodgyBear
{
    [Serializable]
    public class HighScores
    {
        #region Fields
        const int MAX_SCORES = 10;

        List<int> scores = new List<int>();

        #endregion

        #region Constructor
        public HighScores()
        {

        }
        #endregion


        public int getHighScore(int x)
        {
            return scores[x];
        }

        #region Public Methods

        public bool AddScore(int score)
        {
            if (scores.Count< MAX_SCORES || score > scores[MAX_SCORES - 1])
            {
                if(scores.Count == MAX_SCORES)
                {
                    scores.RemoveAt(MAX_SCORES - 1);
                }

                int addLocation = 0;
                while(addLocation < scores.Count && scores[addLocation] > score)
                {
                    addLocation++;
                }

                scores.Insert(addLocation, score);

                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion
    }
}
