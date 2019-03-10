using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class UserInteractionPanelController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private List<string> items;
        [SerializeField] private Text userItemsInHand;
        [SerializeField] private Text userInteractionText;
        [SerializeField] private Text userScoreText;
        [SerializeField] private Text userTime;
        #endregion

        #region Properties
        public List<string> Items
        {
            set
            {
                items = value;
                userItemsInHand.text = "";
                for (int i = 0; i < items.Count; i++)
                {
                    userItemsInHand.text += items[i] + "\n";
                }
            }
        }

        public string UserInteractionText
        {
            set
            {
                userInteractionText.text = value;
            }
        }
        #endregion

        #region ScoreFunction
        public void UpdateScore(int score)
        {
            userScoreText.text = "Score:" + score.ToString();
        }
        #endregion

        #region TimeFunctions
        public void UpdateTime(int time)
        {
            userTime.text = "Time:" + time;
        }
        #endregion
    }
}
