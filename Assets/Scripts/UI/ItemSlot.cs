using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private Image itemImage;
    private Color imageColor = Color.white;

    private void Start()
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void AddItem(Sprite _itemSprite)
    {
        itemImage.sprite = _itemSprite;
        imageColor.a = 1;
        itemImage.color = imageColor;
    }

    public void RemoveSprite()
    {
        imageColor.a = 0;
        itemImage.color = imageColor;
    }
}
