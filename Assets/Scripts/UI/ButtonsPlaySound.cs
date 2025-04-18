using UnityEngine;

public class ButtonsPlaySound : MonoBehaviour
{
    public void PlaySound()
    {
        AudioManager.Instance.PlaySound(AudioType.buttons);
    }
}
