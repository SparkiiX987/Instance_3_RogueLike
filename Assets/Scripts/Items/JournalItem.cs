using UnityEngine;

public class JournalItem : MonoBehaviour
{
    [SerializeField] private Sprite floorSprite;
    [SerializeField] private Sprite highlightedFloorSprite;
    [SerializeField] private float distance;
    [SerializeField] private GameObject loreUI;
    
    private bool hasBeenChecked;

    private void Start()
    {
       loreUI = GameObject.Find("LorePanel");
       loreUI.SetActive(false);
    }
    
    public void ActionJournal()
    {
        if (hasBeenChecked == false)
        {
            print("Start");
            loreUI.SetActive(true);
            loreUI.GetComponent<LoreUI>().ShowLorePage();
            hasBeenChecked = true;
            print("End");
        }
    }
}