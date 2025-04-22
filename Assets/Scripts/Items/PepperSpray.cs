using UnityEngine;

public class PepperSpray : UsableObject
{
    public GameObject sprayPrefab;

    public PepperSpray(int _price, string _name, string _description, int _type, GameObject _sprayPrefabs) : base(_price, _name, _description, _type)
    {
        sprayPrefab = _sprayPrefabs;
    }  

    public override void Action(GameObject _player)
    {
        AudioManager.Instance.PlaySound(AudioType.lacrymogene);
        PepperSprayProjectile pepperSpray = (Object.Instantiate(sprayPrefab, _player.transform.position, _player.transform.rotation)).GetComponent<PepperSprayProjectile>();
        pepperSpray.GetComponent<Collider2D>().enabled = false;
    }
}
