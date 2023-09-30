using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager2 : MonoBehaviour
{
    public Button left;
    public Button right;
    private WordGenerationAndAssignment wordGenAndAssign; // Reference to WordGenerationAndAssignment
    public Button restartButton;
    public TextMeshPro scoreText;
    public GameObject gameOverObject;
    public GameObject gameGameObject;
    public GameObject block;
    private GameObject background;
    public AudioSource winSound; // Reference to the Audio Source component
    public AudioSource loseSound;
    public float pitchIncrement = 0.1f; // Adjust this value to control the pitch increment
    public Color correctColor = Color.green;
    public Color backGrColor = Color.white; 
    public float[] yPositions; // Array to store the Y positions
    private int currentPositionIndex = 0; // Index of the current position in the array


    private int score = 0;
    private bool isGameOver = false;

    private void Start()
    {
        wordGenAndAssign = GetComponent<WordGenerationAndAssignment>();
        // Add event listeners to the left and right buttons
        left.onClick.AddListener(() => HandleButtonClick(left));
        right.onClick.AddListener(() => HandleButtonClick(right));
        
        // Attach the RestartGame method to the button's onClick event
        restartButton.onClick.AddListener(RestartGame);
        yPositions = new float[] { -2f, 0.5f, 3f };
    }

    public void HandleButtonClick(Button clickedButton)
    {
        // Find the child GameObject with TextMeshPro component
            TextMeshPro textMeshPro = clickedButton.GetComponentInChildren<TextMeshPro>();

            if (textMeshPro != null)
            {
                // Get the text from the TextMeshPro component
                string chosenTranslation = textMeshPro.text;
            
                // Pass the chosen translation to the WordGenerationAndAssignment script
                wordGenAndAssign.HandleChoice2(chosenTranslation);
            }
            else
            {
                Debug.LogWarning("TextMeshPro component not found on the clicked button's children.");
            }
    }

    public void HandleInteraction(bool isCorrect)
    {
        float initialX = block.transform.position.x;
        float initialZ = block.transform.position.z;
        background = GameObject.Find("BackgroundL2");

        if (isCorrect)
        {
            IncreaseScore();
            winSound.Play();
            winSound.pitch += pitchIncrement;
            // Calculate the next Y position index (cycling through positions)
            currentPositionIndex = (currentPositionIndex + 1) % yPositions.Length;
            // Set the new Y position
            Vector3 newPosition = new Vector3(initialX, yPositions[currentPositionIndex], initialZ);
            block.transform.position = newPosition;
            startBlinking();
        }
        else
        {
            isGameOver = true;
            // Play the audio and increase the pitch on correct answer
            winSound.pitch = 1;
            loseSound.Play();
            gameOverObject.SetActive(true);
            gameGameObject.SetActive(false);
            // Reset to the first Y position
            currentPositionIndex = 0;

            // Set the Y position back to the first position
            Vector3 newPosition = new Vector3(initialX, yPositions[currentPositionIndex], initialZ);
            block.transform.position = newPosition;
        }
    }

    private void IncreaseScore()
    {
        score++;
        UpdateScoreText();
        Debug.Log("Score increased to: " + score);
    }
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public void RestartGame()
    {
        wordGenAndAssign.GenerateAndAssignWord();
        score = 0;
        UpdateScoreText();
        isGameOver = false;
        gameOverObject.SetActive(false);
        gameGameObject.SetActive(true);
    }

    private IEnumerator blink()
    { 
        background.GetComponent<Image>().color = correctColor;
        yield return new WaitForSeconds(0.5f);
        background.GetComponent<Image>().color = backGrColor;
    }

    public void startBlinking()
    {
        StartCoroutine(blink());
    }
}