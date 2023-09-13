using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public WordGenerator wordGenerator;
    public GameObject[] cornerObjects;
    public TextMeshPro scoreText;
    public GameObject gameOverObject;
    public GameObject gameGameObject;
    public Image background;

    private string correctTranslation;
    private bool isGameOver = false;
    private Color defaultColor = Color.white;
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;

    private int score = 0;

    private void Start()
    {
        wordGenerator.GenerateRandomWord((word) => {
            AssignWordAndTranslationsToCorners(word);
            UpdateScoreText();
            StartBlinkingBackground();
        });
    }

 private void AssignWordAndTranslationsToCorners(Word randomWord)
{
    string englishWord = randomWord.englishWord;


    correctTranslation = randomWord.correctTranslation.text;
    string randomTranslation = randomWord.randomTranslation.text;

    List<int> availablePositions = new List<int> { 0, 1, 2, 3 }; // Assuming 4 corners
    int englishWordPosition = Random.Range(0, availablePositions.Count);
    availablePositions.RemoveAt(englishWordPosition);

    int germanWordPosition = availablePositions[Random.Range(0, availablePositions.Count)];
    availablePositions.Remove(germanWordPosition);

    int randomTranslationPosition = availablePositions[Random.Range(0, availablePositions.Count)];

    for (int i = 0; i < cornerObjects.Length; i++)
    {
        TextMeshPro textMeshPro = cornerObjects[i].GetComponentInChildren<TextMeshPro>();

        if (i == englishWordPosition)
        {
            textMeshPro.text = englishWord;

        }
        else if (i == germanWordPosition)
        {
            textMeshPro.text = correctTranslation;
        }
        else if (i == randomTranslationPosition)
        {
            textMeshPro.text = randomTranslation;
        }
        else
        {
            textMeshPro.text = correctTranslation; // Duplicates for remaining corners
        }
    }
}



    public bool IsCorrectInteraction(GameObject cornerObject)
    {
        TextMeshPro textMeshPro = cornerObject.GetComponentInChildren<TextMeshPro>();
        return textMeshPro.text == correctTranslation;
    }

    public void StartBlinkingBackground()
    {
        StartCoroutine(BlinkBackground());
    }

    private IEnumerator BlinkBackground()
    {
        while (!isGameOver)
        {
            background.color = Color.white;
            yield return new WaitForSeconds(0.5f);
            background.color = Color.black;
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void HandleInteraction(bool isCorrect)
    {
        if (isCorrect)
        {
            IncreaseScore();
            background.color = correctColor;
            StartCoroutine(ResetBackgroundColorAfterDelay(1.0f, defaultColor));
            wordGenerator.GenerateRandomWord((word) => {
                AssignWordAndTranslationsToCorners(word);
            });
        }
        else
        {
            background.color = incorrectColor;
            gameOverObject.SetActive(true);
            gameGameObject.SetActive(false);
            isGameOver = true;
        }
    }

    private IEnumerator ResetBackgroundColorAfterDelay(float delay, Color targetColor)
    {
        yield return new WaitForSeconds(delay);
        background.color = targetColor;
    }

    private void IncreaseScore()
    {
        score++;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}
