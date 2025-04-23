using UnityEngine;

public class ItemBuyer : MonoBehaviour
{
    [SerializeField] private int itemType;
    [SerializeField] private PickableObject item;
    [SerializeField] private Sprite itemSprite;

    public void BuyItem()
    {
        Shop.Instance.SetItemForGame(itemType, item, itemSprite);
        PlayerMoney.Instance.AddMoney(-item.GetPrice());
    }
}