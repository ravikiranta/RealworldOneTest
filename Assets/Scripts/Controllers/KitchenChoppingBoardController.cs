using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInterfaces;
using Enums;
using System;

namespace Controllers
{
    public class KitchenChoppingBoardController : MonoBehaviour,IInteractable, IStorage, IChop
    {
        #region Variables
        [Header ("Dev Settings")]
        [SerializeField] private int maxStorageCount;
        [SerializeField] private int maxChoppedItemsOnBoardCount;
        [SerializeField] private string interactionControlsMessage;
        [SerializeField] [Range(1,5)] private float chopTime;

        [Header ("Info")]
        [SerializeField] private List<string> itemsInStorage;
        [SerializeField] private List<string> processedItems;
        [SerializeField] private List<KitchenInteractions> possibleInteractions;
        [SerializeField] private float chopTimer;
        #endregion

        #region InterfaceImplementations
        string IStorage.Retrieve()
        {
            if (itemsInStorage.Count > 0)
            {
                string item = itemsInStorage[itemsInStorage.Count - 1];
                itemsInStorage.RemoveAt(itemsInStorage.Count - 1);
                return itemsInStorage[itemsInStorage.Count - 1];
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
                StartCoroutine(ChopTimer(itemsInStorage[0], callback));
            }
            else
                Debug.Log("No items in storage");
        }
        #endregion

        #region ChopFunction
        IEnumerator ChopTimer(string itemName, Action callback)
        {
            while(chopTimer <= chopTime)
            {
                chopTimer++;
                yield return new WaitForSeconds(1f);
            }
            chopTimer = 0;
            callback();
            //processedItems.Add()
        }
        #endregion
    }
}