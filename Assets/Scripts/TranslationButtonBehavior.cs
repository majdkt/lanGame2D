using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TranslationButtonBehavior : MonoBehaviour
{
    public WordManager wordManager; // Reference to the WordManager script
    public Image background; // Reference to the image background

    public string translationText; // The text on this button (translation)

    private Color correctColor = Color.green;

    private void Start()
    {
        // Add a click listener to the button
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        if (translationText == wordManager.GetCorrectTranslation())
        {
            background.color = correctColor;
            wordManager.HandleCorrectInput();
        }
        else
        {
            wordManager.HandleIncorrectInput();
        }
    }
}