using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayUpdater : MonoBehaviour
{
    public GameObject _panelBase;

    [Header("Texts")]
    public TMP_Text textJour;
    public TMP_Text textImpots;

    [Header("Impots")]
    [SerializeField] private List<Image> cases = new List<Image>();
    [SerializeField] private Color _color = Color.red;

    public Transform shopOfferPanel;

    [SerializeField] private List<GameObject> shopOfferPrefabs = new List<GameObject>();

    private void Start()
    {
        DayManager.Instance.IncrementDay();
        UpdateDayUI();
        CaseImpots();
        UpdateImpots();

        for(int i = 0; i < shopOfferPrefabs.Count; i++)
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
}
