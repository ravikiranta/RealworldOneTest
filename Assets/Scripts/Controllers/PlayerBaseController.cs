using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Data;

namespace Controllers
{
    [RequireComponent(typeof(PlayerData))]
    [RequireComponent(typeof(PlayerMovementController))]
    [RequireComponent(typeof(PlayerInteractionsController))]
    [RequireComponent(typeof(PlayerTimerController))]
    [RequireComponent(typeof(PlayerScoreController))]

    // This class acts as a root class which can give access to all the components of the player gameobject
    public class PlayerBaseController : MonoBehaviour
    {
        #region Variables
        public PlayerData playerData;
        public PlayerMovementController playerMovementController;
        public PlayerInteractionsController playerInteractionsController;
        public PlayerTimerController playerTimeController;
        public PlayerScoreController playerScoreController;
        #endregion

        #region Init
        void Awake()
        {
            playerData = GetComponent<PlayerData>();
            playerMovementController = GetComponent<PlayerMovementController>();
            playerInteractionsController = GetComponent<PlayerInteractionsController>();
            playerTimeController = GetComponent<PlayerTimerController>();
            playerScoreController = GetComponent<PlayerScoreController>();
        }

        void Start()
        {
            Init();
        }

        void Init()
        {
            playerTimeController.StartTime(GameManager.Instance.DefaultPlayerTimer, TimerUpdateCallback);
            UpdateScore(0);
        }
        #endregion

        #region Score
        public void UpdateScore(int addScore)
        {
            // Update score on the score component
            playerScoreController.UpdateScore(addScore);
            
            //Update score on the player HUD
            UIManager.Instance.
                UpdatePlayerScore((int)playerData.PlayerID, playerScoreController.Score);
        }
        #endregion

        #region TimerUpdate
        //This function is a callback from the timer component to the root class
        void TimerUpdateCallback(int time)
        {
            // Update the player time on the UI
            UIManager.Instance.UpdatePlayerTime((int)playerData.PlayerID,time);

            // If the timer ran out check for game over condition
            if (time == 0)
                GameManager.Instance.CheckGameOverCondition();
        }
        #endregion
    }
}
