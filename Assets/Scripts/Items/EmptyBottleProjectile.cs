using UnityEngine;

public class EmptyBottleProjectile : MonoBehaviour
{
    [SerializeField] float stunDuration;
    [SerializeField] private float launchStrength;
    [SerializeField] private float minSpeed;
    [SerializeField] private GameObject particleEffect;
    [SerializeField] private SpriteRenderer spriteRenderer;
    bool particleEffectActive = false;
    private Rigidbody2D rb;
    private Ennemy monster;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * launchStrength;
    }

    private void Update()
    {
        if (rb.linearVelocity.magnitude < minSpeed && particleEffectActive == false)
        {
            PlayParticleEffect();
        }
        if (!particleEffect)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        try
        {
            monster = other.collider.GetComponent<Ennemy>();
            monster.stunDuration = stunDuration;
            monster.isStunned = true;
            rb.linearVelocity = Vector2.zero;
            rb.freezeRotation = true;
        }
        catch { return; }
        PlayParticleEffect();
    }

    private void PlayParticleEffect()
    {
        particleEffectActive = true;
        spriteRenderer.enabled = false;
        particleEffect.GetComponent<ParticleSystem>().Play();
    }
}
