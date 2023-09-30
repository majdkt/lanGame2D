using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshPro scoreText;
    public GameObject gameOverObject;
    public GameObject gameGameObject;
    public Button restartButton;
    public Image background;
    public PlayerController player;
    public AudioSource winSound; // Reference to the Audio Source component
    public AudioSource loseSound;
    public float pitchIncrement = 0.1f; // Adjust this value to control the pitch increment

    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;

    private int score = 0;
    private bool isGameOver = false;

    private WordGenerationAndAssignment wordGenAndAssign; // Reference to WordGenerationAndAssignment

    private void Start()
    {
        StartBlinkingBackground();
        wordGenAndAssign = GetComponent<WordGenerationAndAssignment>();
        // Attach the RestartGame method to the button's onClick event
        restartButton.onClick.AddListener(RestartGame);
    }
    

    private IEnumerator BlinkBackground()
    {
        Color startColor = new Color(1f, 1f, 1f, 0.25f); //White
        Color targetColor = Color.black;
        while (!isGameOver)        
        {
            background.color = startColor;
            yield return new WaitForSeconds(0.5f);
            background.color = targetColor;
            yield return new WaitForSeconds(0.5f);
        }
        while (isGameOver)        {
            background.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            background.color = targetColor;
            yield return new WaitForSeconds(0.5f);
        }

    }


    public void StartBlinkingBackground()
    {
        StartCoroutine(BlinkBackground());
    }
    
    
    
    public void HandleInteraction(bool isCorrect)
    {
        if (isCorrect)
        {
            StartCoroutine(hideWrongTranslations());
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
        PlayerController.ResetPlayerPosition(player);
        score = 0;
        UpdateScoreText();
        isGameOver = false;
        gameOverObject.SetActive(false);
        gameGameObject.SetActive(true);
        StartBlinkingBackground();
    }
    


    
    private IEnumerator hideWrongTranslations()
    {
        foreach (GameObject cornerObject in wordGenAndAssign.cornerObjects)
        {
            TextMeshPro textMeshPro = cornerObject.GetComponentInChildren<TextMeshPro>();
            if (textMeshPro.text != wordGenAndAssign.correctTranslation)
            {
                textMeshPro.enabled = false;
            }
        }

        yield return new WaitForSeconds(1f); // Wait for 1 second

        foreach (GameObject cornerObject in wordGenAndAssign.cornerObjects)
        {
            TextMeshPro textMeshPro = cornerObject.GetComponentInChildren<TextMeshPro>();
            textMeshPro.enabled = true;
        }
    }
}