using System.Collections.Generic;
using Enums;

namespace GameInterfaces
{
    public interface IServable
    {
        void Serve(string food, PlayerID playerID);
    }
}
