using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAmbianceSound : MonoBehaviour
{
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            AudioManager.Instance.PlaySound(AudioType.ambianceMenu);
        }
        else 
        {
            AudioManager.Instance.PlaySound(AudioType.ambianceIG);
        }
    }
}
