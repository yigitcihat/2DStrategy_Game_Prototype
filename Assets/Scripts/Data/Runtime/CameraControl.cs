using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float moveSpeed = 1.0f; // Speed of camera movement
    public float zoomSpeed = 2.0f; // Speed of zooming
    public float minOrthoSize = 1.0f; // Minimum orthographic size
    public float maxOrthoSize = 10.0f; // Maximum orthographic size

    private Camera cameraComponent;
    private Vector3 touchStart; // Starting position of touch input for moving camera

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();
    }

    private void Update()
    {
        // Handle zooming using touch input
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            float newOrthoSize = cameraComponent.orthographicSize + deltaMagnitudeDiff * zoomSpeed * Time.deltaTime;
            newOrthoSize = Mathf.Clamp(newOrthoSize, minOrthoSize, maxOrthoSize);

            cameraComponent.orthographicSize = newOrthoSize;
        }
        else
        {
            float zoomInput = Input.GetAxis("Mouse ScrollWheel");

            float newOrthoSize = cameraComponent.orthographicSize - zoomInput * zoomSpeed * Time.deltaTime;
            newOrthoSize = Mathf.Clamp(newOrthoSize, minOrthoSize, maxOrthoSize);

            cameraComponent.orthographicSize = newOrthoSize;
        }
        // Handle camera movement using touch input
        if (Input.touchCount == 1 && !Input.GetMouseButton(0))
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStart = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 touchPos = touch.position;
                Vector3 delta = (touchPos - touchStart) * moveSpeed * Time.deltaTime;
                transform.Translate(-delta.x, -delta.y, 0);
                touchStart = touchPos;
            }
        }

        // Handle camera movement using WASD keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0)
        {
            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }

       
    }
}