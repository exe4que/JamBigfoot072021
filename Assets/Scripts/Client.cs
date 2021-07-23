using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Client
{
    public float TimerDuration;
    public int RandomOrder;
    public IceCream Order;

    private float _spawnTime;
    private bool _punched = false;
    public bool Punched => _punched;
    public float Timer => Mathf.Min((Time.time - _spawnTime) / TimerDuration, 1f);
    public int RemainingSeconds => Mathf.Max(0, (int)(TimerDuration - (Time.time - _spawnTime)));

    public void Initialize()
    {
        _spawnTime = Time.time;
    }

    public void GetPunched()
    {
        _punched = true;
    }
}


