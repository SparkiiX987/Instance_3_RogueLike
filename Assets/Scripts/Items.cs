using UnityEngine;
[System.Serializable]
public class PickableObject : IPickableObject
{
    public int price;
    public string name;
    public string description;

    public PickableObject(int _price, string _name, string _description)
    {
        price = _price;
        name = _name;
        description = _description;
    }

    public int GetPrice()
    {
        return price;
    }
}
