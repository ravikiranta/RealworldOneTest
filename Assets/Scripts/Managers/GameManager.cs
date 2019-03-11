using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjects;
using Data;
using Enums;
using Controllers;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Managers {
    public class GameManager : MonoBehaviour {
        #region Variables
        [Header("Dev Settings")]
        [Range(0.1f, 10f)] [SerializeField] private float customerSpawnGap = 5.0f;      // Controls the spawn gap between customers
        [SerializeField] private string highScoreFolderName = "RealworldOne";           // Name of the highscore folder
        [SerializeField] private string highScoreFileName = "Highscores";               // Name of the highscore file
        [SerializeField] private string fileExtension = ".dat";                         // Highscore file extension

        [Header("Info")]
        [SerializeField] private int defaultScore;                                      // Default point the player gets for correct delivery
        [SerializeField] private int defaultPlayerTimer;                                // Default amount of time the player starts with
        [SerializeField] private HighScoreFileData highscoreList;                       // This variable contains the highscore data
        [SerializeField] private string folderPath;                                     // Folder path of the highscore file
        [SerializeField] private string highScoreFilePath;                              // Full path of the highscore file
        

        [Header("Databases")]
        [SerializeField] private FoodDatabaseScriptableObject foodDatabase;             // Food database used in the game
        [SerializeField] private ItemDatabaseScriptableObject itemDatabase;             // Item database used in the game for combining items
        [SerializeField] private PickupDatabaseScriptableObject pickupDatabase;         // Pickup database that is used in the game

        [Header("References")]
        [SerializeField] private List<PlayerBaseController> playerBaseControllers;      // List of players(playerBaseControllers) in the game
        [SerializeField] private List<GameObject> customerSpawnPoints;                  // Spawnpoints with the customer
        [SerializeField] private List<GameObject> pickupSpawnPoints;                    // Spwanpoints for the pickup
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
        void Awake()
        {
            // Calculate the highscore folder file
            folderPath = Path.Combine(Application.persistentDataPath, highScoreFolderName);
            //Calculate the fullpath of the highscore file
            highScoreFilePath = Path.Combine(folderPath, highScoreFileName + fileExtension);

            CheckForHighscoreFolder();
        }

        //Function to check if the folder exists or else to create
        void CheckForHighscoreFolder()
        {
            if (!Directory.Exists(Path.Combine(Application.persistentDataPath, highScoreFolderName)));
                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, highScoreFolderName));
        }


        void Start() {
            // Start spawning the customers
            StartCoroutine(AutoSpawnCustomers());

            // Load any highscores already existing
            LoadHighScores();
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
        // Give the customer a food suggestion from the food database
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

        // Return the result item of an action on a base item (like chopping)
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

        // Check if the food served was correct
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
        // Return a random pickup from the pickup database for the customre to spawn
        public PickupData ReturnRandomPickup()
        {
            PickupType randomPickup = (PickupType) Random.Range(0,System.Enum.GetValues((typeof(PickupType))).Length);
            PickupData pickup = pickupDatabase.pickups.Find(x => x.pickupType == randomPickup);
            if (pickup.pickupModel != null)
                return new PickupData(pickup.pickupType,pickup.pickupModel, pickup.pickupValue, pickup.lifeTime);
            else
            {
                Debug.Log("Pickup not found in database:" + randomPickup.ToString());
                return null;
            }
        }

        // Return  a random pickup spawn point at which the customer can spawn the pickup item
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
        // Add/Remove score to the player score controller
        public void UpdateScore(int addScore, PlayerID playerID)
        {
            playerBaseControllers[(int)playerID].UpdateScore(addScore);
        }

        // Check which player won the game
        public void CheckWinner()
        {
            string winningPlayerName = playerBaseControllers[0].playerData.PlayerID.ToString();
            int winningPlayerScore = playerBaseControllers[0].playerScoreController.Score;
            for(int i = 0; i < playerBaseControllers.Count; i++)
            {
                if (playerBaseControllers[0].playerScoreController.Score > winningPlayerScore)
                {
                    winningPlayerScore = playerBaseControllers[0].playerScoreController.Score;
                    winningPlayerName = playerBaseControllers[0].playerData.PlayerID.ToString();
                }
            }

            //Update the winner's name and score on the UI
            UIManager.Instance.ShowWinner(winningPlayerName, winningPlayerScore);
        }
        #endregion

        #region CheckGameOver
        // Check if all players timer's have reached zero
        public void CheckGameOverCondition()
        {
            bool gameOver = true;
            for(int i = 0; i < playerBaseControllers.Count; i++)
            {
                if(playerBaseControllers[i].playerTimeController.Timer > 0)
                {
                    gameOver = false;

                    // Add the new scores to the higshcore List
                    AddNewScoreToHighScoreList();

                    // Save the highscores to file
                    SaveHighScores();
                }
            }

            if (gameOver)
            {
                CheckWinner();
                UIManager.Instance.SetGameOverScreenActive(true);
                UIManager.Instance.UpdateHighScore(highscoreList);
            }
        }
        #endregion

        #region RestartGame
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        #endregion

        #region HighScore
        //Function to add the new scores from this game to the highscore list
        void AddNewScoreToHighScoreList()
        {
            for (int i = 0; i < playerBaseControllers.Count; i++)
            {
                highscoreList.highScoreContents.Add(
                    new HighscoreData(
                        playerBaseControllers[i].playerData.PlayerID.ToString(),
                            playerBaseControllers[i].playerScoreController.Score));

            }

            // Sort the list by score to get it ready for display
            SortHighScoreList();
        }

        //Function to save the highscores to file
        void SaveHighScores()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream fileStream = File.Open(highScoreFilePath, FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(fileStream, highscoreList);
            }
        }

        //Function to load any existing highscores
        void LoadHighScores()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if (File.Exists(highScoreFilePath))
            {
                using (FileStream fileStream = File.Open(highScoreFilePath, FileMode.Open))
                {
                    highscoreList = (HighScoreFileData)binaryFormatter.Deserialize(fileStream);
                    
                }
            }
            else
                Debug.Log("No save file found");

            // Sort the list by score to get it ready for display
            SortHighScoreList();
        }

        // Function to sort the highscores in the list in descending order
        void SortHighScoreList()
        {
            highscoreList.highScoreContents = new List<HighscoreData>(highscoreList.highScoreContents.OrderByDescending(x => x.score));
        }
        #endregion
    }
}

