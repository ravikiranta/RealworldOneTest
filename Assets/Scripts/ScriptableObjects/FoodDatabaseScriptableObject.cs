using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "CreateItem/FoodDatabase", order = 1)]
    public class FoodDatabaseScriptableObject : ScriptableObject
    {
        public List<FoodData> foodItems;
        public FoodDatabaseScriptableObject()
        {
            foodItems = null;
        }
    }
}
