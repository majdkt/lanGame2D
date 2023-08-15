using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public WordGenerator wordGenerator;
    public GameObject[] cornerObjects;
    public TextMeshPro germanWordText;
    public TextMeshPro scoreText;
    public GameObject gameOverObject;
    public GameObject gameGameObject;

    private string correctTranslation;
    private string[] incorrectTranslations;

    private Color defaultColor = Color.white;
    public Color correctColor = Color.green;    // Change to desired correct color
    public Color incorrectColor = Color.red;    // Change to desired incorrect color

    private Renderer correctRenderer;
    private int score = 0;

    private bool isGameOver = false;

    private void Start()
    {
        AssignGermanWordToText();
        GenerateRandomWordAndTranslations();
        AssignTranslationsToCornerObjects();
        UpdateScoreText();
    }

    private void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
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
                correctRenderer = cornerObjects[0].GetComponent<Renderer>();
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
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    public bool IsCorrectInteraction(GameObject cornerObject)
    {
        return cornerObject == cornerObjects[0];
    }

    public void HandleInteraction(bool isCorrect)
    {
        if (isCorrect)
        {
            IncreaseScore();
            // Change color to correctColor for the correct corner object
            correctRenderer.material.color = correctColor;
            StartCoroutine(ResetColorAfterDelay(correctRenderer, defaultColor));
            RestartGame();
        }
        else
        {
            // Change color to incorrectColor for the chosen corner object
            correctRenderer.material.color = incorrectColor;
            StartCoroutine(ResetColorAfterDelay(correctRenderer, defaultColor));
            ActivateGameOver();
        }
    }

    private IEnumerator ResetColorAfterDelay(Renderer renderer, Color targetColor)
    {
        yield return new WaitForSeconds(0.3f); // Change this delay as needed
        renderer.material.color = targetColor;
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
