using UnityEngine;

public class TouchCamera : MonoBehaviour
{
    private float zoomSpeedTouch = 0.05f;
    private float rotationSpeedTouch = 0.1f;

    void Update()
    {
        if (Input.touchCount == 1)
        {
            float touchDelta = Input.GetTouch(0).deltaPosition.x;
            Vector3 stepMove = new Vector3(0,touchDelta * rotationSpeedTouch, 0);
            Vector3 pos = transform.parent.eulerAngles + stepMove;
            
            transform.parent.eulerAngles = pos;
        }
        else if (Input.touchCount == 2)
        {
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

            this.GetComponent<Camera>().fieldOfView += deltaMagnitudeDiff * zoomSpeedTouch;

            // Clamp the field of view to make sure it's between 10 and 100.
            this.GetComponent<Camera>().fieldOfView = Mathf.Clamp(this.GetComponent<Camera>().fieldOfView, 15f, 80f);
        }
    }
}
