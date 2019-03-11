using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using Data;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] private List<UserInteractionPanelController> userInteractionPanelControllers;
        [SerializeField] private GameObject gameOverScreen;
        [SerializeField] private ScrollableListController highScoreUIController;
        [SerializeField] private Text winnerText;
        #endregion

        #region Singleton
        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<UIManager>();

                return instance;
            }
        }
        #endregion

        #region UIFunctions
        //Function to show the winner of the game
        public void ShowWinner(string text, int score)
        {
            winnerText.text = "Winner:" + text + " Score:" + score.ToString();
        }

        //Function to update the items in player hands to the HUD
        public void UpdatePlayerItemsInHand(int playerID, List<string> items)
        {
            userInteractionPanelControllers[playerID].Items = items;
        }

        //Function to update interaction messages from each player when next to interactables
        public void UpdatePlayerInteractionMessages(int playerID, string interactionMesage)
        {
            userInteractionPanelControllers[playerID].UserInteractionText = interactionMesage;
        }

        //Function to update player score to HUD
        public void UpdatePlayerScore(int playerID, int score)
        {
            userInteractionPanelControllers[playerID].UpdateScore(score);
        }

        //Function to update player timer to HUD
        public void UpdatePlayerTime(int playerID, int time)
        {
            userInteractionPanelControllers[playerID].UpdateTime(time);
        }

        // Function to disable/enable game over screen;
        public void SetGameOverScreenActive(bool show)
        {
            gameOverScreen.SetActive(show);
        }

        // Function to update the highscore on the game over UI
        public void UpdateHighScore(HighScoreFileData highScoreDataList)
        {
            highScoreUIController.UpdateUI(highScoreDataList);
        }

        // Function to restart the game
        public void RestartGame()
        {
            SetGameOverScreenActive(false);
            GameManager.Instance.RestartGame();
        }
        #endregion
    }
}
