using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float orthoZoomSpeed = 0.5f;
    public float minOrthographicSize = 3f;
    public float maxOrthographicSize = 7f;

    Vector3 startTouchPositionUC; //UC - Unity Coordinates
    Vector3 movingTouchPositionUC;

    float areaSize = 9.5f; //this should be provided from JSON at some point

    float mapBoundriesXPositive;
    float mapBoundriesXNegative;

    float mapBoundriesYPositive;
    float mapBoundriesYNegative;

    // Update is called once per frame
    void Update()
    {
        //Scroll map
        if (Input.touchCount == 1)
        {
            //Store touch
            Touch touch = Input.GetTouch(0);

            //On touch down
            if (touch.phase == TouchPhase.Began)
            {

                //This is position in unity coordinates when touch on mobile devices occures
                startTouchPositionUC = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0f));


                //calculates max and min position of camera where camere can move in unity coordinates based on camera size
                //cause camera size can change based on zoom in/out
                mapBoundriesXPositive = areaSize - Camera.main.orthographicSize * Camera.main.aspect;
                mapBoundriesXNegative = -areaSize + Camera.main.orthographicSize * Camera.main.aspect;

                mapBoundriesYPositive = areaSize - Camera.main.orthographicSize;
                mapBoundriesYNegative = -areaSize + Camera.main.orthographicSize;
            }

            //On touch moved
            if (touch.phase == TouchPhase.Moved)
            {
                //Gets canera boubds
                float cameraBoundriesXPOsitive = transform.position.x + (Camera.main.orthographicSize * Camera.main.aspect);
                float cameraBoundriesXNegative = transform.position.x - (Camera.main.orthographicSize * Camera.main.aspect);

                //This is position of touch in unity coordinates
                movingTouchPositionUC = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0f));

                
                //Camera should move for same distance as touch moved from its first click     

                //calculates new x & y position for camera bases on distance touch traveled
                float x = transform.position.x + (startTouchPositionUC.x - movingTouchPositionUC.x);
                float y = transform.position.y + (startTouchPositionUC.y - movingTouchPositionUC.y);

                //Checks to see if camera coordinates are out of bounds and put them back to max or min bounds
                x = Mathf.Clamp(x, mapBoundriesXNegative, mapBoundriesXPositive);
                y = Mathf.Clamp(y, mapBoundriesYNegative, mapBoundriesYPositive);

                //Finally set new camera position
                transform.position = new Vector3(x, y, -10f);
            }
        }

        if(Input.touchCount == 2)
        {
            //Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // If the camera is orthographic...
            if (Camera.main.orthographic)
            {
                //Camera size
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + deltaMagnitudeDiff * orthoZoomSpeed, minOrthographicSize, maxOrthographicSize);

                //Changes camera position if zoom in/out would move camera out of bounds
                float x = transform.position.x;
                float y = transform.position.y;

                mapBoundriesXPositive = areaSize - Camera.main.orthographicSize * Camera.main.aspect;
                mapBoundriesXNegative = -areaSize + Camera.main.orthographicSize * Camera.main.aspect;

                mapBoundriesYPositive = areaSize - Camera.main.orthographicSize;
                mapBoundriesYNegative = -areaSize + Camera.main.orthographicSize;

                x = Mathf.Clamp(x, mapBoundriesXNegative, mapBoundriesXPositive);
                y = Mathf.Clamp(y, mapBoundriesYNegative, mapBoundriesYPositive);

                transform.position = new Vector3(x, y, -10f);

            }
        }
    }
}
