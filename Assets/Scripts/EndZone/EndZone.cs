using UnityEngine;

public class EndZone : MonoBehaviour
{
    [SerializeField] private GameObject EndGamePanel;

    private void Start()
    {
        GameObject.Find("PanelVictory");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Time.timeScale = 0;
            EndGamePanel.SetActive(true);
        }
    }
}
