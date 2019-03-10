using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Controllers
{
    public class PlayerTimerController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private int timer;
        #endregion

        #region PlayerTimers
        public void StartTime(int time, Action<int> timerUpdateCallback)
        {
            timer = time;
            StartCoroutine(StartTimer(timerUpdateCallback));
        }

        IEnumerator StartTimer(Action<int> timerUpdateCallback)
        {
            while (timer > 0)
            {
                timer--;
                timerUpdateCallback(timer);
                yield return new WaitForSeconds(1f);
            }
        }
        #endregion
    }
}