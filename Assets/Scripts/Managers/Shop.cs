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

    public PickableObject activeOffer;
    public PickableObject[] offers = new PickableObject[5];

    public Quest[] questsAvailables = new Quest[3];

    public itemSavedForGame itemStruct;

    public UnityEvent<Sprite> OnItemBuyed;

    private void Awake()
    {
        if (Instance != null)
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

    public void SetItemForGame(int _itemType, PickableObject _item, Sprite _itemSprite, GameObject _projectile = null)
    {
        switch (_itemType)
        {
            case 1:
                itemStruct = new(_itemType, new PepperSpray(_item.price, _item.name, _item.description, _itemType, _projectile), _itemSprite);
                break;
            case 2:
                itemStruct = new(_itemType, new EmptyBottle(_item.price, _item.name, _item.description, _itemType, _projectile), _itemSprite);
                break;
            case 3:
                itemStruct = new(_itemType, new MonsterCan(_item.price, _item.name, _item.description, _itemType, 3f), _itemSprite);
                break;
        }
        OnItemBuyed.Invoke(_itemSprite);
    }

    public bool CanAddItem()
    {
        return itemStruct.item is not null;
    }
}
