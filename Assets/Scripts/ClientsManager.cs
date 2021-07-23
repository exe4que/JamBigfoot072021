using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientsManager : MonoBehaviour
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


    public TextAsset Json;
    [SerializeField]
    public List<Client> Clients;

    private void Start()
    {
        LoadClients(Json.text);
    }

    public void LoadClients(string json)
    {
        var clientsList = JsonUtility.FromJson<ClientsList>(json);
        Clients = new List<Client>(clientsList.Clients);
    }
}
