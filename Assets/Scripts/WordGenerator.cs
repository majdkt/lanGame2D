using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
    public string englishWord; 
    public Translation correctTranslation; 
    public List<Translation> incorrectTranslations = new List<Translation>();
}

[System.Serializable]
public class WordArray
{
    public string[] words;
}

public class WordGenerator : MonoBehaviour
{
    private const string RANDOM_WORD_URL = "https://random-word-api.herokuapp.com/word?length=3";
    private const string TRANSLATE_URL = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=de&dt=t&q=";

    public void GenerateRandomWord(System.Action<Word> callback)
    {
        StartCoroutine(FetchWordAndTranslation(callback));
    }

    private IEnumerator FetchWordAndTranslation(System.Action<Word> callback)
    {
        Word word = new Word();

        using (UnityWebRequest wordRequest = UnityWebRequest.Get(RANDOM_WORD_URL))
        {
            yield return wordRequest.SendWebRequest();

            if (wordRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch random word: " + wordRequest.error);
                yield break;
            }

            WordArray wordArray = JsonUtility.FromJson<WordArray>("{\"words\":" + wordRequest.downloadHandler.text + "}");
            word.englishWord = wordArray.words[0];

            using (UnityWebRequest translationRequest = UnityWebRequest.Get(TRANSLATE_URL + word.englishWord))
            {
                yield return translationRequest.SendWebRequest();

                if (translationRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Failed to translate word: " + translationRequest.error);
                    yield break;
                }

                string jsonResponse = translationRequest.downloadHandler.text;
                int firstQuote = jsonResponse.IndexOf("\"") + 1;
                int secondQuote = jsonResponse.IndexOf("\"", firstQuote);
                string translatedWord = jsonResponse.Substring(firstQuote, secondQuote - firstQuote);

                word.correctTranslation = new Translation
                {
                    text = translatedWord,
                    isCorrect = true
                };
            }
        }

        for (int i = 0; i < 3; i++)
        {
            using (UnityWebRequest randomWordRequest = UnityWebRequest.Get(RANDOM_WORD_URL))
            {
                yield return randomWordRequest.SendWebRequest();

                if (randomWordRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Failed to fetch random word for random translation: " + randomWordRequest.error);
                    yield break;
                }

                WordArray randomWordArray = JsonUtility.FromJson<WordArray>("{\"words\":" + randomWordRequest.downloadHandler.text + "}");
                string randomEnglishWord = randomWordArray.words[0];

                using (UnityWebRequest translationRequest = UnityWebRequest.Get(TRANSLATE_URL + randomEnglishWord))
                {
                    yield return translationRequest.SendWebRequest();

                    if (translationRequest.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Failed to translate random word: " + translationRequest.error);
                        yield break;
                    }

                    string jsonResponse = translationRequest.downloadHandler.text;
                    int firstQuote = jsonResponse.IndexOf("\"") + 1;
                    int secondQuote = jsonResponse.IndexOf("\"", firstQuote);
                    string randomTranslatedWord = jsonResponse.Substring(firstQuote, secondQuote - firstQuote);

                    word.incorrectTranslations.Add(new Translation
                    {
                        text = randomTranslatedWord,
                        isCorrect = false
                    });
                }
            }
        }

        callback?.Invoke(word);
    }
}