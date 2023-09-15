using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshPro scoreText;
    public GameObject gameOverObject;
    public GameObject gameGameObject;
    public Image background;

    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;

    private int score = 0;
    private bool isGameOver = false;

    private WordGenerationAndAssignment wordGenAndAssign; // Reference to WordGenerationAndAssignment

    private void Start()
    {
        // Find all TextMeshPro components in the scene
        TextMeshPro[] textMeshPros = FindObjectsOfType<TextMeshPro>();
        // Iterate through each TextMeshPro component and auto-size it
        foreach (TextMeshPro textMeshPro in textMeshPros)
        {
            AutoSizeTextToFitRectTransform(textMeshPro);
        }
        StartBlinkingBackground();
        wordGenAndAssign = GetComponent<WordGenerationAndAssignment>();
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

            // Swap startColor and targetColor for the next cycle
            (startColor, targetColor) = (targetColor, startColor);
        }
    }

    public void HandleInteraction(bool isCorrect)
    {
        if (isCorrect)
        {
            IncreaseScore();
            background.color = correctColor;
            StartCoroutine(ResetBackgroundColorAfterDelay(1.0f));
        }
        else
        {
            background.color = incorrectColor;
            gameOverObject.SetActive(true);
            gameGameObject.SetActive(false);
            isGameOver = true;
        }
    }

    private IEnumerator ResetBackgroundColorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        background.color = new Color(1f, 1f, 1f, 0.25f);
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
 
    public static void AutoSizeTextToFitRectTransform(TextMeshPro textMeshPro)
    {
        if (textMeshPro == null)
        {
            return;
        }

        RectTransform rectTransform = textMeshPro.rectTransform;
        float availableWidth = rectTransform.rect.width;
        float availableHeight = rectTransform.rect.height;

        // Calculate the aspect ratio of the RectTransform
        float rectAspectRatio = availableWidth / availableHeight;

        // Calculate the aspect ratio of the TextMeshPro component
        float textAspectRatio = textMeshPro.preferredWidth / textMeshPro.preferredHeight;

        // Calculate the scaling factor based on the aspect ratios
        float scaleFactor = rectAspectRatio / textAspectRatio;

        // Set the font size based on the scaling factor
        textMeshPro.fontSize *= scaleFactor;
    }
}
