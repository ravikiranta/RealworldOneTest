using System.Collections.Generic;
using UnityEngine;
using GameInterfaces;
using Enums;
using Data;
using System;

namespace Controllers
{
    public class PickupController : MonoBehaviour, IInteractable, IPickup
    {
        #region Variables
        [Header("Dev Set Values")]
        [SerializeField] private List<KitchenInteractions> possibleInteractions;
        [SerializeField] private string interactionControlsMessage;

        [Header ("Info")]
        [SerializeField] private PickupData pickupData;
        [SerializeField] private PlayerID pickupForPlayer;
        #endregion

        #region Init
        public void Init(PickupData pickupDataInit)
        {
            pickupData = pickupDataInit;
            pickupData.pickupModel = gameObject;
        }
        #endregion

        #region InterfaceImplementations
        string IInteractable.GetInteractionControls()
        {
            return interactionControlsMessage;
        }

        List<KitchenInteractions> IInteractable.GetPossibleInteractions()
        {
            return possibleInteractions;
        }

        void IPickup.PickupItem(Action<PickupData> pickedUpCallback, PlayerID playerID)
        {
            if(playerID == pickupForPlayer)
                pickedUpCallback(pickupData);
            else
                pickedUpCallback(null);

            Destroy(gameObject);
        }
        #endregion
    }
}
