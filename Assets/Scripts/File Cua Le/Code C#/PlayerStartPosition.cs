using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPosition : MonoBehaviour
{
    public Transform startPoint;

    void Start()
    {
        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }
    }
}
