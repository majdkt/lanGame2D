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
    public Image background;
    public AudioSource winSound; // Reference to the Audio Source component
    public AudioSource loseSound;
    public float pitchIncrement = 0.1f; // Adjust this value to control the pitch increment
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;

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
        if (isCorrect)
        {
            IncreaseScore();
            background.color = correctColor;
            winSound.Play();
            winSound.pitch += pitchIncrement;
        }
        else
        {
            isGameOver = true;
            // Play the audio and increase the pitch on correct answer
            winSound.pitch = 1;
            loseSound.Play();
            gameOverObject.SetActive(true);
            gameGameObject.SetActive(false);
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

}