using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 0.125f;
    public Vector3 offset;

    private void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
