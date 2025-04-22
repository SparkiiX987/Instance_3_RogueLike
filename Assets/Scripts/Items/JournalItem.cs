using UnityEngine;

public class JournalItem : MonoBehaviour
{
    [SerializeField] private Sprite floorSprite;
    [SerializeField] private Sprite highlightedFloorSprite;
    [SerializeField] private float distance;
    [SerializeField] private LoreUI loreUI;
    
    private GameObject player;
    private SpriteRenderer spriteRenderer;
    

    private void Update()
    {
        if(player is null) { return; }

        //print(Vector3.Distance(player.transform.position, transform.position));
        spriteRenderer.sprite = distance <= Vector3.Distance(player.transform.position, transform.position) ? floorSprite : highlightedFloorSprite;
    }

    public void ActionJournal()
    {
        loreUI.GetComponent<LoreUI>().ShowLorePage();
    }
}
