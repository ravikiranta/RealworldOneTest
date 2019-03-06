using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;
using Data;

namespace Managers {
    public class GameManager : MonoBehaviour {
        #region Variables
        [Header("Dev Settings")]
        [Range(0.1f, 10f)] [SerializeField] private float customerSpawnGap = 5.0f;

        [Header("References")]
        [SerializeField] private FoodDatabaseScriptableObject foodDatabase;
        [SerializeField] private VegetableDatabaseScriptableObject vegetableDatabase;
        [SerializeField] private List<GameObject> customerSpawnPoints;
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
        public FoodData ReturnRandomFoodSuggestionForCustomer()
        {
            if(foodDatabase != null)
            {
                return foodDatabase.foodItems[Random.Range(0, foodDatabase.foodItems.Count - 1)];
            }
            else
            {
                Debug.Log("Attach a food database");
                return null;
            }
        }
        #endregion
    }
}
