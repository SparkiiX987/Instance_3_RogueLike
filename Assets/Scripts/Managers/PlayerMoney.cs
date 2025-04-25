using DG.Tweening.Core.Easing;
using UnityEngine;


public class PlayerMoney : MonoBehaviour
{
    public int money;
    private int impotsIncrement = 50;
    public int impots = 200;

    public static PlayerMoney Instance;
    public Save save => Save.Instance;

    public bool gotSavedInfos = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        GetMoneyFromSave();
    }

    public void GetMoneyFromSave()
    {
        gotSavedInfos = false;
        if (PlayerPrefs.HasKey(save.moneySaveKey))
        {
            money = save.GetMoney();
        }
        else
        {
            money = 100;
            save.SaveMoney(money);
        }
        gotSavedInfos = true;
    }

    public int GetMoney()
    {
        return money;
    }

    public void AddMoney(int _amount)
    {
        money += _amount;
        save.SaveMoney(money);
    }

    public int GetCurrentImpots(int _additionFactor)
    {
        return impots + _additionFactor * impotsIncrement;
    }
}
