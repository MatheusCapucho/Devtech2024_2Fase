using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTransform : MonoBehaviour
{
    Transform playerTransform;
    private void Awake()
    {
        playerTransform = FindFirstObjectByType<PlayerManager>().transform;
    }
    void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        //if (Vector3.Distance(playerTransform.position, transform.position) > 1.05f)
            //transform.position = playerTransform.position;
    }
}
