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
    public class PlayerInteractionsController : MonoBehaviour
    {
        #region Variables
        [Header("Dev Settings")]
        [SerializeField] private int itemInventoryLimit;

        [Header("Info")]
        [SerializeField] private List<string> itemsInHand;
        [SerializeField] private GameObject currentInteractableGameObject;
        [SerializeField] private IInteractable currentInteractable;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private List<KitchenInteractions> currentPossibleInteractions;
        [SerializeField] private bool disablePlayerInteractions;

        [Header("References")]
        [SerializeField] private PlayerMovementController playerMovementController;
        [SerializeField] private GameObject playerChoppingText;
        #endregion

        #region Init
        void Awake()
        {
            playerData = GetComponent<PlayerData>();
            playerMovementController = GetComponent<PlayerMovementController>();
        }
        #endregion

        #region Event
        public delegate void ActionCompleteCallback();
        ActionCompleteCallback ChopCompleteCallback;
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
                    UIManager.Instance.UpdatePlayerInteractionMessages(
                        (int)playerData.PlayerID, currentInteractable.GetInteractionControls());
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
                UIManager.Instance.UpdatePlayerInteractionMessages((int)playerData.PlayerID, "");
            }
            else
            {
                //Debug.Log("Entered another interactable");
            }
        }
        #endregion

        #region PlayerFunctions
        void PickupItem()
        {
            if (itemsInHand.Count < itemInventoryLimit)
            {
                itemsInHand.Add(currentInteractableGameObject.GetComponent<IRetrieve>().Retrieve());
                UpdateItemsInUI();
            }
        }

        void UpdateItemsInUI()
        {
            UIManager.Instance.UpdatePlayerItemsInHand((int)playerData.PlayerID, itemsInHand);
        }

        void StoreItem()
        {
            if (itemsInHand.Count > 0 && !currentInteractableGameObject.GetComponent<IStorage>().isStorageFull())
            {
                currentInteractableGameObject.GetComponent<IStorage>().Store(itemsInHand[0]);
                itemsInHand.RemoveAt(0);
                UpdateItemsInUI();
            }
        }

        void ServeItem(PlayerID playerID)
        {
            if (itemsInHand.Count > 0)
            {
                currentInteractableGameObject.GetComponent<IServable>().Serve(itemsInHand[0],playerID);
                itemsInHand.RemoveAt(0);
                UpdateItemsInUI();
            }
        }

        void DisposeItem()
        {
            if (itemsInHand.Count > 0)
            {
                currentInteractableGameObject.GetComponent<IDisposer>().Dispose(itemsInHand[0]);
                itemsInHand.RemoveAt(0);
                UpdateItemsInUI();
            }
        }
        
        void ChopActionBegin()
        {
            if (playerMovementController != null)
            {
                playerMovementController.DisablePlayerMovement = true;
                disablePlayerInteractions = true;
                currentInteractableGameObject.GetComponent<IChop>().Chop(ChopActionCompleted);
                playerChoppingText.SetActive(true);
            }
        }

        void ChopActionCompleted()
        {
            if (playerMovementController != null)
            {
                playerMovementController.DisablePlayerMovement = false;
                disablePlayerInteractions = false;
                playerChoppingText.SetActive(false);
            }
        }

        void CombineItems()
        {
            currentInteractableGameObject.GetComponent<ICombine>().Combine();
        }
        #endregion

        #region InputPolling
        void Update()
        {
            if (!disablePlayerInteractions)
            {
                switch (playerData.PlayerID)
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

                        break;
                }
            }
        }
        #endregion
    }
}
