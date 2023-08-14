using UnityEngine;
using TMPro;

public class Levelcontroller : MonoBehaviour
{
    public WordGenerator wordGenerator;
    public GameObject[] cornerObjects;
    public TextMeshPro germanWordText;
    public TextMeshPro scoreText;
    public GameObject wrongTranslationObject;

    private string correctTranslation;
    private string[] incorrectTranslations;
    private int score;

    private void Start()
    {
        StartNewLevel();
    }

    private void StartNewLevel()
    {
        ResetLevel();
        GenerateRandomWordAndTranslations();
        AssignTranslationsToCornerObjects();
        UpdateScoreText();
    }

    private void ResetLevel()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();
        wrongTranslationObject.SetActive(false);
    }

    private void GenerateRandomWordAndTranslations()
    {
        Word randomWord = wordGenerator.GenerateRandomWord();

        // Get the German word
        string germanWord = randomWord.germanWord;
        germanWordText.text = germanWord;

        // Get the translations
        Translation[] translations = randomWord.translations;
        incorrectTranslations = new string[cornerObjects.Length - 1];

        int correctIndex = 0;
        int incorrectIndex = 0;

        // Assign translations to correct and incorrect arrays
        for (int i = 0; i < translations.Length; i++)
        {
            if (translations[i].isCorrect)
            {
                correctTranslation = translations[i].text;
                correctIndex = i;
            }
            else
            {
                incorrectTranslations[incorrectIndex] = translations[i].text;
                incorrectIndex++;
            }
        }

        // Shuffle the incorrect translations array
        ShuffleArray(incorrectTranslations);

        // Assign the correct translation to a random corner object
        TextMeshPro correctText = cornerObjects[correctIndex].GetComponentInChildren<TextMeshPro>();
        correctText.text = correctTranslation;
    }

    private void AssignTranslationsToCornerObjects()
    {
        int incorrectIndex = 0;

        // Assign incorrect translations to the remaining corner objects
        for (int i = 0; i < cornerObjects.Length; i++)
        {
            if (cornerObjects[i].GetComponentInChildren<TextMeshPro>().text != correctTranslation)
            {
                TextMeshPro textMeshPro = cornerObjects[i].GetComponentInChildren<TextMeshPro>();
                textMeshPro.text = incorrectTranslations[incorrectIndex];
                incorrectIndex++;
            }
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CornerObject"))
        {
            TextMeshPro textMeshPro = collision.gameObject.GetComponentInChildren<TextMeshPro>();

            if (textMeshPro.text == correctTranslation)
            {
                score++;
                UpdateScoreText();
                StartNewLevel(); // Move to the next level
            }
            else
            {
                wrongTranslationObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CornerObject"))
        {
            wrongTranslationObject.SetActive(false);
        }
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
}
