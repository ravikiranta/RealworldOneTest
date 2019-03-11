using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Controllers
{
    //This class solely handles the player timer
    public class PlayerTimerController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private int timer;
        #endregion

        #region Properties
        public int Timer
        {
            get
            {
                return timer;
            }
        }
        #endregion

        #region PlayerTimers
        public void StartTime(int time, Action<int> timerUpdateCallback)
        {
            timer = time;
            StartCoroutine(StartTimer(timerUpdateCallback));
        }

        public void AddTime(int time)
        {
            timer += time;
        }

        IEnumerator StartTimer(Action<int> timerUpdateCallback)
        {
            while (timer > 0)
            {
                timer--;
                timerUpdateCallback(timer);
                yield return new WaitForSeconds(1f);
            }

            //If timer runs out, disable player movement and interactions
            GetComponent<PlayerBaseController>().playerMovementController.DisablePlayerMovement = true;
            GetComponent<PlayerBaseController>().playerInteractionsController.DisablePlayerInteractions = true;
        }
        #endregion
    }
}