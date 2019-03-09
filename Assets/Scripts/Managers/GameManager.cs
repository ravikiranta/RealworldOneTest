using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;
using Data;
using Enums;

namespace Managers {
    public class GameManager : MonoBehaviour {
        #region Variables
        [Header("Dev Settings")]
        [Range(0.1f, 10f)] [SerializeField] private float customerSpawnGap = 5.0f;
        

        [Header("Info")]
        [SerializeField] private int defaultScore;
        [SerializeField] private int defaultPlayerTimer;
        [SerializeField] private List<int> score;

        [Header("References")]
        [SerializeField] private FoodDatabaseScriptableObject foodDatabase;
        [SerializeField] private ItemDatabaseScriptableObject itemDatabase;
        [SerializeField] private List<GameObject> customerSpawnPoints;
        #endregion

        #region Properties
        public int DefaultScore
        {
            get
            {
                return defaultScore;
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
            UIManager.Instance.StartPlayerTimers(defaultPlayerTimer);
            score = new List<int>(2);
            score.Add(0);
            score.Add(0);
            UIManager.Instance.UpdatePlayerScore(score);
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
                    break;
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

        #region Scoring
        public void UpdateScore(int addScore, PlayerID player)
        {
            score[(int)player] += addScore;
            UIManager.Instance.UpdatePlayerScore(score);
        }
        #endregion
    }
}
