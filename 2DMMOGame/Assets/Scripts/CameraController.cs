using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 position;
    private float width;
    private float height;

    // Start is called before the first frame update
    void Awake()
    {
        width = (float)Screen.width;
        height = (float)Screen.height;

        Debug.Log("WIDTH: " + width);
        Debug.Log("HEIGHT: " + height);

        // Position used for the cube.
        position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    Vector3 startTouchPositionUC; //UC - Unity Coordinates
    Vector3 movingTouchPositionUC;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);


            if (touch.phase == TouchPhase.Began)
            {

                //This is position in unity coordinates when touch on mobile devices occures
                startTouchPositionUC = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0f));

            }

            if (touch.phase == TouchPhase.Moved)
            {
                //Coordinates of touch while moving
                movingTouchPositionUC = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0f));

                //Camera should move for same distance as touch moved from its first click     
                transform.position = transform.position + (startTouchPositionUC - movingTouchPositionUC);
            }
        }
    }
}
