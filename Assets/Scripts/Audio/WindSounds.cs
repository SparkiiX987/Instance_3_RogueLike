using System.Collections;
using UnityEngine;

public class WindSounds : MonoBehaviour
{
    private float minTime = 5f;
    private float maxTime = 120f;

    private void Start()
    {
        StartCoroutine(PlaySound());
    }

    private IEnumerator PlaySound()
    {
        while (true)
        {
            float index = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(index);
            AudioManager.Instance.PlaySound(AudioType.wind);
        }
    }
}
