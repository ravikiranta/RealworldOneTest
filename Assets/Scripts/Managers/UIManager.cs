using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        #region Variables
        [SerializeField] private List<UserInteractionPanelController> userInteractionPanelControllers;
        #endregion

        #region Singleton
        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<UIManager>();

                return instance;
            }
        }
        #endregion

        #region UIFunctions
        public void UpdatePlayerItemsInHand(int playerID, List<string> items)
        {
            userInteractionPanelControllers[playerID].Items = items;
        }

        public void UpdatePlayerInteractionMessages(int playerID, string interactionMesage)
        {
            userInteractionPanelControllers[playerID].UserInteractionText = interactionMesage;
        }

        public void UpdatePlayerScore(int playerID, int score)
        {
            userInteractionPanelControllers[playerID].UpdateScore(score);
        }

        public void UpdatePlayerTime(int playerID, int time)
        {
            userInteractionPanelControllers[playerID].UpdateTime(time);
        }
        #endregion
    }
}
