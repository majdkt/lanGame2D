using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 difference;
    private GameManager gameManager;

    private void Start()
    {
        // Find the GameManager in the scene and store a reference to it
        gameManager = FindObjectOfType<GameManager>();
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
        if (collision.gameObject.CompareTag("CornerObject"))
        {
            // Determine if the corner interaction is correct or not
            bool isCorrect = gameManager.IsCorrectInteraction(collision.gameObject);

            // Pass the correctness information to the GameManager
            gameManager.HandleInteraction(isCorrect);
        }
    }
}