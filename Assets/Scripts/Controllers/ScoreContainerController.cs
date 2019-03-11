using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers {
    //This class holds the rows in the highscore table for each player and their score
    public class ScoreContainerController : MonoBehaviour
    {
        [SerializeField] private Text playerName;
        [SerializeField] private Text playerScore;

        public void UpdateData(string playerName, int score)
        {
            this.playerName.text = playerName;
            this.playerScore.text = score.ToString();
        }
    }
}
