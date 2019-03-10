using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;
using Data;
using Enums;
using Controllers;

namespace Managers {
    public class GameManager : MonoBehaviour {
        #region Variables
        [Header("Dev Settings")]
        [Range(0.1f, 10f)] [SerializeField] private float customerSpawnGap = 5.0f;

        [Header("Info")]
        [SerializeField] private int defaultScore;
        [SerializeField] private int defaultPlayerTimer;

        [Header("Databases")]
        [SerializeField] private FoodDatabaseScriptableObject foodDatabase;
        [SerializeField] private ItemDatabaseScriptableObject itemDatabase;
        [SerializeField] private PickupDatabaseScriptableObject pickupDatabase;

        [Header("References")]
        [SerializeField] private List<PlayerBaseController> playerBaseControllers;
        [SerializeField] private List<GameObject> customerSpawnPoints;
        [SerializeField] private List<GameObject> pickupSpawnPoints;
        #endregion

        #region Properties
        public int DefaultScore
        {
            get
            {
                return defaultScore;
            }
        }

        public int DefaultPlayerTimer
        {
            get
            {
                return defaultPlayerTimer;
            }
        }
        #endregion

        #region Singleton
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<GameManager>();

                return instance;
            }
        }
        #endregion

        #region Init
        void Start() {
            StartCoroutine(AutoSpawnCustomers());
            
        }

        IEnumerator AutoSpawnCustomers()
        {
            while (true)
            {
                CreateCustomers();
                yield return new WaitForSeconds(customerSpawnGap);
            }
        }

        void CreateCustomers() {
            for (int i = 0; i < customerSpawnPoints.Count; i++) {
                if (!customerSpawnPoints[i].activeInHierarchy) {
                    customerSpawnPoints[i].SetActive(true);
                    break;
                }
            }
        }
        #endregion

        #region FoodFunctions
        public string ReturnRandomFoodSuggestionForCustomer()
        {
            
            if(foodDatabase != null)
            {
                FoodData foodSugestion = foodDatabase.foodItems[Random.Range(0, foodDatabase.foodItems.Count - 1)];
                string food = "";
                for (int i = 0; i < foodSugestion.ingredients.Count; i++)
                {
                    
                    food += foodSugestion.ingredients[i];
                    if (i < foodSugestion.ingredients.Count - 1)
                        food += ", ";
                }
                return food;
            }
            else
            {
                Debug.Log("Attach a food database");
                return null;
            }
        }

        public string ReturnResultItemOnKitchenActionOnItem(string itemName, KitchenInteractions interaction)
        {
            switch (interaction)
            {
                case KitchenInteractions.Chop:
                    ItemData itemData = itemDatabase.items.Find(x => x.itemName == itemName);
                    if (itemData.possibleKitchenInteractions.Contains(interaction))
                    {
                        int resultPos = itemData.possibleKitchenInteractions.IndexOf(interaction);
                        if(resultPos > -1 && itemData.resultItems.Count > resultPos)
                        {
                            return itemData.resultItems[resultPos];
                        }
                        else
                        {
                            Debug.Log("No result item found for this interaction:" + interaction);
                            return null;
                        }
                    }
                    else
                    {
                        Debug.Log("This item does not have interaction:" + interaction);
                        return null;
                    }
            }
            return null;
        }

        public bool FoodServedCheck(string orderedFood, string servedFood)
        {
            if (orderedFood == servedFood)
            {
                Debug.Log("Food served correct");
                return true;
            }
            else
            {
                Debug.Log("Food served incorrect");
                return false;
            }
        }
        #endregion

        #region PickupFunctions
        public PickupData ReturnRandomPickup()
        {
            PickupType randomPickup = (PickupType) Random.Range(0,System.Enum.GetValues((typeof(PickupType))).Length - 1);
            PickupData pickup = pickupDatabase.pickups.Find(x => x.pickupType == randomPickup);
            if (pickup.pickupModel != null)
                return new PickupData(pickup.pickupType,pickup.pickupModel, pickup.pickupValue, pickup.lifeTime);
            else
            {
                Debug.Log("Pickup not found in database:" + randomPickup.ToString());
                return null;
            }
        }

        public Transform ReturnRandomPickupSpawnPoint()
        {
            if (pickupSpawnPoints.Count > 0)
            {
                int randomSpawnPoint = Random.Range(0, pickupSpawnPoints.Count - 1);
                return pickupSpawnPoints[randomSpawnPoint].transform;
            }
            else
                return null;
        }
        #endregion

        #region Scoring
        public void UpdateScore(int addScore, PlayerID playerID)
        {
            playerBaseControllers[(int)playerID].UpdateScore(addScore);
        }
        #endregion
    }
}
