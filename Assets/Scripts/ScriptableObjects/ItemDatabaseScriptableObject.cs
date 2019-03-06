using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "CreateItem/ItemDatabase", order = 1)]
    public class ItemDatabaseScriptableObject : ScriptableObject
    {
        public List<ItemData> items;
    }
}
