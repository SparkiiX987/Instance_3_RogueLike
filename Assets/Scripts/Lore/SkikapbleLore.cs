using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkikapbleLore : MonoBehaviour
{
    [SerializeField] private GameObject IntroductionPanel;
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private List<string> texts;
    [SerializeField] private Button skipButton;
    [SerializeField] private Button nextButton;
    [Header("FadeInOut")]
    [SerializeField] private float fadeSpeed = 0.05f;

    private int currentIndex;
    private float maxTimeParagraph = 10f;
    private float maxTimeMeltedEffect = 1f;
    private float timer = 0f;
    private void Start()
    {
        skipButton.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();

        skipButton.onClick.AddListener(() => SkipIntroduction());
        nextButton.onClick.AddListener(() => NextText());

        NextText();
    }

    private void Update()
    {
        if (timer < maxTimeParagraph) { timer += Time.deltaTime; }
        else
        {
            NextText();
            timer = 0;
        }
    }

    private void NextText()
    {
        if (currentIndex != texts.Count) 
        {
            timer = 0;
            StartCoroutine(FadeInOut());
        }
        else
        {
            StartCoroutine(FadeInToMenu());
        }
    }

    private void SkipIntroduction()
    {
        StartCoroutine(FadeInToMenu());
    }

    IEnumerator FadeInOut()
    {
        Color currentColor = textBox.color;
        while (currentColor.a >= 0)
        {
            currentColor = currentColor - new Color(currentColor.r, currentColor.b, currentColor.g, currentColor.a - fadeSpeed);
            textBox.color -= currentColor;
            currentColor = textBox.color;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        if (currentIndex == texts.Count)
        {
            IntroductionPanel.SetActive(false);
        }
        textBox.text = texts[currentIndex];
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeInToMenu()
    {
        Color currentColor = textBox.color;
        while (currentColor.a >= 0)
        {
            currentColor = currentColor - new Color(currentColor.r, currentColor.b, currentColor.g, currentColor.a - fadeSpeed);
            textBox.color -= currentColor;
            currentColor = textBox.color;
            yield return new WaitForSecondsRealtime(0.02f);
        }
        IntroductionPanel.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        Color currentColor = textBox.color;
        while (currentColor.a <= 1f)
        {
            currentColor = currentColor + new Color(currentColor.r, currentColor.b, currentColor.g, currentColor.a + fadeSpeed);
            textBox.color = currentColor;
            currentColor = textBox.color;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        currentIndex++;
    }
}
