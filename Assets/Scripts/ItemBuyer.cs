using UnityEngine;

public class ItemBuyer : MonoBehaviour
{
    [SerializeField] private int itemType;
    [SerializeField] private PickableObject item;
    [SerializeField] private Sprite itemSprite;

    public void BuyItem()
    {
        if(PlayerMoney.Instance.money < item.price || Shop.Instance.CanAddItem()) { return; }
        PlayerMoney.Instance.AddMoney(-item.GetPrice());
        Shop.Instance.SetItemForGame(itemType, item, itemSprite);
    }
}