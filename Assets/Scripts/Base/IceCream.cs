using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IceCream
{
    public IceCreamBase Base;
    public List<IceCreamFlavour> Flavours;
    public IceCreamTopping Topping;

    public IceCream()
    {

    }

    public IceCream(IceCream iceCreamToCopy)
    {
        this.Base = iceCreamToCopy.Base;
        this.Flavours = new List<IceCreamFlavour>();
        for (int i = 0; i < this.Flavours.Count; i++)
        {
            this.Flavours[i] = iceCreamToCopy.Flavours[i];
        }
        this.Topping = iceCreamToCopy.Topping;
    }

    public bool Compare(IceCream iceCreamToCompare)
    {
        Debug.Log($"base: {Base} == compared base {iceCreamToCompare.Base}");
        if(this.Base != iceCreamToCompare.Base) return false;
        Debug.Log($"flavour count: {Flavours.Count} == compared lavour count {iceCreamToCompare.Flavours.Count}");
        if(this.Flavours.Count != iceCreamToCompare.Flavours.Count) return false;
        for (int i = 0; i < this.Flavours.Count; i++)
        {
            Debug.Log($"flavour: {Flavours[i]} == compared flavour {iceCreamToCompare.Flavours[i]}");
            if(this.Flavours[i] != iceCreamToCompare.Flavours[i]) return false;
        }
        Debug.Log($"topping: {Topping} == compared topping {iceCreamToCompare.Topping}");
        if(this.Topping != iceCreamToCompare.Topping) return false;
        return true;
    }

    
}

public enum IceCreamBase{CUP = 0, CONE = 1};
public enum IceCreamFlavour{YELLOW = 0, BROWN = 1, PINK = 2, BLUE = 3};
public enum IceCreamTopping{NONE = 0, CHOCOLATE_CHIPS = 1, RAINBOW_SPRINKLES = 2, GUMMY_BEARS = 3};
