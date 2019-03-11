using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Data
{
    [Serializable]
    public class HighscoreData
    {
        public string playerName;
        public int score;

        public HighscoreData(string playerName, int score)
        {
            this.playerName = playerName;
            this.score = score;
        }
    }

    [Serializable]
    public class HighScoreFileData
    {
        public List<HighscoreData> highScoreContents;
    }
}
