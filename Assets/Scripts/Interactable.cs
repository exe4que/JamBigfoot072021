using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Action OnInteract;

    public void OnMouseDown()
    {
        Debug.Log("On Mouse Down");
        OnInteract?.Invoke();
    }

}
