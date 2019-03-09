using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Controllers;
using System;

namespace GameInterfaces
{
    public interface IChop
    {
        void Chop(Action callback);
    }
}
