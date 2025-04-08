using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour, ITargetable
{
    [SerializeField] private float staminaMax;
    
    [SerializeField] private float cooldown;
    
    private bool isDetectable;
    private int health;
    private float stamina;
    private bool isRunning;

    private Stats stats;
    private SellableObject sellableObject;
    private UsableObject usableObject;
    
    private float currentCooldown;
    
    private InputAction moveAction;
    private InputAction lookAction;

    private Vector2 movementDir;
    private Vector2 nextPlayerPos;
    private Transform playerTransform;
    
    private void Awake()
    {
        stats = GetComponent<Stats>();
        sellableObject = GetComponent<SellableObject>();
        usableObject = GetComponent<UsableObject>();
        playerTransform = GetComponent<Transform>();
    }

    private void Start()
    {
        stamina = staminaMax;
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
    }
    
    public void LookAtMouse()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(playerTransform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    
    public void PickUp(InputAction.CallbackContext _ctx)
    {
        
    }

    public void UseItem(InputAction.CallbackContext _ctx)
    {
        if (_ctx.performed && usableObject != null) 
            usableObject.Action();
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
}
