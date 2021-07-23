using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;
using Random = UnityEngine.Random;

public class ClientsManager : MonoBehaviour, IClient
{
    private static ClientsManager _instance;
    public static ClientsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<ClientsManager>();
            }

            return _instance;
        }
    }

    public int ActiveClientsCount = 5;
    public GameObject ClientPrefab;

    public Transform[] ClientsLinePath;

    [Space]
    public TextAsset Json;
    public List<Client> Clients;
    public List<Client> ActiveClients = new List<Client>();

    public Client FrontClient => ActiveClients[0];

    private bool _isGameAlive = true;

    private void Start()
    {
        LoadClients(Json.text);

        int clientsAdded = 0;
        while (clientsAdded < ActiveClientsCount)
        {
            if (!TryAddNewClient()) break;
            clientsAdded++;
        }
    }

    private bool TryAddNewClient()
    {
        if (Clients.Count == 0)
        {
            _isGameAlive = false; //out of clients
            return false;
        }
        var client = Clients[0];
        Clients.RemoveAt(0);
        ActiveClients.Add(client);
        client.Initialize();
        return true;
    }

    public void LoadClients(string json)
    {
        var clientsList = JsonUtility.FromJson<ClientsList>(json);
        Clients = new List<Client>(clientsList.Clients);
    }

    public bool TryReceive(IceCream iceCream)
    {
        if (!_isGameAlive) return false;
        if (this.FrontClient.Order.Compare(iceCream))
        {
            ActiveClients.RemoveAt(0);
            TryAddNewClient();
            Debug.Log("Ice cream accepted!");
            return true;
        }
        else
        {
            Debug.Log("Ice cream rejected!");
            return false;
        }
    }

    [Button]
    public void TestTryReceive()
    {
        if (!Application.isPlaying) return;
        bool same = Random.value < 0.5f;
        bool ok = false;
        IceCream offeredIceCream = new IceCream(FrontClient.Order);

        if (!same)
        {
            int count = Enum.GetValues(typeof(IceCreamTopping)).Length;
            offeredIceCream.Topping = (IceCreamTopping)(((int)(offeredIceCream.Topping + 1)) % count);
        }
        ok = TryReceive(offeredIceCream);

        Debug.Log($"Offered {(same ? "an equal" : "a different")} ice cream and client {(ok ? "accepted it" : "rejected it")}, now total clients count is {(ActiveClients.Count + Clients.Count)}");
    }

    private void Update()
    {
        if (!_isGameAlive) return;
        for (int i = 0; i < ActiveClients.Count; i++)
        {
            Client activeClient = ActiveClients[i];
            if (activeClient.Timer <= 0)
            {
                ActiveClients.Remove(activeClient);
                break;
            }
        }
    }

    private void OnDrawGizmos() {
        for (int i = 1; i < this.ClientsLinePath.Length; i++)
        {
            Gizmos.DrawLine(this.ClientsLinePath[i].transform.position, this.ClientsLinePath[i-1].transform.position);
            
        }
    }
}
