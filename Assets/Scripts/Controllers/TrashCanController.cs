using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInterfaces;
using Enums;

namespace Controllers
{
    //This class controls the behaviour of the trash can
    public class TrashCanController : MonoBehaviour,IInteractable, IDisposer
    {
        #region Variables
        [SerializeField] private string interactionControlsMessage;
        [SerializeField] private List<KitchenInteractions> possibleInteractions;
        #endregion

        #region InterfaceImplementations
        public void Dispose(string item)
        {
            //Debug.Log("Item Disposed");
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
