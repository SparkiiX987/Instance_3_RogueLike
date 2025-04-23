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
            if(PlayerPrefs.GetInt(i.ToString()) == 1)
            {
                achievementsPanel[i].color = Color.gray;
            }
        }
    }
}
