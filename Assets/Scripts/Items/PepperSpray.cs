public class PepperSpray : UsableObject
{
    [SerializeField] GameObject sprayPrefab;

    public PepperSpray(int _price, string _name, string _description) : base(_price, _name, _description)
    {
    }  

    public override void Action()
    {
        AudioManager.Instance.PlaySound(AudioType.lacrymogene);
        Instantiate(sprayPrefab, _player.transform.position, _player.transform.rotation);
    }
}
