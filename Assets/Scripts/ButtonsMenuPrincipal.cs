using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsMenuPrincipal : MonoBehaviour
{
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Button _buttonLeave;

    private void Start()
    {
        //AudioManager.Instance.PlaySound(AudioType.ambianceMenu);
    }

    public void Leave()
    {
        if(_buttonLeave != null)
        {
            Application.Quit();
        }
    }

    public void Play()
    {
        if( _buttonPlay != null)
        {
            SceneManager.LoadScene("GameScene");
            AudioManager.Instance.StopSound(AudioType.ambianceMenu);
            AudioManager.Instance.PlaySound(AudioType.ambianceIG);
        }
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene(2);
    }
}
