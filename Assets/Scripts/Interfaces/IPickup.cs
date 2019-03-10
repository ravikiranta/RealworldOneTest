using System;
using Data;
using Enums;

namespace GameInterfaces
{
    public interface IPickup
    {
        void PickupItem(Action<PickupData> pickedUpCallback, PlayerID playerID);
    }
}
