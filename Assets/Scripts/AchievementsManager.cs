using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsManager : MonoBehaviour
{
    [SerializeField] private GameObject achievementPanel;
    [SerializeField] private Image achievementImage;
    [SerializeField] private TextMeshProUGUI achievementTitle;
    [SerializeField] private TextMeshProUGUI achievementDesc;
    
    public List<Sprite> achievementsIcons = new List<Sprite>();
    public List<String> achievementsTitles = new List<String>();
    public List<String> achievementsDescs = new List<String>();
    
    private bool isPlaying = false;

    private async Awaitable AddAchievements(int achievementsID)
    {
        if (!isPlaying)
        {
            PlayerPrefs.SetInt(achievementsID.ToString(), 1);
            achievementPanel.SetActive(true);
            isPlaying = true;
            achievementTitle.text = achievementsTitles[achievementsID];
            achievementDesc.text = achievementsDescs[achievementsID];
            achievementImage.sprite = achievementsIcons[achievementsID];
            achievementPanel.transform.DOMoveY(990, 2f);
            await Awaitable.WaitForSecondsAsync(5f);
            achievementPanel.transform.DOMoveY(1230, 2f);
            await Awaitable.WaitForSecondsAsync(3f);
            achievementPanel.SetActive(false);
            isPlaying = false;
        }
    }

    public void PlayAddAchievement()
    {
        AddAchievements(1);
    }
}