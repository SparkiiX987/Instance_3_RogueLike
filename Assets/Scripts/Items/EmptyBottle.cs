using UnityEngine;

public class EmptyBottle : UsableObject
{
    public GameObject bottlePrefab;

    public EmptyBottle(int _price, string _name, string _description, int _type, GameObject _bottlePrefabs) : base(_price, _name, _description, _type)
    {
        bottlePrefab = _bottlePrefabs;
    }
    
    public override void Action(GameObject _player)
    {
        AudioManager.Instance.PlaySound(AudioType.Throw);
        Object.Instantiate(bottlePrefab, _player.transform.position, _player.transform.rotation);
    }

}
