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
    [SerializeField] private GameObject achivements;
    [SerializeField] private GameObject mainPanel;

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

    private float angleLookAt;

    [Header("Achivements"), HideInInspector]
    private AchievementsManager achivementsManager;
    private float useCount = 0;
    private bool isTryingAchivement8;
    private bool isTryingAchivement9;

    private bool[] consomableCosumed = new bool[3];

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
        if (paused)
        { animator.SetTrigger("Sleep"); }

        GameObject canva = GameObject.Find("Canvas");
        Transform canvaTransform = canva.transform;
        slots[0] = canvaTransform.GetChild(0).GetChild(0).GetComponent<ItemSlot>();
        slots[1] = canvaTransform.GetChild(0).GetChild(1).GetComponent<ItemSlot>();
        deathPanel = canvaTransform.GetChild(2).gameObject;
        if (canvaTransform.GetChild(3))
        { pauseMenu = canvaTransform.GetChild(3).gameObject; }
        achivementsManager = canvaTransform.GetComponentInChildren<AchievementsManager>();

        if (isInHub is not true)
        {
            GameObject FogOfWar = GameObject.Find("FogOfWar");
            Transform fowTransform = FogOfWar.transform;
            fovMain = fowTransform.GetChild(2).GetChild(0).GetComponent<FieldOfView>();
            fovSecond = fowTransform.GetChild(2).GetChild(1).GetComponent<FieldOfView>();
            playerMain = fowTransform.GetChild(2).GetChild(2).GetComponent<FieldOfView>();
            playerSecond = fowTransform.GetChild(2).GetChild(3).GetComponent<FieldOfView>();

            if (Shop.Instance.CanAddItem())
            {
                AddItem(Shop.Instance.itemStruct.itemType, Shop.Instance.itemStruct.item, Shop.Instance.itemStruct.itemSprite);
                Shop.Instance.itemStruct.item = null;
            }
        }
    }

    private void Update()
    {
        if (paused) { return; }

        LookAtMouse();

        if (stamina <= 0)
        {
            stamina = 0;
            isRunning = false;
        }
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
                AudioManager.Instance.PlaySound(AudioType.run);
                animator.SetBool("IsRunning", isRunning);
            }
            else
            {
                StaminaRegen();
                AudioManager.Instance.StopSound(AudioType.run);
                animator.SetBool("IsRunning", isRunning);
            }

        }

        if (isInHub is not true)
        {
            angleLookAt = playerTransform.eulerAngles.z;

            fovMain.SetAimDirection(angleLookAt + 90);
            fovMain.SetOrigin(playerTransform.position);

            fovSecond.SetAimDirection(angleLookAt + 90);
            fovSecond.SetOrigin(playerTransform.position);

            playerMain.SetAimDirection(angleLookAt + 90);
            playerMain.SetOrigin(playerTransform.position);

            playerSecond.SetAimDirection(angleLookAt + 90);
            playerSecond.SetOrigin(playerTransform.position);
        }
    }

    public void Death()
    {
        if(paused) { return; }
        animator.SetTrigger("IsDead");
        StartCoroutine(DeathCoroutine());
        if (PlayerPrefs.GetInt("2") == 0) { achivementsManager.PlayAddAchievement(2); }
    }

    private IEnumerator DeathCoroutine()
    {
        paused = true;

        yield return new WaitForSeconds(0.8f);
        deathPanel.SetActive(true);
        slots[0].transform.parent.gameObject.SetActive(false);
        enduranceBar.gameObject.SetActive(false);
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
        float actualMovementSpeed = isRunning ? stats.speed * 2 : stats.speed;
        nextPlayerPos.Set(playerTransform.position.x + movementDir.x * actualMovementSpeed * Time.deltaTime, playerTransform.position.y + movementDir.y * actualMovementSpeed * Time.deltaTime);
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
        if (paused) { return; }
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
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
        hit = Physics2D.Raycast(playerTransform.position, GetMousePosition(), pickupDistance, interactibleLayer);
        if (hit.collider != null)
        {
            Transform hitTransform = hit.collider.transform;
            if (hitTransform.GetComponent<CollectableItem>())
            {
                if(AddItem(hitTransform.GetComponent<CollectableItem>().itemType, hitTransform.GetComponent<CollectableItem>().Item, hitTransform.GetComponent<CollectableItem>().GetInventorySprite))
                {
                    animator.SetTrigger("IsPickingUpItem");
                    Destroy(hit.collider.gameObject);
                }
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
            else if (hitTransform.tag == "Trophy")
            {
                paused = true;
                achivements.SetActive(true);
                mainPanel.SetActive(false);
            }
            else if (hitTransform.tag == "Journal")
            {
                hitTransform.GetComponent<JournalItem>().ActionJournal();
            }
        }
    }

    public bool AddItem(int _itemType, PickableObject _item, Sprite _itemSprite)
    {
        print(_itemType);
        switch (_itemType)
        {
            case 0:
                if (sellableObject is not null) { return false; }

                sellableObject = (SellableObject)_item;
                slots[0].AddItem(_itemSprite);
                break;
            case 1:
                if (usableObject is not null) { return false; }

                usableObject = (PepperSpray)_item;
                slots[1].AddItem(_itemSprite);
                break;
            case 2:
                if (usableObject is not null) { return false; }

                usableObject = (EmptyBottle)_item;
                slots[1].AddItem(_itemSprite);
                break;
            case 3:
                if (usableObject is not null) { return false; }

                usableObject = (MonsterCan)_item;
                slots[1].AddItem(_itemSprite);
                break;
        }
        return true;
    }

    public void UseItem(InputAction.CallbackContext _ctx)
    {
        if(paused) { return; }
        hit = Physics2D.Raycast(playerTransform.position, GetMousePosition(), pickupDistance, obstacleLayer);

        if (usableObject is not null)
        {
            usableObject.Action(gameObject);
            AddConsumedItem(usableObject.type);
            CheckForAchivement6();

            if (usableObject.type == 1 || usableObject.type == 2)
            {
                print(usableObject.type);
                animator.SetTrigger("IsThrowingItem");
                if (usableObject.type == 1)
                {
                    if (PlayerPrefs.GetInt("9") is 0)
                    {
                        PepperSpray paperSpay = (PepperSpray)usableObject;
                        paperSpay.SetAchivement(achivementsManager);
                        if (isTryingAchivement9 is not true && useCount is 0)
                        {
                            isTryingAchivement9 = true;
                            StartCoroutine(StartCheckingForAchivementUsingItems(9));
                        }
                        else if (isTryingAchivement9 is true)
                        {
                            useCount++;
                        }
                    }
                }
            }

            if (usableObject.type == 3)
            {
                animator.SetTrigger("IsDrinkingItem");
                if (PlayerPrefs.GetInt("8") is 0)
                {
                    if (isTryingAchivement8 is not true && useCount is 0)
                    {
                        isTryingAchivement8 = true;
                        StartCoroutine(StartCheckingForAchivementUsingItems(8));
                    }
                    else if (isTryingAchivement8 is true)
                    {
                        useCount++;
                    }
                }
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

    private void AddConsumedItem(int _itemId)
    {
        consomableCosumed[_itemId-1] = true;
    }

    private void CheckForAchivement6()
    {
        for (int i = 0; i < consomableCosumed.Length; ++i)
        {
            if (consomableCosumed[i] is true)
            {
                return;
            }
        }
        achivementsManager.PlayAddAchievement(6);
    }

    private IEnumerator StartCheckingForAchivementUsingItems(int _achivementIndex)
    {
        float timeRemaining = 0;
        useCount = 1;
        while (timeRemaining < 3f)
        {
            timeRemaining += Time.deltaTime;
            if (useCount == 3)
            {
                achivementsManager.PlayAddAchievement(_achivementIndex);
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }

        useCount = 0;
        isTryingAchivement8 = false;
        isTryingAchivement9 = false;
    }

    public void Sprint(InputAction.CallbackContext _ctx)
    {
        if (stamina <= 0)
        {
            return;
        }

        isRunning = true;
    }

    private void StopPrint(InputAction.CallbackContext _ctx)
    {
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
        while (remainingTime < _cooldown)
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
        return movementDir != Vector2.zero && isRunning;
    }

    private Vector2 GetMousePosition()
    {
        return Input.mousePosition - Camera.main.WorldToScreenPoint(playerTransform.position);
    }
}