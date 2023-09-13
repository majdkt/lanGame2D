using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WordManager : MonoBehaviour
{
    public WordGenerator wordGenerator;
    public GameObject[] cornerObjects;
    public TextMeshPro germanWordText;

    private string correctTranslation;
    private string[] incorrectTranslations;

    private void Start()
    {
        AssignGermanWordToText();
        GenerateRandomWordAndTranslations();
        AssignTranslationsToCornerObjects();
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

    public string GetCorrectTranslation()
    {
        return correctTranslation;
    }

    public void HandleCorrectInput()
    {
        // Handle any additional logic for a correct input
    }

    public void HandleIncorrectInput()
    {
        // Handle any additional logic for an incorrect input
    }
}
