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
        }
        #endregion

        #region Score
        public void UpdateScore(int addScore)
        {
            playerScoreController.UpdateScore(addScore);
            UIManager.Instance.
                UpdatePlayerScore((int)playerData.PlayerID, playerScoreController.Score);
        }
        #endregion

        #region TimerUpdate
        void TimerUpdateCallback(int time)
        {
            UIManager.Instance.UpdatePlayerTime((int)playerData.PlayerID,time);
        }
        #endregion
    }
}
