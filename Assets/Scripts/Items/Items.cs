[System.Serializable]
public class PickableObject : IPickableObject
{
    public int price;
    public string name;
    public string description;
    public int type;

    public PickableObject(int _price, string _name, string _description, int _type)
    {
        price = _price;
        name = _name;
        description = _description;
        type = _type;
    }

    public int GetPrice()
    {
        return price;
    }
}
