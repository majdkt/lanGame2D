using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject[] cornerObjects;
    private Color defaultColor;
    private Color touchedColor;
    private Vector2 difference;

    private void Start()
    {
        defaultColor = cornerObjects[0].GetComponent<Renderer>().material.color;
        touchedColor = Color.red; // Change it to the desired touched color

        // Assign the OnTriggerEnter2D and OnTriggerExit2D methods as the trigger event handlers for the corner objects
        for (int i = 0; i < cornerObjects.Length; i++)
        {
            cornerObjects[i].GetComponent<Renderer>().material.color = defaultColor;
        }
    }

    private void OnMouseDown()
    {
        difference = (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        Vector2 newPosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + difference;
        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the trigger is with one of the corner objects
        if (collision.gameObject.CompareTag("CornerObject"))
        {
            // Change the color of the collided corner object to the touched color
            collision.gameObject.GetComponent<Renderer>().material.color = touchedColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the trigger is with one of the corner objects
        if (collision.gameObject.CompareTag("CornerObject"))
        {
            // Reset the color of the corner object to the default color
            collision.gameObject.GetComponent<Renderer>().material.color = defaultColor;
        }
    }
}