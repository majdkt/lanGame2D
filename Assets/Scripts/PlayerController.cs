using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 difference;
    private WordGenerationAndAssignment wordGenAndAssign; // Reference to WordGenerationAndAssignment

    private void Start()
    {
        // Find the WordGenerationAndAssignment script in the scene and store a reference to it
        wordGenAndAssign = FindObjectOfType<WordGenerationAndAssignment>();
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
            // Get the text from the collided corner object
            string chosenTranslation = collision.gameObject.GetComponentInChildren<TextMeshPro>().text;

            // Pass the chosen translation to the WordGenerationAndAssignment script
            wordGenAndAssign.HandleChoice(chosenTranslation);
        }
    }

    public static void ResetPlayerPosition(PlayerController player)
    {
        // Set the player's position to (0, 0, -1) for the specified player instance
        player.transform.position = new Vector3(0f, 0f, -1f);
    }
}