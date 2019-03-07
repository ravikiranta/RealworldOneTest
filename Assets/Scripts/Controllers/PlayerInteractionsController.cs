using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInterfaces;
using Managers;
using Data;

namespace Controllers {
    [RequireComponent(typeof(PlayerData))]
    public class PlayerInteractionsController : MonoBehaviour
    {
        #region Variables
        [Header("Dev Settings")]
        [SerializeField] private int itemInventoryLimit;
        [SerializeField] private List<string> itemsInHand;
        [SerializeField] private UserInteractionPanelController userInteractionPanelController;
        private IInteractable currentInteractable;
        private PlayerData playerData;
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
                UIManager.Instance.UpdatePlayerInteractionMessages((int)playerData.PlayerID, "");
            }
            else
            {
                Debug.Log("Entered another interactable");
            }
        }

        void PickupItem(GameInterfaces.IPickable item)
        {
            if (itemsInHand.Count < itemInventoryLimit)
            {
                itemsInHand.Add(item.GetItemName());
                item.Destroy();

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
    }
}
