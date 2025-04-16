using TMPro;
using UnityEngine;

public class TutoTrigger : MonoBehaviour
{
    [SerializeField] private string text;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI paneltext;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            paneltext.text = text;
            panel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            panel.SetActive(false);
            Destroy(gameObject);
        }
    }
}
