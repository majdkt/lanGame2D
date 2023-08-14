using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float targetWidth = 1080f;
    private const float targetHeight = 1920f;
    private const float targetAspect = targetWidth / targetHeight;

    void Start()
    {
        // Set the desired aspect ratio
        float targetScreenAspect = (float)Screen.height / (float)Screen.width;

        // Check if the current aspect ratio is already in portrait mode
        if (targetScreenAspect < targetAspect)
        {
            // Calculate the desired width to fit the screen height
            float desiredWidth = targetHeight / targetScreenAspect;

            // Calculate the letterbox/pillarbox size
            float letterboxSize = (desiredWidth - targetWidth) * 0.5f;

            // Set the camera's viewport rect to create letterboxing/pillarboxing
            Camera.main.rect = new Rect(letterboxSize / desiredWidth, 0f, targetWidth / desiredWidth, 1f);
        }
        else
        {
            // Calculate the desired height to fit the screen width
            float desiredHeight = targetWidth * targetScreenAspect;

            // Calculate the letterbox/pillarbox size
            float pillarboxSize = (desiredHeight - targetHeight) * 0.5f;

            // Set the camera's viewport rect to create letterboxing/pillarboxing
            Camera.main.rect = new Rect(0f, pillarboxSize / desiredHeight, 1f, targetHeight / desiredHeight);
        }
    }
}
