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
        if (PlayerPrefs.GetInt(achievementsID.ToString()) == 1)
            return;
        
        compAchievementList.Add(achievementsID);
        PlayerPrefs.SetInt(achievementsID.ToString(), 1);
        
        while (compAchievementList.Count > 0 && !isPlaying)
        {
            achievementPanel.SetActive(true);
            isPlaying = true;
            achievementTitle.text = achievementsTitles[compAchievementList[0]];
            achievementDesc.text = achievementsDescs[compAchievementList[0]];
            achievementImage.sprite = achievementsIcons[compAchievementList[0]];
            achievementPanel.transform.DOMoveY(990, 2f);
            await Awaitable.WaitForSecondsAsync(5f);
            achievementPanel.transform.DOMoveY(1230, 2f);
            await Awaitable.WaitForSecondsAsync(3f);
            achievementPanel.SetActive(false);
            isPlaying = false;
            compAchievementList.Remove(compAchievementList[0]);
        }
    }

    public void PlayAddAchievement()
    {
        AddAchievements(0);
        AddAchievements(8);
    }
    
    public void DEL()
    {
        PlayerPrefs.DeleteAll();
    }
}