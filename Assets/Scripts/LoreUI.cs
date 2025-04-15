using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoreUI : MonoBehaviour
{
    public List<LoreText> loreText = new List<LoreText>();
    public TMP_Text text;
    public GameObject lorePageUI;
    public void ShowLorePage()
    {
        lorePageUI.SetActive(true);
        int lorePage = Save.Instance.GetInt("lorePage");
        Save.Instance.GetInt("lorePage");
        text.text = loreText[lorePage].text;
    }
}
