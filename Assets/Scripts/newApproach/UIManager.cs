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

    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;

    private int score = 0;
    private bool isGameOver = false;

    private WordGenerationAndAssignment wordGenAndAssign; // Reference to WordGenerationAndAssignment

    private void Start()
    {
        StartBlinkingBackground();
        wordGenAndAssign = GetComponent<WordGenerationAndAssignment>();
        restartButton.onClick.AddListener(RestartGame);
    }

    public void StartBlinkingBackground()
    {
        StartCoroutine(BlinkBackground());
    }

    private IEnumerator BlinkBackground()
    {
        Color startColor = new Color(1f, 1f, 1f, 0.25f);
        Color targetColor = Color.black;
        while (!isGameOver)
        {
            background.color = startColor;
            yield return new WaitForSeconds(0.5f);
            background.color = targetColor;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void HandleInteraction(bool isCorrect)
    {
        if (isCorrect)
        {
            IncreaseScore();
            background.color = correctColor;

        }
        else
        {
            background.color = incorrectColor;
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
        
    }

}
