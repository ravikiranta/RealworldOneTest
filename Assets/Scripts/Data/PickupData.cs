using Enums;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class PickupData
    {
        public PickupType pickupType;
        public GameObject pickupModel;
        public float pickupValue;
        public float lifeTime;

        public PickupData(PickupType pickupType,GameObject pickupModel, float pickupValue, float lifeTime)
        {
            this.pickupType = pickupType;
            this.pickupValue = pickupValue;
            this.pickupModel = pickupModel;
            this.lifeTime = lifeTime;
        }
    }
}
