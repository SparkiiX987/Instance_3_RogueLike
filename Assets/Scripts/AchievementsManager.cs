using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsManager : MonoBehaviour
{
    [SerializeField] private GameObject achievementPanel;
    
    public List<Sprite> achievementsIcons = new List<Sprite>();
    public List<String> achievementsTitles = new List<String>();
    public List<String> achievementsDescs = new List<String>();
    
    private Image achievementImage;
    private TextMeshProUGUI achievementTitle;
    private TextMeshProUGUI achievementDesc;
    
    private void Start()
    {
        achievementImage = achievementPanel.GetComponentInChildren<Image>();
        achievementTitle = achievementPanel.GetComponentInChildren<TextMeshProUGUI>();
        achievementDesc = achievementPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    private async void AddAchievements(int achievementsID)
    {
        PlayerPrefs.SetInt(achievementsID.ToString(), 1);
        achievementPanel.SetActive(true);
        achievementTitle.text = achievementsTitles[achievementsID];
        achievementDesc.text = achievementsDescs[achievementsID];
        achievementImage.sprite = achievementsIcons[achievementsID];
        achievementPanel.transform.DOMoveY(990, 2f);
        await Awaitable.WaitForSecondsAsync(5f);
        achievementPanel.transform.DOMoveY(1230, 2f);
        await Awaitable.WaitForSecondsAsync(3f);
        achievementPanel.SetActive(false);
    }

    public void PlayAddAchievement()
    {
        AddAchievements(1);
    }
}