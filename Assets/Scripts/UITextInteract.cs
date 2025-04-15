using UnityEngine;

public class UITextInteract : MonoBehaviour
{
    public GameObject panelCommande;
    public GameObject panelText;

    private void Update()
    {
        Interact();
    }

    public void Interact()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            panelText.SetActive(true);
        }
    }
}
