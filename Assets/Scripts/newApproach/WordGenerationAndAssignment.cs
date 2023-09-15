using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordGenerationAndAssignment : MonoBehaviour
{
    public TextMeshPro englishWordDisplay;
    public GameObject[] cornerObjects;
    public WordGenerator wordGenerator; // Reference to the WordGenerator

    private string correctTranslation;
    public UIManager uiManager; // Reference to the UIManager

    private void Start()
    {
        GenerateAndAssignWord();
    }

    public void GenerateAndAssignWord()
    {
        wordGenerator.GenerateRandomWord((word) => {
            englishWordDisplay.text = word.englishWord;
            correctTranslation = word.correctTranslation.text;
            AssignTranslationsToCorners(word);
        });
    }

    private void AssignTranslationsToCorners(Word randomWord)
    {
        List<string> cornerWords = new List<string> 
        {
            correctTranslation,
            randomWord.incorrectTranslations[0].text,
            randomWord.incorrectTranslations[1].text,
            randomWord.incorrectTranslations[2].text
        };

        for (int i = 0; i < cornerObjects.Length; i++)
        {
            TextMeshPro textMeshPro = cornerObjects[i].GetComponentInChildren<TextMeshPro>();
            int randomIndex = Random.Range(0, cornerWords.Count);
            textMeshPro.text = cornerWords[randomIndex];
            cornerWords.RemoveAt(randomIndex);
        }
    }

    // Call this method when the player makes a choice
    public void HandleChoice(string chosenTranslation)
    {
        if (chosenTranslation == correctTranslation)
        {
            uiManager.HandleInteraction(true); // Call UIManager's HandleInteraction with true
            GenerateAndAssignWord(); // Generate the next set of words
        }
        else
        {
            uiManager.HandleInteraction(false); // Call UIManager's HandleInteraction with false
        }
    }
    
}
