using System.Collections.Generic;
using Enums;

namespace GameInterfaces
{
    public interface IInteractable
    {
        string GetInteractionControls();
        List<KitchenInteractions> GetPossibleInteractions();
    }
}
