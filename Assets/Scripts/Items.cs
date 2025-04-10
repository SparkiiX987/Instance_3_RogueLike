using UnityEngine;
[System.Serializable]
public class PickableObject : IPickableObject
{
    public int price;
    public string name;
    public string description;

    public int GetPrice()
    {
        return price;
    }
}
