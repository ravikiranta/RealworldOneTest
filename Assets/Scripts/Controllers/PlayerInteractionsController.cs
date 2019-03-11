using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInterfaces;
using Managers;
using Data;
using Enums;

namespace Controllers {
    [RequireComponent(typeof(PlayerData))]
    [RequireComponent(typeof(PlayerMovementController))]
    // This class controls most of the interactions the player has with the items in the kitchen
    public class PlayerInteractionsController : MonoBehaviour
    {
        #region Variables
        [Header("Dev Settings")]
        [SerializeField] private int itemInventoryLimit;                                //Max items the user can carry

        [Header("Info")]
        [SerializeField] private List<string> itemsInHand;                              //List of items in the user's hand
        [SerializeField] private GameObject currentInteractableGameObject;              //Item overlapping the trigger for this player
        [SerializeField] private IInteractable currentInteractable;                     //Interface of the object overlapping (if any)
        [SerializeField] private List<KitchenInteractions> currentPossibleInteractions; //Possible interactions user can do with this object (if any)
        [SerializeField] private bool disablePlayerInteractions;                        //This flag disables player interactions

        [Header("References")]
        [SerializeField] private PlayerBaseController playerBaseController;
        [SerializeField] private GameObject playerChoppingText;                          //UI showing the chop text while user is chopping
        #endregion

        #region Properties
        public bool DisablePlayerInteractions
        {
            set
            {
                disablePlayerInteractions = value;
            }
            get
            {
                return disablePlayerInteractions;
            }
        }
        #endregion

        #region Init
        void Awake()
        {
            playerBaseController = GetComponent<PlayerBaseController>();
        }
        #endregion

        #region Event
        public delegate void ActionCompleteCallback();
        ActionCompleteCallback ChopCompleteCallback;    //Callback used to intimate the completion of chop action
        #endregion

        #region OnTriggerFunctions
        void OnTriggerStay(Collider objectCollider)
        {
            // Check if the items is interactable
            currentInteractable = objectCollider.GetComponent<GameInterfaces.IInteractable>();
            {
                if(currentInteractable != null)
                {
                    currentInteractableGameObject = objectCollider.gameObject;
                    currentPossibleInteractions = new List<KitchenInteractions>(currentInteractable.GetPossibleInteractions());

                    // Update the UI about the current item the user it touch or actions that can be performed
                    UIManager.Instance.UpdatePlayerInteractionMessages(
                        (int)playerBaseController.playerData.PlayerID, currentInteractable.GetInteractionControls());
                }
            }
        }

        void OnTriggerExit(Collider exitObjectCollider)
        {
            if(currentInteractable == exitObjectCollider.GetComponent<GameInterfaces.IInteractable>())
            {
                //Debug.Log("Exited existing interactable");
                
                // Clear possible interactions
                currentPossibleInteractions.Clear();
                currentInteractableGameObject = null;
                
                //Update UI interactions text
                UIManager.Instance.UpdatePlayerInteractionMessages((int)playerBaseController.playerData.PlayerID, "");
            }
            else
            {
                //Debug.Log("Entered another interactable");
            }
        }
        #endregion

        #region PlayerFunctions
        // Function to retrieve items from storages/sources
        void RetrieveItem()
        {
            if (itemsInHand.Count < itemInventoryLimit)
            {
                itemsInHand.Add(currentInteractableGameObject.GetComponent<IRetrieve>().Retrieve());
                UpdateItemsInUI();
            }
        }

        // Function to pickup an item
        void PickupItem()
        {
            if (itemsInHand.Count < itemInventoryLimit && currentInteractableGameObject != null)
            {
                currentInteractableGameObject.GetComponent<IPickup>().PickupItem(PickedUpCallback,playerBaseController.playerData.PlayerID);
            }
        }

        // Function to identify what type of item was picked up
        void PickedUpCallback(PickupData pickupData)
        {
            //Debug.Log("Got Pickup:" + pickupData.pickupType);
            if (pickupData != null)
            {
                switch (pickupData.pickupType)
                {
                    case PickupType.Speed:
                        playerBaseController.playerMovementController.IncrementMoveSpeed(pickupData.pickupValue, pickupData.lifeTime);
                        break;
                    case PickupType.Time:
                        playerBaseController.playerTimeController.AddTime((int)pickupData.pickupValue);
                        break;
                    case PickupType.Score:
                        playerBaseController.UpdateScore((int)pickupData.pickupValue);
                        break;
                }
            }
            //Debug.Log("Cannot pick this up");
        }

        // This function updates the UI on which items are in the user's hand
        void UpdateItemsInUI()
        {
            UIManager.Instance.UpdatePlayerItemsInHand((int)playerBaseController.playerData.PlayerID, itemsInHand);
        }

        // Function to store an item on a storage
        void StoreItem()
        {
            if (itemsInHand.Count > 0 && !currentInteractableGameObject.GetComponent<IStorage>().isStorageFull())
            {
                currentInteractableGameObject.GetComponent<IStorage>().Store(itemsInHand[0]);
                itemsInHand.RemoveAt(0);
                UpdateItemsInUI();
            }
        }

        // Function to serve an item to a customer
        void ServeItem(PlayerID playerID)
        {
            if (itemsInHand.Count > 0)
            {
                currentInteractableGameObject.GetComponent<IServable>().Serve(itemsInHand[0],playerID);
                itemsInHand.RemoveAt(0);
                UpdateItemsInUI();
            }
        }

        // Function to dispose the item in the user's hand
        void DisposeItem()
        {
            if (itemsInHand.Count > 0)
            {
                currentInteractableGameObject.GetComponent<IDisposer>().Dispose(itemsInHand[0]);
                itemsInHand.RemoveAt(0);
                UpdateItemsInUI();
            }
        }
        
        // Function called when chop beings
        void ChopActionBegin()
        {
            if (playerBaseController != null)
            {
                //Player cannot move while chopping
                playerBaseController.playerMovementController.DisablePlayerMovement = true;
                
                //Player cant do any other interactions while chopping
                DisablePlayerInteractions = true;

                // Start the chop action and pass callback
                currentInteractableGameObject.GetComponent<IChop>().Chop(ChopActionCompleted);

                //Activate the chopping text on the user
                playerChoppingText.SetActive(true);
            }
        }

        void ChopActionCompleted()
        {
            if (playerBaseController != null)
            {
                playerBaseController.playerMovementController.DisablePlayerMovement = false;
                DisablePlayerInteractions = false;
                playerChoppingText.SetActive(false);
            }
        }

        //This Function combines items on the chopping board
        void CombineItems()
        {
            currentInteractableGameObject.GetComponent<ICombine>().Combine();
        }
        #endregion

        #region InputPolling
        void Update()
        {
            // If interaction is not disabled
            if (!DisablePlayerInteractions)
            {
                // This section checks which player is providing input 
                //* Note: This can be improved to poll input independent of user ID (Update this later)
                switch (playerBaseController.playerData.PlayerID)
                {
                    case Enums.PlayerID.First:
                        if (Input.GetButtonDown("FirstPlayerStore"))
                        {
                            // Check if current interactable has the store interaction
                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Store))
                                StoreItem();

                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Dispose))
                                DisposeItem();

                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Serve))
                                ServeItem(PlayerID.First);
                        }
                        if (Input.GetButtonDown("FirstPlayerRetrieve"))
                        {
                            // Check if current interactable has the retrieve interaction
                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Retrieve))
                                RetrieveItem();

                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Pickup))
                                PickupItem();
                        }
                        if (Input.GetButtonDown("FirstPlayerChop"))
                        {
                            // Check if current interactable has the chop interaction
                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Chop))
                                ChopActionBegin();
                        }

                        else if (Input.GetButtonDown("FirstPlayerCombine"))
                        {
                            // Check if current interactable has the combine interaction
                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Combine))
                                CombineItems();
                        }
                        break;

                    case Enums.PlayerID.Second:
                        if (Input.GetButtonDown("SecondPlayerStore"))
                        {
                            // Check if current interactable has the store interaction
                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Store))
                                StoreItem();

                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Dispose))
                                DisposeItem();

                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Serve))
                                ServeItem(PlayerID.First);
                        }
                        if (Input.GetButtonDown("SecondPlayerRetrieve"))
                        {
                            // Check if current interactable has the retrieve interaction
                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Retrieve))
                                RetrieveItem();

                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Pickup))
                                PickupItem();
                        }
                        if (Input.GetButtonDown("SecondPlayerChop"))
                        {
                            // Check if current interactable has the chop interaction
                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Chop))
                                ChopActionBegin();
                        }

                        else if (Input.GetButtonDown("SecondPlayerCombine"))
                        {
                            // Check if current interactable has the combine interaction
                            if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Combine))
                                CombineItems();
                        }
                        break;
                }
            }
        }
        #endregion
    }
}
