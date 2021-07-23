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
    public float Timer => TimerDuration - (Time.time - _spawnTime);

    public void Initialize()
    {
        _spawnTime = Time.time;
    }
}
    
    
