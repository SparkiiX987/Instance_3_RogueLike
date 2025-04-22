using UnityEngine;

public abstract class UsableObject : PickableObject
{
    public UsableObject(int _price, string _name, string _description, int _type) : base(_price, _name, _description, _type)
    {
    }
    
    public abstract void Action(GameObject _player);

}
