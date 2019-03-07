using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInterfaces;

namespace Controllers
{
    public class KitchenChoppingBoardController : MonoBehaviour, IStorage
    {
        #region Variables
        [SerializeField] private int maxStorageCount;
        [SerializeField] private List<string> itemsInStorage;
        #endregion

        #region Functions
        List<string> IStorage.Retrieve()
        {
            List<string> items = itemsInStorage;
            itemsInStorage.Clear();
            return itemsInStorage;
        }

        void IStorage.Store(List<string> items)
        {
            if (itemsInStorage.Count < maxStorageCount)
            {
                itemsInStorage.AddRange(items);
            }
        }
        #endregion
    }
}