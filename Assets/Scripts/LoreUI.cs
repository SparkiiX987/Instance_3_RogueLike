using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoreUI : MonoBehaviour
{
    public List<LoreText> loreText = new List<LoreText>();
    public TMP_Text text;
    public TMP_Text text2;
    public TMP_Text text3;
    public GameObject lorePageUI;
    
    public void ShowLorePage()
    {
        lorePageUI.SetActive(true);
        int lorePage = PlayerPrefs.GetInt(Save.Instance.loreSaveKey);
        
        if (text.text != null)
            text.text = loreText[lorePage].text;
        text2.text = loreText[lorePage].text2;
        text3.text = loreText[lorePage].text3;
    }

    public void CloseLorePage()
    {
        lorePageUI.SetActive(false);

        if (PlayerPrefs.GetInt(Save.Instance.loreSaveKey) <= 8)
        {
            PlayerPrefs.SetInt(Save.Instance.loreSaveKey, PlayerPrefs.GetInt(Save.Instance.loreSaveKey) + 1);
            return;
        }
            
        PlayerPrefs.SetInt(Save.Instance.loreSaveKey, 0);
    }
}