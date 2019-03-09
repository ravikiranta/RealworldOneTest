using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInterfaces;
using Managers;
using Data;
using Enums;

namespace Controllers {
    [RequireComponent(typeof(PlayerData))]
    public class PlayerInteractionsController : MonoBehaviour
    {
        #region Variables
        [Header("Dev Settings")]
        [SerializeField] private int itemInventoryLimit;
        [SerializeField] private List<string> itemsInHand;
        private GameObject currentInteractableGameObject;
        private IInteractable currentInteractable;
        private PlayerData playerData;
        private List<KitchenInteractions> currentPossibleInteractions;
        #endregion

        #region Init
        void Awake()
        {
            playerData = GetComponent<PlayerData>();
        }
        #endregion

        #region Functions
        void OnTriggerEnter(Collider objectCollider)
        {
            // Check if the items is interactable
            currentInteractable = objectCollider.GetComponent<GameInterfaces.IInteractable>();
            {
                if(currentInteractable != null)
                {
                    currentPossibleInteractions = currentInteractable.GetPossibleInteractions();
                    currentInteractableGameObject = objectCollider.gameObject;
                    UIManager.Instance.UpdatePlayerInteractionMessages(
                        (int)playerData.PlayerID, currentInteractable.GetInteractionControls());
                }
            }

            ////Debug.Log("Colliding:" + interactiveItem.name);
            //GameInterfaces.IPickable pickable = objectCollider.GetComponent<GameInterfaces.IPickable>();
            //if (pickable != null)
            //{
            //    //Debug.Log("Picked up item:" + interactable.GetItemName());
            //    PickupItem(pickable);
            //}
            //else
            //{
            //    Debug.Log("Not Pickable");
            //}

            //GameInterfaces.IStorage kitchenInteractableStorage = 
            //    objectCollider.GetComponent<GameInterfaces.IStorage>();

            //if (kitchenInteractableStorage != null)
            //{

            //}
            //else
            //{
            //    Debug.Log("Not Storable");
            //}
        }

        void OnTriggerExit(Collider exitObjectCollider)
        {
            if(currentInteractable == exitObjectCollider.GetComponent<GameInterfaces.IInteractable>())
            {
                Debug.Log("Exited existing interactable");
                // Clear possible interactions
                currentPossibleInteractions.Clear();
                currentInteractableGameObject = null;
                //Update UI interactions text
                UIManager.Instance.UpdatePlayerInteractionMessages((int)playerData.PlayerID, "");
            }
            else
            {
                Debug.Log("Entered another interactable");
            }
        }

        void PickupItem(string item)
        {
            if (itemsInHand.Count < itemInventoryLimit)
            {

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
                        Debug.Log("Retrieving");
                        // Check if current interactable has the retrieve interaction
                        if (currentPossibleInteractions.Exists(x => x == KitchenInteractions.Retrieve)) {
                            PickupItem(currentInteractableGameObject.GetComponent<IStorage>().Retrieve());
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
