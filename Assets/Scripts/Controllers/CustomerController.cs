using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Data;

namespace Controllers
{
    public class CustomerController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private FoodData foodOrdered;
        #endregion

        #region Init
        void OnEnable()
        {
            foodOrdered = GameManager.Instance.ReturnRandomFoodSuggestionForCustomer();
        }
        #endregion
    }
}
