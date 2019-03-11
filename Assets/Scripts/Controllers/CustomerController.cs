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
    // This class controls the behaviour of the customers coming to the shop
    public class CustomerController : MonoBehaviour,IInteractable, IServable
    {
        #region Variables
        [Header ("Dev Set Values")]
        [SerializeField] private string interactionControlsMessage;
        [SerializeField] private List<KitchenInteractions> possibleInteractions;
        [SerializeField] private float waitTime;
        [SerializeField] [Range(0,100)] private float percentageWaitTimeBeforeWhichPickupSpawns;

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
        void Start()
        {
            //CreateRandomPickup();   //Testing
        }

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
            //Debug.Log("Wait Time Over");
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
            // Check if the delivered food is matching with the requested food
            bool foodCheck = GameManager.Instance.FoodServedCheck(foodOrdered, foodServed);

            // If matching
            if (foodCheck)
            {
                GameManager.Instance.UpdateScore(GameManager.Instance.DefaultScore, playerID);

                // Check if the item was delivered on time and spawn pickup reward for this player
                CheckTimerForPickupSpawn(playerID);

                // Deactivate the cusomter
                transform.parent.gameObject.SetActive(false);
            }
            else
            {
                // If the deliver was wrong add this player to incorrect delivery list
                if (!incorrectDelivery.Contains(playerID))
                    incorrectDelivery.Add(playerID);

                // Customer is now angry
                customerAngry = true;

                // Wait timer counts down faster
                timerUIController.IncreaseCountDownSpeed();
            }
            //Debug.Log("Serve Acceptance:" + foodCheck);
        }

        void CheckTimerForPickupSpawn(PlayerID playerID)
        {
            if (timerUIController.ReturnFractionOfTimeLeft() * 100 >= percentageWaitTimeBeforeWhichPickupSpawns)
            {
                CreateRandomPickup(playerID);
            }
        }

        void CreateRandomPickup(PlayerID playerID)
        {
            // Give random pickup reward
            PickupData randomPickup = GameManager.Instance.ReturnRandomPickup();

            // Give random reward spawn point
            Transform spawnPoint = GameManager.Instance.ReturnRandomPickupSpawnPoint();
            if (spawnPoint != null)
            {
                if (randomPickup.pickupModel != null)
                {
                    GameObject pickup = Instantiate(
                        randomPickup.pickupModel, spawnPoint.position, spawnPoint.rotation, spawnPoint);
                    
                    // Set the data for this pickup like value, lifetime, pickup player id who can pick this item up
                    pickup.GetComponent<PickupController>().Init(randomPickup, playerID);
                }
                else
                    Debug.Log("Random Pickup is null:" + randomPickup);
            }
            else
                Debug.Log("No spawn point available on game manager");
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
