using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    // Variables
    [Header("Swipe")]
    [SerializeField] private bool printSwipes;
    [SerializeField] private ThirdPersonMovement playerMovement;
    private float startPosX;
    private bool fingerDownMobile;

    [Header("Swipe sides")]
    [SerializeField] private float monitorHorizontalPixelPercent = 25f;
    private int maxHorizontalPixelDistance; 
    private float horizontalStatus = 0f;

    [Header("Swipe up")]
    [SerializeField] private float monitorVerticalPixelPercent = 2f;
    [SerializeField] private float maxTimeToSwipeUp = .35f; // in seconds
    private float startPosY; // cada "maxTimeToSwipeUp" es guarda de nou
    private int verticalStatus = 0;
    private int maxVerticalPixelDistance; 
    private float timeSinceSwiped = 0f;

    // PC
    private bool fingerDownPC;

    private void Start()
    {
        maxHorizontalPixelDistance = (int) Mathf.Floor(Screen.width * monitorHorizontalPixelPercent / 100);
        maxVerticalPixelDistance = (int) Mathf.Floor(Screen.height * monitorVerticalPixelPercent / 100);

        startPosX = Screen.width / 2;
    }

    private void Update()
    {
        // MOBIL //
        if (!fingerDownMobile && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            //startPos = Input.touches[0].position;
            startPosY = Input.touches[0].position.y;
            timeSinceSwiped = 0f;
            fingerDownMobile = true;
        }

        if (fingerDownMobile && Input.touchCount > 0)
        {
            // HORIZONTAL //
            horizontalStatus = ((Input.touches[0].position.x - startPosX) * 100 / maxHorizontalPixelDistance) / 100; // % de 0 a 1
            if (horizontalStatus > 1) horizontalStatus = 1;
            else if (horizontalStatus < -1) horizontalStatus = -1;

            playerMovement.setSide(horizontalStatus);
            // FI HORIZONTAL //

            // VERTICAL //
            if (verticalStatus == 0 && timeSinceSwiped <= maxTimeToSwipeUp)
            {
                timeSinceSwiped += Time.deltaTime;
                if ((Input.touches[0].position.y - startPosY) >= maxVerticalPixelDistance) 
                {
                    verticalStatus = 1;
                    playerMovement.jump();
                }
                else if (Input.touches[0].position.y < startPosY)
                {
                    startPosY = Input.touches[0].position.y;
                }
            }
            else
            {
                startPosY = Input.touches[0].position.y;
                verticalStatus = 0;
                timeSinceSwiped = 0f;
            }
            // FI VERTICAL //
        }

        if (fingerDownMobile && Input.touchCount <= 0)
        {
            horizontalStatus = 0f;
            verticalStatus = 0;

            // Avisem al jugador que s'ha de parar lateralment
            playerMovement.setSide(horizontalStatus);
            fingerDownMobile = false;
        }
        // FI MOBIL //



        // PC //
        if (!fingerDownMobile)
        {
            if (!fingerDownPC && Input.GetMouseButton(0))
            {
                //startPos = Input.mousePosition;
                startPosY = Input.mousePosition.y;
                timeSinceSwiped = 0f;
                fingerDownPC = true;
            }

            if (fingerDownPC)
            {
                // HORIZONTAL //
                horizontalStatus = ((Input.mousePosition.x - startPosX) * 100 / maxHorizontalPixelDistance) / 100;
                if (horizontalStatus > 1) horizontalStatus = 1;
                else if (horizontalStatus < -1) horizontalStatus = -1;
                
                playerMovement.setSide(horizontalStatus);
                // FI HORIZONTAL //

                // VERTICAL //
                if (verticalStatus == 0 && timeSinceSwiped <= maxTimeToSwipeUp)
                {
                    timeSinceSwiped += Time.deltaTime;
                    if ((Input.mousePosition.y - startPosY) >= maxVerticalPixelDistance) 
                    {
                        verticalStatus = 1;
                        playerMovement.jump();
                    }
                    else if (Input.mousePosition.y < startPosY)
                    {
                        startPosY = Input.mousePosition.y;
                    }
                }
                else
                {
                    startPosY = Input.mousePosition.y;
                    verticalStatus = 0;
                    timeSinceSwiped = 0f;
                }
                // FI VERTICAL //
            }

            if (fingerDownPC && !Input.GetMouseButton(0))
            {
                horizontalStatus = 0f;
                verticalStatus = 0;

                // Avisem al jugador que s'ha de parar lateralment
                playerMovement.setSide(horizontalStatus);
                fingerDownPC = false;
            }
        }
        // FI PC //
    }
}
