using UnityEngine;

public class EmptyBottle : UsableObject
{
    public GameObject bottlePrefab;

    public EmptyBottle(int _price, string _name, string _description, GameObject _bottlePrefabs) : base(_price, _name, _description)
    {
        bottlePrefab = _bottlePrefabs;
    }
    
    public override void Action(GameObject _player)
    {
        AudioManager.Instance.PlaySound(AudioType.Throw);
        Instantiate(bottlePrefab, _player.transform.position, transform.rotation);
    }

}
