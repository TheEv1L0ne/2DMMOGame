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

            }

            //On touch moved
            if (touch.phase == TouchPhase.Moved)
            {
                //Coordinates of touch while moving
                movingTouchPositionUC = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0f));

                //Camera should move for same distance as touch moved from its first click     
                transform.position = transform.position + (startTouchPositionUC - movingTouchPositionUC);
                transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
            }
        }

        if(Input.touchCount == 2)
        {
            // Store both touches.
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
                // ... change the orthographic size based on the change in distance between the touches.
                if(Camera.main.orthographicSize >= minOrthographicSize && Camera.main.orthographicSize <= maxOrthographicSize)
                    Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                //Makes sure that camere size do not fall below boundries
                //MIN
                if (Camera.main.orthographicSize < minOrthographicSize)
                    Camera.main.orthographicSize = minOrthographicSize;
                //MAX
                if (Camera.main.orthographicSize > maxOrthographicSize)
                    Camera.main.orthographicSize = maxOrthographicSize;
            }
        }
    }
}
