using System.Collections;
using UnityEngine;

public class PepperSprayProjectile : MonoBehaviour
{
    [SerializeField] float stunDuration;
    [SerializeField] private float detonationTime;
    [SerializeField] private float cloudRadius;
    [SerializeField] private GameObject smoke;
    [SerializeField] private float launchStrength;
    private bool hasDetonated = false;
    private float elapsedTime;
    private Rigidbody2D rb;
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * launchStrength, ForceMode2D.Impulse);
    }
    
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < detonationTime) return;
        if (hasDetonated == false)
        {
            smoke.GetComponent<ParticleSystem>().Play();
            GetComponent<Collider2D>().enabled = false;
            hasDetonated = true;
            elapsedTime = 0;
            StartCoroutine(CloudColission());
        }
        else
        {
            CircleCollider2D circle = GetComponent<CircleCollider2D>();
            if (circle == null)
            {
                gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
                gameObject.GetComponent<CircleCollider2D>().radius = cloudRadius * 4f;
            }
        }
        
    }

    private IEnumerator CloudColission()
    {
        if (!smoke)
        {
            CircleCollider2D circle = GetComponent<CircleCollider2D>();
            Destroy(gameObject);
            yield break;
        }
        //make the monster run away here
        yield return null;
        StartCoroutine(CloudColission());
    } 

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, cloudRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ennemy enemy = collision.GetComponent<Ennemy>();
        if (enemy != null && !enemy.isStunned && hasDetonated)
        {
            enemy.isStunned = true;
            enemy.stunDuration = stunDuration;
        }
    }
}
