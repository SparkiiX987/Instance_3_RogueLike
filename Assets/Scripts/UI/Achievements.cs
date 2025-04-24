using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{
    [SerializeField] private List<Image> achievementsPanel = new List<Image>();

    private void Start()
    {
        ActiveUnlockedAchievements();
    }

    public void ActiveUnlockedAchievements()
    {
        for(int i = 0; i < achievementsPanel.Count; i++)
        {
            if(PlayerPrefs.GetInt((i + 1).ToString()) == 0)
            {
                achievementsPanel[i].color = Color.gray;
            }
        }
    }
}
