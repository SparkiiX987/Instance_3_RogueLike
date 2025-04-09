using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayUpdater : MonoBehaviour
{
    public GameObject _panelBase;
    public TMP_Text textJour;
    public TMP_Text textImpots;

    [SerializeField] private List<Image> cases = new List<Image>();
   
   

    [SerializeField] private Color _color = Color.red;

    

    private void Start()
    {
        DayManager.Instance.IncrementDay();
        UpdateDayUI();
        CaseImpots();
        UpdateImpots();
    }

    public void UpdateDayUI()
    {
        textJour.text = string.Empty;
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
        textImpots.text = string.Empty;
        textImpots.text = "Impôts : " + PlayerMoney.Instance.GetCurrentImpots(DayManager.Instance.impotAdditions) + "$";
    }
}
