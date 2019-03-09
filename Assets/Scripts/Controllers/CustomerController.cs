using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Data;
using GameInterfaces;
using Enums;
using UnityEngine.UI;

namespace Controllers
{
    public class CustomerController : MonoBehaviour,IInteractable, IServable
    {
        #region Variables
        [Header ("Dev Set Values")]
        [SerializeField] private string interactionControlsMessage;
        [SerializeField] private List<KitchenInteractions> possibleInteractions;
        [SerializeField] private float waitTime;

        [Header ("Info")]
        [SerializeField] private string foodOrdered;
        [SerializeField] private string foodServed;
        [SerializeField] private bool customerAngry;
        [SerializeField] private List<PlayerID> incorrectDelivery;

        [Header ("References")]
        [SerializeField] private TimerUIController timerUIController;
        [SerializeField] private Text orderTextUI;
        #endregion

        #region Init
        void OnEnable()
        {
            foodOrdered = GameManager.Instance.ReturnRandomFoodSuggestionForCustomer();
            timerUIController.StarTimer(waitTime, WaitTimeOver);
            orderTextUI.text = foodOrdered;
            customerAngry = false;
            incorrectDelivery.Clear();
        }

        void WaitTimeOver()
        {
            Debug.Log("Wait Time Over");
            // Double penality for person who delivered wrong
            if (customerAngry)
            for (int i = 0; i < incorrectDelivery.Count; i++)
            {
                GameManager.Instance.UpdateScore(-2 * GameManager.Instance.DefaultScore, incorrectDelivery[i]);
            }

            // Negative points for both players if customer leaves
            for (int i = 0; i < System.Enum.GetValues(typeof(PlayerID)).Length; i++)
            {
                GameManager.Instance.UpdateScore(-GameManager.Instance.DefaultScore,(PlayerID)i);
            }

            transform.parent.gameObject.SetActive(false);
        }

        void CheckMyFood(PlayerID playerID)
        {
            bool foodCheck = GameManager.Instance.FoodServedCheck(foodOrdered, foodServed);

            if (foodCheck)
            {
                GameManager.Instance.UpdateScore(GameManager.Instance.DefaultScore, playerID);
            }
            else
            {
                if (!incorrectDelivery.Contains(playerID))
                    incorrectDelivery.Add(playerID);

                customerAngry = true;
                timerUIController.IncreaseCountDownSpeed();
            }
            Debug.Log("Serve Accepted:" + foodCheck);
        }

        void IServable.Serve(string food, PlayerID playerID)
        {
            foodServed = food;
            CheckMyFood(playerID);
        }

        string IInteractable.GetInteractionControls()
        {
            return interactionControlsMessage;
        }

        List<KitchenInteractions> IInteractable.GetPossibleInteractions()
        {
            return possibleInteractions;
        }
        #endregion
    }
}
