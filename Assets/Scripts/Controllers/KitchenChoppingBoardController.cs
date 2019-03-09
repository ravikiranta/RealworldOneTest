using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInterfaces;
using Enums;

namespace Controllers
{
    public class KitchenChoppingBoardController : MonoBehaviour,IInteractable, IStorage
    {
        #region Variables
        [SerializeField] private int maxStorageCount;
        [SerializeField] private List<string> itemsInStorage;
        [SerializeField] private string interactionControlsMessage;
        [SerializeField] private List<KitchenInteractions> possibleInteractions;
        #endregion

        #region Functions
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
        #endregion
    }
}