using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientAvatar : MonoBehaviour
{
    public Client Client;

    public float TargetPositionInLine;

    public Vector3 GetPositionAlongTheLine(float normalizedPosition)
    {
        var path = ClientsManager.Instance.ClientsLinePath;

        return Vector3.zero;
    }
}
