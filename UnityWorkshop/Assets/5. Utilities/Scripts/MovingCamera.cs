using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    private Vector3 moveSpeed = new Vector3(2, 0, 0);
    private bool CanMove { get; set; } = true;
    [SerializeField] private MovingBackground[] movingBackgrounds;
    private float deltaX = 0;
    private Vector3 backgroundStep = new Vector3(23, 0, 0);


    void Update()
    {
        if (CanMove)
        {
            transform.position += moveSpeed * Time.deltaTime;
            deltaX += moveSpeed.x * Time.deltaTime;
        }
        if (deltaX >= backgroundStep.x)
        {
            for (int i = 0; i < movingBackgrounds.Length; i++)
            {
                movingBackgrounds[i].transform.position += backgroundStep;
            }
            deltaX -= backgroundStep.x;
        }
    }
}
