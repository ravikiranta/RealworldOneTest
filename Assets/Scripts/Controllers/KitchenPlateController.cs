using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInterfaces;
using Enums;

namespace Controllers {
    public class KitchenPlateController : MonoBehaviour,IInteractable, IStorage, IRetrieve
    {
        #region Variables
        [Header ("Dev Settings")]
        [SerializeField] private int maxStorageCount;
        [SerializeField] private string interactionControlsMessage;
        [SerializeField] private List<KitchenInteractions> possibleInteractions;

        [Header("Info")]
        [SerializeField] private List<string> itemsInStorage;

        [Header("References")]
        [SerializeField] private UITextUpdateController uiTextUpdateController;
        #endregion

        #region Functions
        string IRetrieve.Retrieve()
        {
            if (itemsInStorage.Count > 0)
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
            if(itemsInStorage.Count < maxStorageCount)
            {
                itemsInStorage.Add(item);
            }

            UpdateUI();
        }

        string IInteractable.GetInteractionControls()
        {
            return interactionControlsMessage;
        }

        List<KitchenInteractions> IInteractable.GetPossibleInteractions()
        {
            return possibleInteractions;
        }

        bool IStorage.isStorageFull()
        {
            if (itemsInStorage.Count < maxStorageCount)
                return false;
            else
                return true;
        }
        #endregion

        #region UIUpdate
        void UpdateUI()
        {
            string text = "";

            for (int i = 0; i < itemsInStorage.Count; i++)
            {
                text += itemsInStorage[i];
            }
            uiTextUpdateController.UpdateText(text);
        }
        #endregion
    }
}
