using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour, ITargetable
{
    [SerializeField] private float rotationSpeed;

    public float staminaMax;

    [SerializeField] private float cooldown;

    [SerializeField] private float pickupDistance;
    [SerializeField] private LayerMask interactibleLayer;
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private bool paused;
    [SerializeField] private bool isInHub;

    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject quests;

    private InputSystem_Actions inputSystem;
    private InputAction moveInput;
    private InputAction interact;
    private InputAction sprint;
    private InputAction useItem;
    private InputAction pause;

    private int health;
    public float stamina;
    private bool isRunning;
    private Rigidbody2D rb;

    private Ray ray;
    private RaycastHit2D hit;

    private Stats stats;
    public SellableObject sellableObject;
    public UsableObject usableObject;
    public CollectableItem collectableObject;

    private ItemSlot[] slots = new ItemSlot[2];

    private float currentCooldown;

    private Vector2 movementDir;
    private Vector2 nextPlayerPos;
    private Transform playerTransform;

    private Collider2D selfCollider;

    [SerializeField] private EnduranceBar enduranceBar;

    private GameObject deathPanel;
    private GameObject pauseMenu;

    private FieldOfView fovMain;
    private FieldOfView fovSecond;
    private FieldOfView playerMain;
    private FieldOfView playerSecond;

    private bool isCafeinated;

    private void Awake()
    {
        stats = GetComponent<Stats>();
        ray = new Ray(transform.position, transform.forward);
        playerTransform = GetComponent<Transform>();
        collectableObject = GetComponent<CollectableItem>();


        inputSystem = new InputSystem_Actions();
        moveInput = inputSystem.Player.Move;
        interact = inputSystem.Player.Interact;
        sprint = inputSystem.Player.Sprint;
        useItem = inputSystem.Player.Attack;
        pause = inputSystem.Player.Pause;
        interact.started += PickUp;
        useItem.started += UseItem;
        sprint.started += Sprint;
        sprint.canceled += StopPrint;
        pause.performed += OpenPauseMenu;

    }

    private void OnEnable()
    {
        moveInput.Enable();
        interact.Enable();
        sprint.Enable();
        useItem.Enable();
        pause.Enable();
    }

    private void OnDisable()
    {
        moveInput.Disable();
        interact.Disable();
        sprint.Disable();
        useItem.Disable();
        pause.Disable();
    }

    private void Start()
    {
        stamina = staminaMax;
        hit = Physics2D.Raycast(playerTransform.position, Vector2.up, 100f);
        rb = GetComponent<Rigidbody2D>();
        selfCollider = GetComponent<Collider2D>();

        Time.timeScale = 1;
        if(paused)
        { animator.SetTrigger("Sleep"); }

        GameObject canva = GameObject.Find("Canvas");
        Transform canvaTransform = canva.transform;
        slots[0] = canvaTransform.GetChild(0).GetChild(0).GetComponent<ItemSlot>();
        slots[1] = canvaTransform.GetChild(0).GetChild(1).GetComponent<ItemSlot>();
        deathPanel = canvaTransform.GetChild(2).gameObject;
        if(canvaTransform.GetChild(3))
        { pauseMenu = canvaTransform.GetChild(3).gameObject; }

        if (isInHub is not true)
        {
            print("recup");
            GameObject FogOfWar = GameObject.Find("FogOfWar");
            Transform fowTransform = FogOfWar.transform;
            fovMain = fowTransform.GetChild(2).GetChild(0).GetComponent<FieldOfView>();
            fovSecond = fowTransform.GetChild(2).GetChild(1).GetComponent<FieldOfView>();
            playerMain = fowTransform.GetChild(2).GetChild(2).GetComponent<FieldOfView>();
            playerSecond = fowTransform.GetChild(2).GetChild(3).GetComponent<FieldOfView>();
        }
    }

    private void Update()
    {
        if (paused) { return; }

        LookAtMouse();

        if (currentCooldown >= 0)
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
                AudioManager.Instance.PlaySound(AudioType.run);
            }
            else
            {
                StaminaRegen();
                animator.SetBool("IsRunning", false);
                AudioManager.Instance.StopSound(AudioType.run);
            }

        }
    }

    public void Death()
    {
        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        paused = true;
        
        yield return new WaitForSeconds(0.8f);
        deathPanel.SetActive(true);
        slots[0].transform.parent.gameObject.SetActive(false);
        enduranceBar.gameObject.SetActive(false);
        Time.timeScale = 0;
    }

    public Vector2 GetMovementDir => movementDir;

    public void WakeUp()
    {
        animator.SetTrigger("WakeUp");
    }

    public void UnPause()
    {
        selfCollider.enabled = true;
        paused = false;
    }

    private void FixedUpdate()
    {
        if (paused is not false) { return; }

        movementDir = moveInput.ReadValue<Vector2>();
        nextPlayerPos.Set(playerTransform.position.x + movementDir.x * stats.speed * Time.deltaTime, playerTransform.position.y + movementDir.y * stats.speed * Time.deltaTime);
        rb.MovePosition(nextPlayerPos);
        animator.SetBool("IsWalkingBool", (movementDir != Vector2.zero));
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int _health)
    {
        health = _health;
    }

    private void OpenPauseMenu(InputAction.CallbackContext _ctx)
    {
        if(paused) { return; }
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
    }

    private void LookAtMouse()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(playerTransform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        if(isInHub is not true)
        {
            fovMain.SetAimDirection(angle + 90);
            fovMain.SetOrigin(playerTransform.position);

            fovSecond.SetAimDirection(angle + 90);
            fovSecond.SetOrigin(playerTransform.position);

            playerMain.SetAimDirection(angle + 90);
            playerMain.SetOrigin(playerTransform.position);

            playerSecond.SetAimDirection(angle + 90);
            playerSecond.SetOrigin(playerTransform.position);
        }
    }

    public void PickUp(InputAction.CallbackContext _ctx)
    {
        hit = Physics2D.Raycast(playerTransform.position, GetMousePosition(), pickupDistance, interactibleLayer);
        if (hit.collider != null)
        {
            Transform hitTransform = hit.collider.transform;
            if (hitTransform.GetComponent<CollectableItem>())
            {
                switch (hitTransform.GetComponent<CollectableItem>().itemType)
                {
                    case 0:
                        if (sellableObject is not null) { return; }

                        sellableObject = (SellableObject)hitTransform.GetComponent<CollectableItem>().Item;
                        slots[0].AddItem(hitTransform.GetComponent<CollectableItem>().GetInventorySprite);
                        break;
                    case 1:
                        if (usableObject is not null) { return; }

                        usableObject = (PepperSpray)hitTransform.GetComponent<CollectableItem>().Item;
                        slots[1].AddItem(hitTransform.GetComponent<CollectableItem>().GetInventorySprite);
                        break;
                    case 2:
                        if (usableObject is not null) { return; }

                        usableObject = (EmptyBottle)hitTransform.GetComponent<CollectableItem>().Item;
                        slots[1].AddItem(hitTransform.GetComponent<CollectableItem>().GetInventorySprite);
                        break;
                    case 3:
                        if (usableObject is not null) { return; }

                        usableObject = (MonsterCan)hitTransform.GetComponent<CollectableItem>().Item;
                        slots[1].AddItem(hitTransform.GetComponent<CollectableItem>().GetInventorySprite);
                        break;
                }
                animator.SetTrigger("IsPickingUpItem");
                Destroy(hit.collider.gameObject);
            }
            else if (hitTransform.TryGetComponent(out Door door))
            {
                door.UseDoor(playerTransform.position);
            }
            else if (hitTransform.tag == "Shop")
            {
                paused = true;
                shop.SetActive(true);
            }
            else if (hitTransform.tag == "Quests")
            {
                paused = true;
                quests.SetActive(true);
            }
        }
    }

    public void UseItem(InputAction.CallbackContext _ctx)
    {
        hit = Physics2D.Raycast(playerTransform.position, GetMousePosition(), pickupDistance, obstacleLayer);

        if (usableObject is not null)
        {
            usableObject.Action(gameObject);

            if (usableObject.type == 1 || usableObject.type == 2)
            {
                animator.SetTrigger("IsThrowingItem");
            }

            if (usableObject.type == 3)
            {
                animator.SetTrigger("IsDrinkingItem");
            }

            usableObject = null;
            slots[1].RemoveSprite();
        }

        else if (hit.collider != null && hit.collider.transform.GetComponent<Obstacle>())
        {
            Obstacle obstacleObject = hit.collider.transform.GetComponent<Obstacle>();
            obstacleObject.Action();
        }
    }

    public void Sprint(InputAction.CallbackContext _ctx)
    {
        if (stamina <= 0) { return; }

        stats.speed = stats.speed * 2f;
        isRunning = true;
    }

    private void StopPrint(InputAction.CallbackContext _ctx)
    {
        stats.speed = stats.speed / 2;
        isRunning = false;
    }

    private void StaminaRegen()
    {
        if (stamina >= staminaMax)
        {
            stamina = staminaMax;
            return;
        }
        stamina += 1;
        enduranceBar.fillBar.fillAmount = stamina / 100f;
    }

    public void StartCafeinatePlayer(float _cooldown)
    {
        StartCoroutine(CafeinatePlayer(_cooldown));
    }

    private IEnumerator CafeinatePlayer(float _cooldown)
    {
        isCafeinated = true;
        float remainingTime = 0;
        while(remainingTime < _cooldown)
        {
            remainingTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isCafeinated = false;
    }

    private void LosingStamina()
    {
        if (isCafeinated)
        {
            return;
        }

        if (stamina <= 0)
        {
            stamina = 0;
            return;
        }
        stamina -= 1;
        enduranceBar.fillBar.fillAmount = stamina / 100f;
    }

    private bool IsUsingStamina()
    {
        if (movementDir != Vector2.zero && isRunning)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector2 GetMousePosition()
    {
        return Input.mousePosition - Camera.main.WorldToScreenPoint(playerTransform.position);
    }
}