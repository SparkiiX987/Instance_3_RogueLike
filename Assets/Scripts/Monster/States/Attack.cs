using Unity.VisualScripting;
using UnityEngine;

public class Attack : IState
{
    //Target
    public ITargetable target;
    //Enemy
    public Ennemy enemy;

    private float attackRange = 1.5f;
    private int damage = 25; // remplace with stats.damage (I suppopse)


    public void Action()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(enemy.transform.position, attackRange, enemy.transform.up, 0f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject != enemy.transform.gameObject)
            {
                Obstacle obstacle = hit.transform.GetComponent<FoodObstacle>();
                if (hit.transform.gameObject.GetComponent<ITargetable>() == target && !obstacle)
                {
                    int healthTarget = target.GetHealth();
                    healthTarget -= damage;
                    target.SetHealth(healthTarget);
                }
            }
        }
    }
}
