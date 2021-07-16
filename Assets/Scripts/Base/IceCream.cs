using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCream
{
    public IceCreamBase Base;
    public List<IceCreamFlavour> Flavours;
    public IceCreamTopping Topping;
}

public enum IceCreamBase{CUP = 0, CONE = 1};
public enum IceCreamFlavour{YELLOW = 0, BROWN = 1, PINK = 2, BLUE = 3};
public enum IceCreamTopping{NONE = 0, CHOCOLATE_CHIPS = 1, RAINBOW_SPRINKLES = 2, GUMMY_BEARS = 3};
