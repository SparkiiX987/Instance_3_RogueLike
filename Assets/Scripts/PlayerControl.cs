using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour, ITargetable
{
    [SerializeField] private float rotationSpeed;
    
    [SerializeField] private float staminaMax;

    [SerializeField] private float cooldown;

    [SerializeField] private float pickupDistance;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private LayerMask obstacleLayer;
    
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private bool isDetectable;
    private int health;
    private float stamina;
    private bool isRunning;
    private Rigidbody2D rb;
    
    private Ray ray;
    private RaycastHit2D hit;

    private Stats stats;
    public SellableObject sellableObject;
    public UsableObject usableObject;
    public CollectableItem collectableObject;
    
    private float currentCooldown;
    
    private InputAction moveAction;
    private InputAction lookAction;

    private Vector2 movementDir;
    private Vector2 nextPlayerPos;
    private Vector2 dir;
    private Transform playerTransform;
    
    private void Awake()
    {
        stats = GetComponent<Stats>();
        ray = new Ray(transform.position, transform.forward);
        playerTransform = GetComponent<Transform>();
        collectableObject = GetComponent<CollectableItem>();
    }

    private void Start()
    {
        stamina = staminaMax;
        hit = Physics2D.Raycast(playerTransform.position, Vector2.up, 100f);
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
        
        LookAtMouse();

        if(currentCooldown >= 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        else
        {
            currentCooldown = cooldown;
            if (IsUsingStamina())
            {
                LosingStamina();
                animator.SetBool("IsRunning", true);
            }
            else
            {
                
                StaminaRegen();
                animator.SetBool("IsRunning", false);
            }
                
        }
    }

    private void FixedUpdate()
    {
        nextPlayerPos.Set(playerTransform.position.x + movementDir.x * stats.speed * Time.deltaTime, playerTransform.position.y + movementDir.y * stats.speed * Time.deltaTime);
        rb.MovePosition(nextPlayerPos);
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int _health)
    {
        health = _health;
    }

    public void Movement(InputAction.CallbackContext _ctx)
    {
        movementDir = _ctx.ReadValue<Vector2>();
        animator.SetBool("IsWalkingBool", true);
        
        if (_ctx.canceled)
            animator.SetBool("IsWalkingBool", false);
    }
    
    private void LookAtMouse()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(playerTransform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
    
    public void PickUp(InputAction.CallbackContext _ctx)
    {
        if (_ctx.started)
        {
            hit = Physics2D.Raycast(playerTransform.position, GetMousePosition(), pickupDistance, itemLayer);
            if (hit.collider!= null && hit.collider.transform.GetComponent<CollectableItem>()) 
            {
                if (hit.collider.transform.GetComponent<CollectableItem>().item.GetType() == typeof(SellableObject))
                {
                    sellableObject = (SellableObject)hit.collider.transform.GetComponent<CollectableItem>().item;
                }
                
                else if (hit.collider.transform.GetComponent<CollectableItem>().item.GetType() == typeof(UsableObject))
                {
                    usableObject = (UsableObject)hit.collider.transform.GetComponent<CollectableItem>().item;
                }
                animator.SetTrigger("IsPickingUpItem");
                Destroy(hit.collider.gameObject);
            }
        }
    }

    public void UseItem(InputAction.CallbackContext _ctx)
    {
        if (_ctx.started)
        {
            hit = Physics2D.Raycast(playerTransform.position, GetMousePosition(), pickupDistance, obstacleLayer);
            
            if (usableObject != null)
            {
                usableObject.Action();
            
                if (usableObject.GetType() == typeof(EmptyBottle))
                {
                    animator.SetTrigger("IsThrowingItem");
                }
            
                if (usableObject.GetType() == typeof(MonsterCan))
                {
                    animator.SetTrigger("IsDrinkingItem");
                }
            }
            
            else if (hit.collider != null && hit.collider.transform.GetComponent<Obstacle>())
            {
                Obstacle obstacleObject = hit.collider.transform.GetComponent<Obstacle>();
                obstacleObject.Action();
            }
            
        }
    }
    
    public void Sprint(InputAction.CallbackContext _ctx)
    {
        float walkingSpeed = 5f;

        if (_ctx.performed && stamina > 0)
        {
            stats.speed = stats.speed * 2f;
            isRunning = true;
        }

        else
        {
            stats.speed = walkingSpeed;
            isRunning = false;
        }
    }

    private void StaminaRegen()
    {
        if (stamina >= staminaMax)
        {
            stamina = staminaMax;
            return;
        }
        stamina += 1;
    }

    private void LosingStamina()
    {
        if (stamina <= 0)
        {
            stamina = 0;
            return;
        }
        stamina -= 1;
    }

    private bool IsUsingStamina()
    {
        if (movementDir != Vector2.zero && isRunning)
            return true;
        
        else
            return false;
    }

    private Vector2 GetMousePosition()
    {
        return Input.mousePosition - Camera.main.WorldToScreenPoint(playerTransform.position);
    }
}