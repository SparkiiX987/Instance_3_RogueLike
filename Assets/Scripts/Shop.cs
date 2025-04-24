using UnityEngine;
using UnityEngine.Events;

public class Shop : MonoBehaviour
{
    public struct itemSavedForGame
    {
        public itemSavedForGame(int _itemType, PickableObject _item, Sprite _itemSprite) 
        {
            itemType = _itemType;
            item = _item;
            itemSprite = _itemSprite;
        }

        public int itemType;
        public PickableObject item;
        public Sprite itemSprite;
    }

    public static Shop Instance;

    public ShopOffer activeOffer;
    public ShopOffer[] offers = new ShopOffer[5];

    public Quest[] questsAvailables = new Quest[3];

    public itemSavedForGame itemStruct;

    public UnityEvent<Sprite> OnItemBuyed;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetItemForGame(int _itemType, PickableObject _item, Sprite _itemSprite)
    {
        itemStruct = new(_itemType, _item, _itemSprite);
        OnItemBuyed.Invoke(_itemSprite);
    }

    public bool CanAddItem()
    {
        return itemStruct.item is not null;
    }
}
