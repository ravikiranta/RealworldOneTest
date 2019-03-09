using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Controllers
{
    public class TimerUIController : MonoBehaviour
    {
        #region Variables
        [Header("References")]
        [SerializeField] private Image timeBar;
        [SerializeField] private float defaultCountDownSpeed = 1/3f;
        [SerializeField] private float countDownSpeed = 1/3f;
        [SerializeField] private float countDownSpeedIncrement = 0.25f;
        private float maxTime;
        #endregion

        #region Timer Functions
        public void StarTimer(float countDownTime, Action callback)
        {
            maxTime = countDownTime;
            ResetTimer();
            StartCoroutine(Timer(countDownTime, callback));
        }

        void ResetTimer()
        {
            countDownSpeed = defaultCountDownSpeed;
        }

        public void IncreaseCountDownSpeed()
        {
            countDownSpeed += countDownSpeedIncrement;
        }

        IEnumerator Timer(float countDownTime, Action callback)
        {
            while (countDownTime > 0)
            {
                countDownTime -= countDownSpeed;
                timeBar.fillAmount = countDownTime / maxTime;
                yield return new WaitForSeconds(1 / 3f);
            }
            callback();
        }
        #endregion
    }
}
