using System.Collections;
using UnityEngine;

public class PepperSprayProjectile : MonoBehaviour
{
    [SerializeField] private float detonationTime;
    [SerializeField] private float cloudDuration;
    [SerializeField] private float cloudRadius;
    [SerializeField] private GameObject smoke;
    private bool hasDetonated = false;
    private float elapsedTime;
    private Rigidbody2D rb;
    private float launchStrength;
    private Ennemy monster;
    private RaycastHit2D hit;
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * launchStrength, ForceMode2D.Impulse);
    }
    
    private void Update()
    {
        if (!(elapsedTime >= detonationTime) || hasDetonated != false) return;
        StartCoroutine(TriggerCloud());
        hasDetonated = true;
        elapsedTime = 0;
    }

    private void OnCollisionEnter()
    {
        StartCoroutine(TriggerCloud());
        hasDetonated = true;
        elapsedTime = 0;
    }

    private IEnumerator TriggerCloud()
    {
        if (!(smoke.GetComponent<ParticleSystem>().main.duration >= cloudDuration))
        {
            Destroy(gameObject);
            yield break;
        }
        smoke.SetActive(true);
        smoke.GetComponent<ParticleSystem>().Play();
        hit = Physics2D.CircleCast(transform.position, cloudRadius,transform.up);
        //make the monster run away here
        yield return null;
    }
}
