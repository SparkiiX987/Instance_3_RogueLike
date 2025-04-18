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
    
    public List<int> compAchievementList = new List<int>();
    
    private bool isPlaying = false;

    private async Awaitable AddAchievements(int achievementsID)
    {
        compAchievementList.Add(achievementsID);
        
        while (compAchievementList.Count > 0 && !isPlaying)
        {
            PlayerPrefs.SetInt(achievementsID.ToString(), 1);
            achievementPanel.SetActive(true);
            isPlaying = true;
            achievementTitle.text = achievementsTitles[compAchievementList[achievementsID]];
            achievementDesc.text = achievementsDescs[compAchievementList[achievementsID]];
            achievementImage.sprite = achievementsIcons[compAchievementList[achievementsID]];
            achievementPanel.transform.DOMoveY(990, 2f);
            await Awaitable.WaitForSecondsAsync(5f);
            achievementPanel.transform.DOMoveY(1230, 2f);
            await Awaitable.WaitForSecondsAsync(3f);
            achievementPanel.SetActive(false);
            isPlaying = false;
            compAchievementList.Remove(compAchievementList[achievementsID]);
        }
    }

    public async void PlayAddAchievement()
    {
        AddAchievements(0);
        await Awaitable.WaitForSecondsAsync(2f);
        AddAchievements(8);
    }
}