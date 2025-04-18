public class EmptyBottle : UsableObject
{
    [SerializeField] GameObject bottlePrefab;

    public EmptyBottle(int _price, string _name, string _description) : base(_price, _name, _description)
    {
    }
    
    public override void Action(GameObject _player)
    {
        AudioManager.Instance.PlaySound(AudioType.Throw);
        Instantiate(bottlePrefab, _player.transform.position, transform.rotation);
    }

}
