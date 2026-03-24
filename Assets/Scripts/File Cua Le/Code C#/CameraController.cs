using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target;
    Vector3 velocity = Vector3.zero;
    [Range(0, 1)]
    public float smoothTime;
    private void Awake()
    {
        target = GetComponent<Transform>();
    }
}
