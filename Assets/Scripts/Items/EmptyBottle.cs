using UnityEngine;

public class EmptyBottle : UsableObject
{
    [SerializeField] GameObject bottlePrefab;
    public override void Action(GameObject _player)
    {
        Instantiate(bottlePrefab, _player.transform.position, transform.rotation);
    }
}
