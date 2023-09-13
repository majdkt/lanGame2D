using UnityEngine;

[System.Serializable]
public class Translation
{
    public string text;
    public bool isCorrect;
}

[System.Serializable]
public class Word
{
    public string germanWord;
    public Translation correctTranslation;
    public Translation[] incorrectTranslations;
}

public class WordGenerator : MonoBehaviour
{
    public Word[] words;

    //Here change GenerateRandomWords
    public Word GenerateRandomWord()
    {
        int randomIndex = Random.Range(0, words.Length);
        return words[randomIndex];
    }
}