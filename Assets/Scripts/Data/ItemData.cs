using UnityEngine;
using Enums;
using System.Collections.Generic;

namespace Data
{
    [System.Serializable]
    public class ItemData
    {
        public string itemName;
        public List<KitchenInteractions> possibleKitchenInteractions;
        public List<string> resultItems;
    }
}
