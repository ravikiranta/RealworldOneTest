using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "CreateItem/VegetableDatabase", order = 1)]
    public class VegetableDatabaseScriptableObject : ScriptableObject
    {
        public List<string> vegetables;
    }
}
