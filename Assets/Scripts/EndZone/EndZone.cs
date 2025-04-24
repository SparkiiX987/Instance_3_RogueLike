using UnityEngine;

public class EndZone : MonoBehaviour
{
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private ParticleSystem endParticles;

   private void Start()
    {
        Instantiate(endParticles, gameObject.transform);
        endGamePanel = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<PlayerControl>().sellableObject is not null)
        {
            Time.timeScale = 0;
            PlayerMoney.Instance.AddMoney(collision.GetComponent<PlayerControl>().sellableObject.GetPrice());
            endGamePanel.SetActive(true);
        }
    }
}
