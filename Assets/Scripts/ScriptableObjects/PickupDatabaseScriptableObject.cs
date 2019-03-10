using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "CreateItem/PickupDatabase", order = 1)]
    public class PickupDatabaseScriptableObject : ScriptableObject
    {
        public List<PickupData> pickups;
    }
}
