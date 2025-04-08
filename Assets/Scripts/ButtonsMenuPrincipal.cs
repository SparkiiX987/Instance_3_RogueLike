using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsMenuPrincipal : MonoBehaviour
{
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Button _buttonSettings;
    [SerializeField] private Button _buttonLeave;

    [SerializeField] private GameObject _panelMenuPrincipal;
    [SerializeField] private GameObject _panelSettings;



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
            SceneManager.LoadScene("Game");
        }
    }
}
