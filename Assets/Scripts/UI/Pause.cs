using UnityEngine;
using UnityEngine.SceneManagement;

class Pause : MonoBehaviour
{
    [SerializeField] private GameObject panelShop;
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);

        if (panelShop != null)
        {
            panelShop.SetActive(true);
        }
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        if (panelShop != null)
        {
            panelShop.SetActive(false);
        }
    }
}