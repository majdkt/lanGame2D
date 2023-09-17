using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FontControl : MonoBehaviour
{
    [SerializeField] private TextFitMode textFitMode = TextFitMode.FillBoth;
    [SerializeField] private float minFontSize = 10f;
    [SerializeField] private float maxFontSize = 100f;

    private void Start()
    {
        TextMeshPro[] textMeshPros = FindObjectsOfType<TextMeshPro>();

        foreach (TextMeshPro textMeshPro in textMeshPros)
        {
            Debug.Log("Found TextMeshPro: " + textMeshPro.name);
            AutoSizeTextToFitRectTransform(textMeshPro);
        }
    }

    private void AutoSizeTextMeshProComponents()
    {
        TextMeshPro[] textMeshPros = FindObjectsOfType<TextMeshPro>();

        foreach (TextMeshPro textMeshPro in textMeshPros)
        {
            AutoSizeTextToFitRectTransform(textMeshPro);
        }
    }

    private void AutoSizeTextToFitRectTransform(TextMeshPro textMeshPro)
    {
        if (textMeshPro == null)
        {
            return;
        }

        RectTransform rectTransform = textMeshPro.rectTransform;
        float availableWidth = rectTransform.rect.width;
        float availableHeight = rectTransform.rect.height;

        float textWidth = textMeshPro.preferredWidth;
        float textHeight = textMeshPro.preferredHeight;

        float widthRatio = availableWidth / textWidth;
        float heightRatio = availableHeight / textHeight;

        float scaleFactor;

        switch (textFitMode)
        {
            case TextFitMode.FillWidth:
                scaleFactor = widthRatio;
                break;
            case TextFitMode.FillHeight:
                scaleFactor = heightRatio;
                break;
            case TextFitMode.FillBoth:
            default:
                scaleFactor = Mathf.Min(widthRatio, heightRatio);
                break;
        }

        scaleFactor = Mathf.Clamp(scaleFactor, minFontSize / textMeshPro.fontSize, maxFontSize / textMeshPro.fontSize);

        textMeshPro.fontSize *= scaleFactor;
    }
}

public enum TextFitMode
{
    FillWidth,
    FillHeight,
    FillBoth
}