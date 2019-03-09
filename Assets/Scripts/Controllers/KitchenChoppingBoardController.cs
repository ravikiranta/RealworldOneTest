using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInterfaces;
using Enums;
using System;
using Managers;

namespace Controllers
{
    public class KitchenChoppingBoardController : MonoBehaviour,IInteractable, IStorage, IChop, IRetrieve, ICombine
    {
        #region Variables
        [Header ("Dev Settings")]
        [SerializeField] private int maxStorageCount;
        [SerializeField] private int maxChoppedItemsOnBoardCount;
        [SerializeField] private string interactionControlsMessage;
        [SerializeField] private List<KitchenInteractions> possibleInteractions;
        [SerializeField] [Range(1,5)] private float chopTime;

        [Header("References")]
        [SerializeField] private UITextUpdateController uiTextUpdateController;

        [Header ("Info")]
        [SerializeField] private List<string> itemsInStorage;
        [SerializeField] private List<string> processedItems;
        
        [SerializeField] private float chopTimer;
        #endregion

        #region InterfaceImplementations
        string IRetrieve.Retrieve()
        {
            if (processedItems.Count > 0)
            {
                string item = processedItems[processedItems.Count - 1];
                processedItems.RemoveAt(processedItems.Count - 1);
                UpdateUI();
                return item;
            }
            else if (itemsInStorage.Count > 0)
            {
                string item = itemsInStorage[itemsInStorage.Count - 1];
                itemsInStorage.RemoveAt(itemsInStorage.Count - 1);
                UpdateUI();
                return item;
            }
            else
            {
                return null;
            }
        }

        void IStorage.Store(string item)
        {
            if (itemsInStorage.Count < maxStorageCount)
            {
                itemsInStorage.Add(item);
            }
            UpdateUI();
        }

        bool IStorage.isStorageFull()
        {
            if (itemsInStorage.Count < maxStorageCount)
                return false;
            else
                return true;
        }

        string IInteractable.GetInteractionControls()
        {
            return interactionControlsMessage;
        }

        List<KitchenInteractions> IInteractable.GetPossibleInteractions()
        {
            return possibleInteractions;
        }

        void IChop.Chop(Action callback)
        {
            if (itemsInStorage.Count > 0)
            {
                // Check if there is a result item for this interaction on this item
                string item = GameManager.Instance.
                    ReturnResultItemOnKitchenActionOnItem(itemsInStorage[0], KitchenInteractions.Chop);
                
                //If result item exists, start the shop coroutine and pass the result item to be added on completion
                if (item != null)
                    StartCoroutine(ChopTimer(item, callback));
                else
                    Debug.Log("This item does not have this interaction");
            }
            else
                Debug.Log("No items in storage");
        }

        void ICombine.Combine()
        {

            if (itemsInStorage.Count < maxStorageCount)
            {
                string food = "";

                // Add the processed items
                for (int i = 0; i < processedItems.Count; i++)
                {
                    food += processedItems[i];
                    if (i < processedItems.Count - 1)
                        food += ", ";
                }

                // Add anything else that is on the table
                for (int i = 0; i < itemsInStorage.Count; i++)
                {
                    food += itemsInStorage[i];
                    if (i < processedItems.Count - 1)
                        food += ", ";
                }

                processedItems.Clear();
                itemsInStorage.Clear();

                itemsInStorage.Add(food);

                UpdateUI();
            }
            else
            {
                Debug.Log("Clear atleast one slot on the chopping board");
            }
        }
        #endregion

        #region ChopFunction
        IEnumerator ChopTimer(string resultItem, Action callback)
        {
            while(chopTimer <= chopTime)
            {
                chopTimer++;
                yield return new WaitForSeconds(1f);
            }
            chopTimer = 0;
            itemsInStorage.RemoveAt(0);
            processedItems.Add(resultItem);
            callback();

            UpdateUI();
        }
        #endregion

        #region UIUpdate
        void UpdateUI()
        {
            string text = "";

            for (int i = 0; i < itemsInStorage.Count; i++) {
                text += itemsInStorage[i];
                text += "\n";
            }

            text += "\n";

            for (int i = 0; i < processedItems.Count; i++)
            {
                text += processedItems[i];
                text += "\n ";
            }

            uiTextUpdateController.UpdateText(text);
        }
        #endregion
    }
}