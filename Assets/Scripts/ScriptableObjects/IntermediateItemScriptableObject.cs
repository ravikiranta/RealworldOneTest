using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "CreateItem/IntermediateItemDatabase", order = 1)]
    public class IntermediateItemScriptableObject : ScriptableObject
    {
        public List<ItemData> items;
    }
}
