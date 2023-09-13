using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TranslationGameManager : MonoBehaviour
{
    public TextMeshPro germanWordText;
    public Button correctTranslationButton;
    public Button randomTranslationButton;
    public TextMeshPro resultText;

    private Dictionary<string, string> wordDictionary = new Dictionary<string, string>();
    private string currentGermanWord;
    private string correctTranslation;

    private void Start()
    {
        // Populate the dictionary (as shown in the previous response)
        // Make sure the wordDictionary is filled with German and English translations

        // Start the game
        StartNewRound();
    }

    private void StartNewRound()
    {
        // Select a random German word from the dictionary
        List<string> germanWords = new List<string>(wordDictionary.Keys);
        currentGermanWord = germanWords[Random.Range(0, germanWords.Count)];
        correctTranslation = wordDictionary[currentGermanWord];

        // Set the German word text
        germanWordText.text = currentGermanWord;

        // Randomly decide which button will have the correct translation
        bool isCorrectOnLeft = Random.Range(0, 2) == 0;

        if (isCorrectOnLeft)
        {
            correctTranslationButton.GetComponentInChildren<TextMeshProUGUI>().text = correctTranslation;
            randomTranslationButton.GetComponentInChildren<TextMeshProUGUI>().text = GetRandomTranslation();
        }
        else
        {
            randomTranslationButton.GetComponentInChildren<TextMeshProUGUI>().text = correctTranslation;
            correctTranslationButton.GetComponentInChildren<TextMeshProUGUI>().text = GetRandomTranslation();
        }
    }

    private string GetRandomTranslation()
    {
        List<string> translations = new List<string>(wordDictionary.Values);
        translations.Remove(correctTranslation);
        return translations[Random.Range(0, translations.Count)];
    }

    public void CheckTranslation(bool isCorrect)
    {
        if (isCorrect)
        {
            resultText.text = "Correct!";
        }
        else
        {
            resultText.text = "Incorrect.";
        }

        // Start a new round after a short delay
        Invoke("StartNewRound", 1.5f);
    }
}
