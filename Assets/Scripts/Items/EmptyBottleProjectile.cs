using System;
using UnityEngine;

public class EmptyBottleProjectile : MonoBehaviour
{
    [SerializeField] float stunDuration;
    [SerializeField] private float launchStrength;
    private Rigidbody2D rb;
    private Ennemy monster;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * launchStrength, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (rb.linearVelocity.magnitude < 0.2f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        try
        {
            monster = other.collider.GetComponent<Ennemy>();
            monster.stunDuration = stunDuration;
            monster.isStunned = true;
            Destroy(gameObject);
        }
        catch { /*ignored*/ }
    }
}
