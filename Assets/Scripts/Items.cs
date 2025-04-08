using UnityEngine;
public class PickableObject : IPickableObject
{
    protected int price; 

    public int GetPrice()
    {
        return price;
    }
}
