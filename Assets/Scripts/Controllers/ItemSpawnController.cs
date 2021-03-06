﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using GameInterfaces;
using Enums;

namespace Controllers {
    public class ItemSpawnController : MonoBehaviour, IInteractable, IRetrieve
    {
        #region Variables
        [Header("Dev Settings")]
        [SerializeField] private string spawnItem;
        [SerializeField] private string interactionControlsMessage;
        [SerializeField] private List<KitchenInteractions> possibleInteractions;
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

        string IRetrieve.Retrieve()
        {
            return spawnItem;
        }
        #endregion
    }
}