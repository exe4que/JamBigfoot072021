using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClient
{
    bool TryReceive(IceCream iceCream);
}
