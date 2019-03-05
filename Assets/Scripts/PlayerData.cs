using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

namespace Data
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private PlayerID playerID;

        #region Properties
        public PlayerID PlayerID{
            get {
                return playerID;
            }
            set
            {
                playerID = value;
            }
        }   
        #endregion
    }
}
