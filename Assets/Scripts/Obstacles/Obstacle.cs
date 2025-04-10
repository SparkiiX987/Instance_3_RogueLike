using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class Obstacle : MonoBehaviour, ITargetable
{
    [SerializeField] private int health;

    protected Ennemy enemy;
    protected PlayerControl player;

    public bool activated;

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int _health)
    {
        health = _health;

        if (health <= 0)
        {
            //Play animation destruction
            Destroy(gameObject);
        }
    }

    public virtual void Action()
    {

    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemy == null) 
            enemy = collision.GetComponent<Ennemy>();

        if (player == null)
        player = collision.GetComponent<PlayerControl>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemy = null;
        player = null;
    }
}
