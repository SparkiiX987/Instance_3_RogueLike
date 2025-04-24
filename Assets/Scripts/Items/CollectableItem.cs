using System.Collections;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    public Sprite floorSprite;
    
    private PickableObject item;
    [SerializeField] private int price;
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private Sprite inventorySprite;
    [SerializeField] private Sprite highlightedFloorSprite;
    [SerializeField] private float distance;

    public int itemType;
    private GameObject player;
    private SpriteRenderer spriteRenderer;

    [Header("Only for pepper spray and empty bottle")]
    [SerializeField] private GameObject projectile;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InitializeItem();
        StartCoroutine(GetPlayer());
    }

    public Sprite GetInventorySprite => inventorySprite;

    private void InitializeItem()
    {
        switch (itemType)
        {
            case 0:
                item = new SellableObject(price, itemName, description, 0);
                break;
            case 1:
                item = new PepperSpray(price, itemName, description, 1, projectile);
                break;
            case 2:
                item = new EmptyBottle(price, itemName, description, 1, projectile);
                break;
            case 3:
                item = new MonsterCan(price, itemName, description, 1, 3f);
                break;
        }
    }

    public PickableObject Item => item;

    private IEnumerator GetPlayer()
    {
        while(player is null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if(player is not null)
            {
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Update()
    {
        if(player is null) { return; }

        spriteRenderer.sprite = distance <= Vector3.Distance(player.transform.position, transform.position) ? floorSprite : highlightedFloorSprite;
    }
}
