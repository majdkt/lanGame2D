using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public WordGenerator wordGenerator;
    public GameObject[] cornerObjects;
    public TextMeshPro germanWordText;
    public TextMeshPro scoreText; // Reference to the TextMeshProUGUI component for displaying the score
    public GameObject gameOverObject; // Reference to the game object to activate on incorrect answer


    private string correctTranslation;
    private string[] incorrectTranslations;

    private Color defaultColor = Color.white;
    public Color correctColor;
    public Color incorrectColor;

    private Renderer correctRenderer;
    private int score = 0; // Score variable to keep track of correct answers

    private void Start()
    {
        AssignGermanWordToText(); // No need to generate a random word here, as it is already generated in GenerateRandomWordAndTranslations()
        GenerateRandomWordAndTranslations();
        AssignTranslationsToCornerObjects();
        UpdateScoreText(); // Display initial score
    }

    private void GenerateRandomWordAndTranslations()
    {
        Word randomWord = wordGenerator.GenerateRandomWord();

        // Get the German word
        string germanWord = randomWord.germanWord;
        germanWordText.text = germanWord;

        // Get the correct translation
        correctTranslation = randomWord.correctTranslation.text;

        // Get the incorrect translations
        Translation[] translations = randomWord.incorrectTranslations;
        incorrectTranslations = new string[cornerObjects.Length - 1];

        int correctIndex = Random.Range(0, cornerObjects.Length - 1);
        int incorrectIndex = 0;

        // Assign translations to correct and incorrect arrays
        for (int i = 0; i < translations.Length; i++)
        {
            if (i == correctIndex)
            {
                // Shuffle the corner objects to randomize the correct translation position
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

        // Shuffle the incorrect translations array
        ShuffleArray(incorrectTranslations);
    }

    private void AssignTranslationsToCornerObjects()
    {
        int incorrectIndex = 0;

        // Assign translations to the corner objects
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the moving object collided with the correct option
        if (collision.gameObject.CompareTag("CornerObject"))
        {
            Renderer renderer = collision.gameObject.GetComponent<Renderer>();
            if (renderer == correctRenderer)
            {
                // Change the color to correctColor if the answer is correct
                renderer.material.color = correctColor;
                IncreaseScore(); // Increment the score
                RestartGame(); // Restart the game with a new word and translations
            }
            else
            {
                // Change the color to incorrectColor if the answer is incorrect
                renderer.material.color = incorrectColor;
                ActivateGameOverObject(); // Activate the game over object on incorrect answer
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Reset the color of the collided corner object to the default color
        if (collision.gameObject.CompareTag("CornerObject"))
        {
            Renderer renderer = collision.gameObject.GetComponent<Renderer>();
            renderer.material.color = defaultColor;
        }
    }
    private void IncreaseScore()
    {
        score++;
        UpdateScoreText(); // Update the score text after increasing the score
    }
    private void RestartGame()
    {
        // Generate new word and translations
        AssignGermanWordToText();
        GenerateRandomWordAndTranslations();
        AssignTranslationsToCornerObjects();
    }
    private void ActivateGameOverObject()
    {
        gameOverObject.SetActive(true);
    }
    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    
    
}
