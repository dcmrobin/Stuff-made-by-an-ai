using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float smoothing = 5f;
    public bool enableSmoothing = true;

    public Transform playerBody;

    float xRotation = 0f;
    float yRotation = 0f;

    private Queue<float> xRotationSamples = new Queue<float>();
    private Queue<float> yRotationSamples = new Queue<float>();
    private int sampleCount = 5;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Add the new mouse input values to the rotation sample queues
        xRotationSamples.Enqueue(mouseX);
        yRotationSamples.Enqueue(mouseY);

        // If the sample queues have more values than the desired number of samples, remove the oldest values
        if (xRotationSamples.Count > sampleCount)
        {
            xRotationSamples.Dequeue();
        }
        if (yRotationSamples.Count > sampleCount)
        {
            yRotationSamples.Dequeue();
        }

        // Calculate the sum of the rotation samples
        float sumXRotation = 0f;
        float sumYRotation = 0f;

        foreach (float x in xRotationSamples)
        {
            sumXRotation += x;
        }
        foreach (float y in yRotationSamples)
        {
            sumYRotation += y;
        }

        // Divide the sum by the number of samples to get the average
        float avgXRotation = sumXRotation / xRotationSamples.Count;
        float avgYRotation = sumYRotation / yRotationSamples.Count;

        // Update the camera's rotation using the smoothed mouse input values
        xRotation -= avgYRotation;
        yRotation += avgXRotation;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Quaternion targetRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        if (enableSmoothing)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, smoothing * Time.deltaTime);
        }
        else
        {
            transform.localRotation = targetRotation;
        }

        // Update the player character's body rotation
        playerBody.Rotate(Vector3.up * avgXRotation);
    }
}
