using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private Ennemy ennemy;
    private Transform playerTransform;
    private Transform selfTransform;
    private RaycastHit2D hit;
    private Vector2 dir = Vector2.zero;

    [SerializeField] private float detectionCooldown;
    private float actualCooldown;

    [SerializeField] private string playerTag;


    private void Awake()
    {
        ennemy = GetComponentInParent<Ennemy>();
        playerTransform = transform.parent.parent.GetChild(0);
        selfTransform = transform;
    }

    private void GetPlayerDIrection()
    {
        dir.Set(playerTransform.position.x - selfTransform.position.x, playerTransform.position.y - selfTransform.position.y);
        dir.Normalize();
        dir *= 1.5f;
    }

    private void DetectPlayer()
    {
        hit = Physics2D.Raycast(selfTransform.position, dir, ennemy.detectionRange);
        Debug.DrawRay(selfTransform.position, dir, Color.red, detectionCooldown);
        if (!hit || hit.collider.tag != playerTag) { ennemy.targetPlayer = null;  return; }

        ennemy.targetPlayer = hit.collider.GetComponent<PlayerControl>();
        ennemy.ChangeState("Chase");
        ennemy.SetPlayerPosition(hit.collider.transform.position);
        print("player is at " + hit.collider.transform.position);
    }

    private void DecrementTimer()
    {
        actualCooldown -= Time.deltaTime;
        if(actualCooldown < 0) { actualCooldown = 0; }
    }

    private void Update()
    {
        if(actualCooldown > 0)
        {
            DecrementTimer();
        }
        else
        {
            actualCooldown = detectionCooldown;
            GetPlayerDIrection();
            DetectPlayer();
        }
    }
}
