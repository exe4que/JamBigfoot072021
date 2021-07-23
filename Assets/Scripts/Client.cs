using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Client
{
    public float TimerDuration;
    public string RandomOrder;
    public IceCream Order;

    private float _spawnTime;
    public float Timer => Mathf.Min((Time.time - _spawnTime) / TimerDuration, 1f);

    public void Initialize()
    {
        _spawnTime = Time.time;
    }
}
    
    
