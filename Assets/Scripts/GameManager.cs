using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public WordGenerator wordGenerator;
    public GameObject[] cornerObjects;
    public TextMeshPro germanWordText;
    public TextMeshPro scoreText;
    public GameObject gameOverObject;
    public GameObject gameGameObject;
    public Image background; // Reference to the background image

    private string correctTranslation;
    private string[] incorrectTranslations;

    private Color defaultColor = Color.white;
    public Color correctColor = Color.green;    // Change to desired correct color
    public Color incorrectColor = Color.red;    // Change to desired incorrect color

    private int score = 0;
    private bool isGameOver = false;
    private bool isCorrectAnswerChosen = false; // Track if the correct answer is chosen
    private bool isBlinking = false; // Track if the background is currently blinking

    private void Start()
    {
        AssignGermanWordToText();
        GenerateRandomWordAndTranslations();
        AssignTranslationsToCornerObjects();
        UpdateScoreText();
        StartBlinkingBackground();
    }

    private void GenerateRandomWordAndTranslations()
    {
        Word randomWord = wordGenerator.GenerateRandomWord();

        string germanWord = randomWord.germanWord;
        germanWordText.text = germanWord;

        correctTranslation = randomWord.correctTranslation.text;

        Translation[] translations = randomWord.incorrectTranslations;
        incorrectTranslations = new string[cornerObjects.Length - 1];

        int correctIndex = Random.Range(0, cornerObjects.Length - 1);
        int incorrectIndex = 0;

        for (int i = 0; i < translations.Length; i++)
        {
            if (i == correctIndex)
            {
                ShuffleArray(cornerObjects);
                cornerObjects[0].GetComponentInChildren<TextMeshPro>().text = correctTranslation;
            }
            else if (incorrectIndex < incorrectTranslations.Length)
            {
                incorrectTranslations[incorrectIndex] = translations[i].text;
                incorrectIndex++;
            }
        }

        ShuffleArray(incorrectTranslations);
    }

    private void AssignTranslationsToCornerObjects()
    {
        int incorrectIndex = 0;

        for (int i = 1; i < cornerObjects.Length; i++)
        {
            TextMeshPro textMeshPro = cornerObjects[i].GetComponentInChildren<TextMeshPro>();
            textMeshPro.text = incorrectTranslations[incorrectIndex];
            incorrectIndex++;
        }
    }

    private void AssignGermanWordToText()
    {
        germanWordText.text = wordGenerator.GenerateRandomWord().germanWord;
    }

    private void ShuffleArray<T>(T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = Random.Range(0, n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }

    public bool IsCorrectInteraction(GameObject cornerObject)
    {
        return cornerObject == cornerObjects[0];
    }

    public void StartBlinkingBackground()
    {
        if (!isBlinking)
        {
            StartCoroutine(BlinkBackground());
        }
    }

    private IEnumerator BlinkBackground()
    {
        isBlinking = true;

        while (!isGameOver)
        {
            if (isCorrectAnswerChosen)
            {
                background.color = correctColor; // Change background to correctColor
                yield return new WaitForSeconds(1.0f); // Wait for 1 second
                background.color = defaultColor; // Reset background color to default
                isCorrectAnswerChosen = false; // Reset the flag
            }
            else
            {
                background.color = Color.white; // Change background to white
                yield return new WaitForSeconds(0.5f); // Adjust the delay as needed
                background.color = Color.black; // Change background to black
                yield return new WaitForSeconds(0.5f); // Adjust the delay as needed
            }
        }

        isBlinking = false;
    }

    public void HandleInteraction(bool isCorrect)
    {
        if (isCorrect)
        {
            IncreaseScore();
            isCorrectAnswerChosen = true; // Set the flag to true
            background.color = correctColor; // Change background to correctColor
            StartCoroutine(ResetBackgroundColorAfterDelay(1.0f, defaultColor)); // Reset color after 1 second
            RestartGame();
        }
        else
        {
            background.color = incorrectColor; // Change background to incorrectColor
            StopBlinkingBackground(); // Stop the background blinking
            ActivateGameOver();
        }
    }

    private IEnumerator ResetBackgroundColorAfterDelay(float delay, Color targetColor)
    {
        yield return new WaitForSeconds(delay);
        background.color = targetColor; // Reset background color to the target color
    }

    public void StopBlinkingBackground()
    {
        StopAllCoroutines(); // Stop all coroutines, including BlinkBackground
    }

    private void IncreaseScore()
    {
        score++;
        UpdateScoreText();
    }

    private void RestartGame()
    {
        AssignGermanWordToText();
        GenerateRandomWordAndTranslations();
        AssignTranslationsToCornerObjects();
        StartBlinkingBackground(); // Restart the background blinking
        isGameOver = false;
    }

    private void ActivateGameOver()
    {
        isGameOver = true;
        gameOverObject.SetActive(true);
        gameGameObject.SetActive(false);
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
