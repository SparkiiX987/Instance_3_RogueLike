using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public int money;
    private int impotsIncrement = 50;
    public int impots = 200;

    public static PlayerMoney Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public int GetMoney()
    {
        return money;
    }

    public void AddMoney(int _amount)
    {
        money += _amount;
    }

    public int GetCurrentImpots(int _additionFactor)
    {
        return impots + _additionFactor * impotsIncrement;
    }
}
