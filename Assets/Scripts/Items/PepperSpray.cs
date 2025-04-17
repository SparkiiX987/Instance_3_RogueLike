using UnityEngine;

public class PepperSpray : UsableObject
{
    [SerializeField] GameObject sprayPrefab;

    public override void Action(GameObject _player)
    {
        Instantiate(sprayPrefab, _player.transform.position, _player.transform.rotation);
    }
    
    // hit = Physics2D.BoxCast(player.transform.position, new Vector2(1, distance), player.transform.rotation.z, new Vector2(0, 1), distance/2);
    // if (hit.collider == null) return;
    // try
    // {
    //     monster = hit.collider.GetComponent<Ennemy>();
    //         
    // }
    // catch { /*ignored*/ }
}
