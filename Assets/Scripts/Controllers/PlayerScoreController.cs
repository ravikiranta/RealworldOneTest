﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace Controllers
{
    //This class solely handles the score of the player
    public class PlayerScoreController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private int score;
        #endregion

        #region Properties
        public int Score
        {
            get
            {
                return score;
            }
        }
        #endregion

        #region ScoreFunctions
        public void UpdateScore(int addScore)
        {
            score += addScore;
        }
        #endregion
    }
}
