using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PlayerManager>();
            }

            return _instance;
        }
    }

    public int MaxFlavours = 3;
    public GameObject[] PrefabsBase;
    public GameObject[] PrefabsFlavours;
    public GameObject[] PrefabsToppings;
    public GameObject[] PrefabsRandomOrders;
    public GameObject FishPrefab;
    public Transform Hand;
    public Interactable[] InteractablesBase;
    public Interactable[] InteractableFlavours;
    public Interactable[] InteractableToppings;
    public Interactable TrashBin;
    public Interactable Client;
    public Interactable Fish;
    public Sprite CounterWithoutFish;
    public Sprite CounterWithFish;
    public SpriteRenderer Counter;
    private IceCream _currentIceCream;
    private bool _emptyHand;
    private bool _fishInHand;
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

        TrashBin.OnInteract += TrashIceCream;

        Client.OnInteract += GiveIceCreamToClient;

        Fish.OnInteract += () => AddFish();
    }

    private void GiveIceCreamToClient()
    {
        if (_fishInHand)
        {
            Client.GetComponent<IClient>().Punch();
        }
        else
        {
            bool accepted = Client.GetComponent<IClient>().TryReceive(_currentIceCream);
            if (accepted)
            {
                TrashIceCream();
            }
        }
    }

    private void Update()
    {
        Hand.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Hand.Translate(0, 0, 9);
    }

    private void TrashIceCream()
    {
        if (!_fishInHand)
        {
            TrashInternal();
        }
    }

    private void TrashInternal()
    {
        _emptyHand = true;
        _currentIceCream.Flavours.Clear();
        _currentIceCream.Topping = IceCreamTopping.NONE;
        foreach (var o in _objectsOnHand)
        {
            Destroy(o);
        }
        _objectsOnHand.Clear();
    }

    private void AddBase(IceCreamBase type)
    {
        if (_emptyHand && !_fishInHand)
        {
            _currentIceCream.Base = type;
            _emptyHand = false;
            AddObjectToHand(PrefabsBase[(int)type]);
        }
    }

    private void AddFish()
    {
        if (_emptyHand)
        {
            if (_fishInHand)
            {
                Counter.sprite = CounterWithFish;
                TrashInternal();
                _fishInHand = false;
            }
            else
            {
                Counter.sprite = CounterWithoutFish;
                AddObjectToHand(FishPrefab);
                _fishInHand = true;
            }
        }
    }


    private void AddFlavour(IceCreamFlavour flavour)
    {
        if (!_emptyHand && !_fishInHand && _currentIceCream.Flavours.Count < MaxFlavours)
        {
            _currentIceCream.Flavours.Add(flavour);
            AddObjectToHand(PrefabsFlavours[(int)flavour]);
        }
    }

    private void AddTopping(IceCreamTopping topping)
    {
        if (!_fishInHand && !_emptyHand && _currentIceCream.Flavours.Count > 0 && _currentIceCream.Topping == 0)
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
        if (go.name.StartsWith("Topping"))
        {
            baseOffset -= 0.3f;
        }
        b.localPosition = new Vector3(0, baseOffset + _objectsOnHand.Count * 0.5f, -_objectsOnHand.Count * 0.01f);
    }
}
