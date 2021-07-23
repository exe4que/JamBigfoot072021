using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int MaxFlavours = 3;
    public GameObject[] PrefabsBase;
    public GameObject[] PrefabsFlavours;
    public Transform Hand;
    public Interactable[] InteractablesBase;
    public Interactable[] InteractablesFlavour;
    public Interactable TrasBin;
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

        InteractablesFlavour[0].OnInteract += () => AddFlavour((IceCreamFlavour)0);
        InteractablesFlavour[1].OnInteract += () => AddFlavour((IceCreamFlavour)1);
        InteractablesFlavour[2].OnInteract += () => AddFlavour((IceCreamFlavour)2);
        InteractablesFlavour[3].OnInteract += () => AddFlavour((IceCreamFlavour)3);

        TrasBin.OnInteract += TrashIceCream;
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

    private void AddObjectToHand(GameObject go)
    {
        Transform b = Instantiate(go, Hand).transform;
        _objectsOnHand.Add(b.gameObject);
        b.localPosition = new Vector3(0, _objectsOnHand.Count * 0.5f, 0);
    }
}
