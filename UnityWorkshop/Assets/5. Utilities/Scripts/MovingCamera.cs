using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    private Vector3 moveSpeed = new Vector3(2,0,0);
    private bool CanMove { get; set; } = true;

    
    void Update()
    {
        if (CanMove)
        {
            transform.position += moveSpeed * Time.deltaTime;
        }
    }
}
