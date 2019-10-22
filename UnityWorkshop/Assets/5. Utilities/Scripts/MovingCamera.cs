using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    private Vector3 moveSpeed = new Vector3(2, 0, 0);
    private bool CanMove { get; set; } = true;
    [SerializeField] private MovingBackground[] movingBackgrounds;
    private float deltaX = 0;
    private int currentBackgroundIndex = 0;
    private int otherBackgroundIndex = 1;
    private Vector3 backgroundOffset = new Vector3(2.5f, 0, 0); // khoảng cách từ tâm background đến camera tối đa để chuyển vị trí background còn lại
    private Vector3 backgroundStep = new Vector3(23, 0, 0); // khoảng cách giữa 2 background

    private Transform playerTransform;
    private bool hasMapLength = false;
    private float maxX;
    [SerializeField] private float freeMoveStep; // khoảng cách player và cam để cam bắt đầu chạy theo player
    private Vector3 deltaDistance = Vector3.zero;
    public void SetMapLength(int length)
    {
        // lấy transform của player trước
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        // rộng cam = ratio * 2 * size camera (size camera mặc định là 5)
        // chiều rộng cam = Screen.width;
        // chiều cao cam Screen.height;
        // khoảng cách giữa 2 ô block đầu và cuối là 1.3* (length-1)
        // => maxX = (length-1)*1.3f - rộng cam
        maxX = (length - 1) * 1.3f - ((float)Screen.width / (float)Screen.height) * 2 * 5;
        hasMapLength = true;
    }


    void Update()
    {
        if (hasMapLength == false) return;

        // di chuyển cam theo player (chỉ theo chiều X)
        if(playerTransform.position.x < transform.position.x)
        {
            // cam không dịch trái quá 0
            if (transform.position.x <= 0) return;
            // nếu như player cách xa hơn freeMoveStep thì dịch cam sang trái
            if(transform.position.x - playerTransform.position.x > freeMoveStep)
            {
                deltaDistance.x = transform.position.x - playerTransform.position.x - freeMoveStep;
                transform.position -= deltaDistance;
            }
        }
        else
        {
            // cam không dịch phải quá maxX
            if (transform.position.x >= maxX) return;
            // nếu như player cách xa hơn freeMoveStep thì dịch cam sang phải
            if (playerTransform.position.x - transform.position.x > freeMoveStep)
            {
                deltaDistance.x = playerTransform.position.x - transform.position.x - freeMoveStep;
                transform.position += deltaDistance;
            }
        }


        // di chuyển background chạy theo camera
        if(transform.position.x - movingBackgrounds[currentBackgroundIndex].transform.position.x > backgroundOffset.x)
        {
            if (movingBackgrounds[otherBackgroundIndex].transform.position.x < movingBackgrounds[currentBackgroundIndex].transform.position.x)
                movingBackgrounds[otherBackgroundIndex].transform.position += 2 * backgroundStep;

            if(transform.position.x - movingBackgrounds[currentBackgroundIndex].transform.position.x > backgroundStep.x)
            {
                // đổi sang background khác
                if (currentBackgroundIndex == 0)
                {
                    currentBackgroundIndex = 1;
                    otherBackgroundIndex = 0;
                }
                else
                {
                    currentBackgroundIndex = 0;
                    otherBackgroundIndex = 1;
                }
            }
        }
        else if( transform.position.x - movingBackgrounds[currentBackgroundIndex].transform.position.x < -backgroundOffset.x)
        {

            if (movingBackgrounds[otherBackgroundIndex].transform.position.x > movingBackgrounds[currentBackgroundIndex].transform.position.x)
                movingBackgrounds[otherBackgroundIndex].transform.position -= 2 * backgroundStep;

            if (transform.position.x - movingBackgrounds[currentBackgroundIndex].transform.position.x > -backgroundStep.x)
            {
                // đổi sang background khác
                if (currentBackgroundIndex == 0)
                {
                    currentBackgroundIndex = 1;
                    otherBackgroundIndex = 0;
                }
                else
                {
                    currentBackgroundIndex = 0;
                    otherBackgroundIndex = 1;
                }
            }
        }
    }
}
