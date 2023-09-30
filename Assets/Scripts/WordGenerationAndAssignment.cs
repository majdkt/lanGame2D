using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordGenerationAndAssignment : MonoBehaviour
{
    public TextMeshPro englishWordDisplay;
    public GameObject[] cornerObjects;
    public WordGenerator wordGenerator; // Reference to the WordGenerator
    internal string correctTranslation;
    public UIManager uiManager; // Reference to the UIManager
    public UIManager2 uiManager2;
    
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
        List<string> cornerWords = new List<string>();

        // Always include the correct translation
        cornerWords.Add(correctTranslation);

        // Add incorrect translations if available
        for (int i = 0; i < cornerObjects.Length-1; i++)
        {
            cornerWords.Add(randomWord.incorrectTranslations[i].text);
        }

        // Shuffle the cornerWords list
        ShuffleList(cornerWords);

        // Assign the shuffled translations to cornerObjects
        for (int i = 0; i < cornerObjects.Length; i++)
        {
            TextMeshPro textMeshPro = cornerObjects[i].GetComponentInChildren<TextMeshPro>();
        
            if (cornerWords.Count > 0)
            {
                textMeshPro.text = cornerWords[0];
                cornerWords.RemoveAt(0);
            }
 
        }
    }

// Shuffle the list 
    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
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
    
    // For level 2
    public void HandleChoice2(string chosenTranslation)
    {
        if (chosenTranslation == correctTranslation)
        {
            uiManager2.HandleInteraction(true); 
            GenerateAndAssignWord(); 
        }
        else
        {
            uiManager2.HandleInteraction(false);
        }
    }
}