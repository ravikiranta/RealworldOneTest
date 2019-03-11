using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Controllers {
    //This class is on the scrollable list on the highscore table and allows highscore to be updated
    public class ScrollableListController : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] private List<GameObject> scoreContainers;
        #endregion
        
        #region Functions
        public void UpdateUI(HighScoreFileData highscoreList)
        {
            for (int i = 0;i < scoreContainers.Count; i++)
            {
                if (i < highscoreList.highScoreContents.Count)
                {
                    scoreContainers[i].GetComponent<ScoreContainerController>()
                        .UpdateData(highscoreList.highScoreContents[i].playerName, highscoreList.highScoreContents[i].score);
                }
            }
        }
        #endregion
    }
}