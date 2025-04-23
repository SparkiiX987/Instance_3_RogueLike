using UnityEngine;

public class ItemBuyer : MonoBehaviour
{
    private PlayerControl playerControl;
    [SerializeField] private int itemType;
    [SerializeField] private PickableObject item;
    [SerializeField] private Sprite itemSprite;

    private void Start()
    {
        playerControl = GameObject.Find("Player").GetComponent<PlayerControl>();
    }

    public void BuyItem()
    {
        playerControl.AddItem(itemType, item, itemSprite);
        PlayerMoney.Instance.AddMoney(-item.GetPrice());
    }
}