using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour, ITargetable
{
    [SerializeField] private float staminaMax;

    [SerializeField] private float cooldown;

    [SerializeField] private float pickupDistance;
    [SerializeField] private LayerMask itemLayer;
    
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private bool isDetectable;
    private int health;
    private float stamina;
    private bool isRunning;
    
    private Ray ray;
    private RaycastHit2D hit;

    private Stats stats;
    private SellableObject sellableObject;
    private UsableObject usableObject;
    private CollectableItem collectableObject;
    
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
    }

    private void Update()
    {
        nextPlayerPos.Set(playerTransform.position.x + movementDir.x * stats.speed * Time.deltaTime, playerTransform.position.y + movementDir.y * stats.speed * Time.deltaTime);
        playerTransform.position = nextPlayerPos;
        
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
            }
            else
                StaminaRegen();
        }
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
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
        if (_ctx.performed && usableObject != null)
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
    }
    
    public void Sprint(InputAction.CallbackContext _ctx)
    {
        float walkingSpeed = 5f;

        if (_ctx.performed && stamina > 0)
        {
            stats.speed = stats.speed * 2f;
            animator.SetFloat("WalkingSpeed", 1.5f);
            isRunning = true;
        }

        else
        {
            stats.speed = walkingSpeed;
            animator.SetFloat("WalkingSpeed", 1);
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