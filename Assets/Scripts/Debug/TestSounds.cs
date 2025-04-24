using UnityEngine;

public class TestSounds : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlaySound(AudioType.walk);
    }
}
