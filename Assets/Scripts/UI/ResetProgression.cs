using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetProgression : MonoBehaviour
{
    private GameObject managers;

    private void Start()
    {
        managers = GameObject.Find("Managers");
    }

    public void ResetP()
    {
        Destroy(managers);
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
}
