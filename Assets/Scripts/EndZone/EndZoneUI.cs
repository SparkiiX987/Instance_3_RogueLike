using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZoneUI : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
