using UnityEngine;
using System.Collections;

/// <summary>
/// Mouse position.
///
/// this class handles getting the location in real space the mouse cursor is over.
///
/// </summary>

public class MousePosition : MonoBehaviour
{

    float yRotation;
    float xRotation;
    float lookSensitivity = 5;
    float currentXRotation;
    float currentYRotation;
    float yRotationV;
    float xRotationV;
    float lookSmoothnes = 0.1f;

    private void Start()
    {
        CaptureMouse();
    }

    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            yRotation += Input.GetAxis("Mouse X") * lookSensitivity;
            xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;
            xRotation = Mathf.Clamp(xRotation, -80, 100);
            currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationV, lookSmoothnes);
            currentYRotation = Mathf.SmoothDamp(currentYRotation, yRotation, ref yRotationV, lookSmoothnes);
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
        else if (Input.GetButton("Fire1"))
        {
            CaptureMouse();
        }

        if (Input.GetButton("Cancel"))
        {
            ReleaseMouse();
        }
    }

    private void CaptureMouse()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void ReleaseMouse()
    {
        transform.rotation = Quaternion.identity;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}