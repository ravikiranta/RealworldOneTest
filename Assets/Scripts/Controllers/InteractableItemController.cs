using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameInterfaces;
using Data;

namespace Controllers
{
    public class InteractableItemController : MonoBehaviour, GameInterfaces.IPickable
    {
        public void Destroy()
        {
            Destroy(this.gameObject);
        }

        public string GetItemName()
        {
            return this.name;
        }
    }
}
