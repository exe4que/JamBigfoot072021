using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int MaxFlavours = 3;
    public GameObject[] PrefabsBase;
    public GameObject[] PrefabsFlavours;
    public GameObject[] PrefabsToppings;
    public Transform Hand;
    public Interactable[] InteractablesBase;
    public Interactable[] InteractableFlavours;
    public Interactable[] InteractableToppings;
    public Interactable TrasBin;
    public Interactable Client;
    private IceCream _currentIceCream;
    private bool _emptyHand;
    private List<GameObject> _objectsOnHand;


    private void Start()
    {
        _emptyHand = true;
        _currentIceCream = new IceCream();
        _currentIceCream.Flavours = new List<IceCreamFlavour>();
        _objectsOnHand = new List<GameObject>();
        
        InteractablesBase[0].OnInteract += () => AddBase(IceCreamBase.CUP);
        InteractablesBase[1].OnInteract += () => AddBase(IceCreamBase.CONE);

        InteractableFlavours[0].OnInteract += () => AddFlavour((IceCreamFlavour)0);
        InteractableFlavours[1].OnInteract += () => AddFlavour((IceCreamFlavour)1);
        InteractableFlavours[2].OnInteract += () => AddFlavour((IceCreamFlavour)2);
        InteractableFlavours[3].OnInteract += () => AddFlavour((IceCreamFlavour)3);

        InteractableToppings[0].OnInteract += () => AddTopping((IceCreamTopping)1);
        InteractableToppings[1].OnInteract += () => AddTopping((IceCreamTopping)2);
        InteractableToppings[2].OnInteract += () => AddTopping((IceCreamTopping)3);

        TrasBin.OnInteract += TrashIceCream;

        Client.OnInteract += GiveIceCreamToClient;
    }

    private void GiveIceCreamToClient()
    {
        bool accepted = Client.GetComponent<IClient>().TryReceive(_currentIceCream);
        if(accepted)
        {
            TrashIceCream();
        }
    }

    private void Update()
    {
        Hand.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Hand.Translate(0, 0, 9);
    }

    private void TrashIceCream()
    {
        _emptyHand = true;
        _currentIceCream.Flavours.Clear();
        foreach (var o in _objectsOnHand)
        {
            Destroy(o);
        }
        _objectsOnHand.Clear();
    }

    private void AddBase(IceCreamBase type)
    {
        if (_emptyHand)
        {
            _currentIceCream.Base = type;
            _emptyHand = false;
            AddObjectToHand(PrefabsBase[(int)type]);
        }
    }

    private void AddFlavour(IceCreamFlavour flavour)
    {
        if (!_emptyHand && _currentIceCream.Flavours.Count < MaxFlavours)
        {
            _currentIceCream.Flavours.Add(flavour);
            AddObjectToHand(PrefabsFlavours[(int)flavour]);
        }
    }

    private void AddTopping(IceCreamTopping topping)
    {
        if (!_emptyHand && _currentIceCream.Flavours.Count > 0 && _currentIceCream.Topping == 0)
        {
            _currentIceCream.Topping = topping;
            AddObjectToHand(PrefabsToppings[(int)topping - 1]);
        }
    }

    private void AddObjectToHand(GameObject go)
    {
        Transform b = Instantiate(go, Hand).transform;
        _objectsOnHand.Add(b.gameObject);

        float baseOffset = (_currentIceCream.Base == IceCreamBase.CONE && _objectsOnHand.Count > 1) ? 0.35f : 0f;
        b.localPosition = new Vector3(0, baseOffset + _objectsOnHand.Count * 0.5f, 0);
    }
}
