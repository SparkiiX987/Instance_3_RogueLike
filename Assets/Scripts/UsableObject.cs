public abstract class UsableObject : PickableObject
{
    public UsableObject(int _price, string _name, string _description) : base(_price, _name, _description)
    {
    }
    
    public abstract void Action(GameObject _player);

}
