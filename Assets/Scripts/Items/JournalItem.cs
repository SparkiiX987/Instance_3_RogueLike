using UnityEngine;
using System.Collections;

public class JournalItem : MonoBehaviour
{
    [SerializeField] private Sprite floorSprite;
    [SerializeField] private Sprite highlightedFloorSprite;
    [SerializeField] private float distance;
    [SerializeField] private GameObject loreUI;
    
    private bool hasBeenChecked;
    
    /*private GameObject player;
    private SpriteRenderer spriteRenderer;*/

    private void Start()
    {
       loreUI = GameObject.Find("LorePanel");
       loreUI.SetActive(false);
       
       //StartCoroutine(GetPlayer());
    }
    
    /*private IEnumerator GetPlayer()
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

        //print(Vector3.Distance(player.transform.position, transform.position));
        spriteRenderer.sprite = distance <= Vector3.Distance(player.transform.position, transform.position) ? floorSprite : highlightedFloorSprite;
    }*/

    public void ActionJournal()
    {
        if (hasBeenChecked == false)
        {
            loreUI.GetComponent<LoreUI>().ShowLorePage();
            hasBeenChecked = true;
        }
    }
}
