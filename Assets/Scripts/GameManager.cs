using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshPro englishWordDisplay;
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
        englishWordDisplay.text = randomWord.englishWord;

        correctTranslation = randomWord.correctTranslation.text;

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