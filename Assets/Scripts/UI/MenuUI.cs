using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public GameObject _panelBase;

    [Header("Texts")]
    public TMP_Text textJour;
    public TMP_Text textImpots;
    public TMP_Text textMoney;

    [Header("Impots")]
    [SerializeField] private List<Image> cases = new List<Image>();
    [SerializeField] private Color _color = Color.red;

    [Header("Achivements")]
    [SerializeField] private AchievementsManager achievementsManager;

    public Transform shopOfferPanel;

    [SerializeField] private Image itemHolded;
    [SerializeField] private List<GameObject> shopOfferPrefabs = new List<GameObject>();

    private void Start()
    {
        DayManager.Instance.IncrementDay();

        Shop.Instance.OnItemBuyed.AddListener(UpdateItem);

        if (DayManager.Instance.dayNumber == 10 && PlayerPrefs.GetInt("3") == 0)
        {
            achievementsManager.PlayAddAchievement(3);
        }
        if (DayManager.Instance.dayNumber == 12 && PlayerPrefs.GetInt("5") == 0)
        {
            achievementsManager.PlayAddAchievement(5);
        }
        UpdateDayUI();
        CaseImpots();
        UpdateImpots();
        UpdateMoney();

        for (int i = 0; i < shopOfferPrefabs.Count; i++)
        {
            GameObject shopOffer = Instantiate(shopOfferPrefabs[i], shopOfferPanel);
        }

    }

    public void UpdateDayUI()
    {
        textJour.text = "Jour : " + DayManager.Instance.dayNumber;
    }

    public void CaseImpots()
    {
        if (DayManager.Instance.dayRemaining == 1)
        {
            return;
        }

        for (int i = 0; i < cases.Count; i++)
        {
            if(i + 2 <= DayManager.Instance.dayRemaining)
            {
                cases[i].color = _color;
            }
        }
    }

    public void UpdateImpots()
    {
        textImpots.text = "Impots: \n" + PlayerMoney.Instance.GetCurrentImpots(DayManager.Instance.impotAdditions) + "$";
    }

    private void UpdateItem(Sprite _itemSprite)
    {
        UpdateMoney();
        itemHolded.sprite = _itemSprite;
    }

    public void UpdateMoney()
    {
        textMoney.text = "Cash : " + PlayerMoney.Instance.money + "$";
    }
}
