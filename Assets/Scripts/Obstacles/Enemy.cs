using UnityEngine;

public class Enemy : MonoBehaviour
{
    public State state;
    public ITargetable target;
    private RaycastHit2D[] hits;
    private int damage = 25;
    [SerializeField] private float attackRange;

    public enum State
    {
        Chase,
        Patrol,
        Idle,
        Attack
    }

    public void Attack()
    {
        if (target != null)
        {
            hits = Physics2D.CircleCastAll(transform.position, attackRange, transform.forward, 0f);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject != this.gameObject)
                {
                    if (hit.transform.gameObject.GetComponent<ITargetable>() == target)
                    {
                        float healthTarget = target.GetHealth();

                    }
                }
            }
        }
    }
}
