using UnityEngine;

public class PepperSpray : UsableObject
{
    public GameObject sprayPrefab;

    public PepperSpray(int _price, string _name, string _description, GameObject _sprayPrefabs) : base(_price, _name, _description)
    {
        sprayPrefab = _sprayPrefabs;
    }  

    public override void Action(GameObject _player)
    {
        AudioManager.Instance.PlaySound(AudioType.lacrymogene);
        Instantiate(sprayPrefab, _player.transform.position, _player.transform.rotation);
    }
}
