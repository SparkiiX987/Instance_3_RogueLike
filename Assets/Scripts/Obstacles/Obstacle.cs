using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;

public class Obstacle : MonoBehaviour, ITargetable
{
    [SerializeField] private int health;
    protected Enemy enemy;
    //[SerializeField] private Obstacles obstacleType;

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int _health)
    {
        health = _health;
    }

    public void TakeDamage(int _damage)
    {
        SetHealth(health - _damage);
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public virtual void Action()
    {
        //if (context.started)
        //{
        //    switch (obstacleType)
        //    {
        //        case Obstacles.WoodenPlankBin:
        //            {
        //                //Play animation
        //                //Play Sound
        //                transformObstacle.localScale = size;
        //                break;
        //            }
        //        case Obstacles.Table:
        //            {
        //                break;
        //            }
        //        case Obstacles.Food:
        //            {
        //                //Play Sound
        //                break;
        //            }
        //    }
        //}
    }

    //public enum Obstacles
    //{
    //    WoodenPlankBin,
    //    Table,
    //    Food
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy = collision.GetComponent<Enemy>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemy = null;
    }
}
