using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private Vector2 difference;

    private void OnMouseDown()
    {
        difference = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        Vector2 newPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + difference;
        transform.position = newPosition;
    }
}