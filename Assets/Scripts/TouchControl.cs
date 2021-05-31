using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TouchControl
{
    private bool isPressing = false;
    private Vector2 touchDownPosition = Vector2.zero;
    private Vector2 touchPosition = Vector2.zero;
    private float dragRate = 0;
    public Vector2 joyStick;
    public void UpdateTouchSystem()
    {
        float minDragRate = 0.05F; //Rate to the screen height.

        if(Input.GetMouseButton(0))
        {
            if(isPressing == false)
            {
                touchDownPosition = Input.mousePosition;
                touchPosition = Input.mousePosition;
                dragRate = 0;
                isPressing = true;
            }
            else
            {
                touchPosition = Input.mousePosition;
                dragRate = (Vector2.Distance(touchDownPosition, touchPosition)) / Screen.height;

                if(dragRate > minDragRate) //Min Drag Threshold
                {
                    Vector2 totalDist = touchPosition - touchDownPosition;
                    float screenRate = Screen.height / 5;
                    joyStick = totalDist / screenRate;
                    joyStick = new Vector2(Mathf.Clamp(joyStick.x, -1, 1), Mathf.Clamp(joyStick.y, -1, 1));
                }
            }
        }
        else
        {
            if(isPressing)
            {
                touchDownPosition = Vector2.zero;
                touchPosition = Vector2.zero;
                dragRate = 0;
                isPressing = false;
            }

            joyStick = Vector2.zero;
        }
    }
}