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
        [SerializeField] private List<string> itemsInHand;
        [SerializeField] private GameObject currentInteractableGameObject;
        [SerializeField] private IInteractable currentInteractable;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private PlayerMovementController playerMovementController;
        [SerializeField] private List<KitchenInteractions> currentPossibleInteractions;
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
        void PickupItem(string item)
        {
            if (itemsInHand.Count < itemInventoryLimit)
            {
                itemsInHand.Add(item);
                UpdateItemsInUI();
            }
        }

        void UpdateItemsInUI()
        {
            UIManager.Instance.UpdatePlayerItemsInHand((int)playerData.PlayerID, itemsInHand);
        }

        void DropItem()
        {
        }
        #endregion

        #region ActionUpdates
        void ChopActionBegin()
        {
            if (playerMovementController != null)
                playerMovementController.DisablePlayerMovement = true;
        }

        void ChopActionCompleted()
        {
            Debug.Log("Chop Completed");
            if(playerMovementController != null)
                playerMovementController.DisablePlayerMovement = false;
        }
        #endregion

        #region InputPolling
        void Update()
        {
            switch (playerData.PlayerID)
            {
                case Enums.PlayerID.First:
                    if (Input.GetButtonDown("FirstPlayerStore"))
                    {
                        // Check if current interactable has the store interaction
                        if(currentPossibleInteractions.Exists(x=> x == KitchenInteractions.Store) && itemsInHand.Count > 0){
                            currentInteractableGameObject.GetComponent<IStorage>().Store(itemsInHand[itemsInHand.Count -1]);
                            itemsInHand.RemoveAt(itemsInHand.Count - 1);
                        }
                    }
                    if (Input.GetButtonDown("FirstPlayerRetrieve"))
                    {
                        // Check if current interactable has the retrieve interaction
                        if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Retrieve)) {
                            PickupItem(currentInteractableGameObject.GetComponent<IStorage>().Retrieve());
                        }
                    }
                    if (Input.GetButtonDown("FirstPlayerChop"))
                    {
                        // Check if current interactable has the retrieve interaction
                        if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Chop))
                        {
                            ChopActionBegin();
                            currentInteractableGameObject.GetComponent<IChop>().Chop(ChopActionCompleted);
                        }
                    }

                    else if (Input.GetButtonDown("FirstPlayerCombine"))
                    {
                        // Check if current interactable has the combine interaction
                        if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Combine))
                        {

                        }
                    }
                    break;

                case Enums.PlayerID.Second:
                    
                    break;
            }
            
        }
        #endregion
    }
}
