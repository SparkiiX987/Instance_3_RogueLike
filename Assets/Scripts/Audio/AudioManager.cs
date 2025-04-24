using UnityEngine;

public enum AudioType
{
    walk,
    bottleBreak,
    death,
    drinkSoda,
    footstepWood,
    itemTake,
    lacrymogene,
    Throw,
    monsterRoaming,
    run,
    ambianceIG,
    ambianceMenu,
    buttons,
    wind
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public struct AudioData
    {
        public AudioType type;
        public AudioSource source;
    }

    public AudioData[] audioDatas;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioType type)
    {
        AudioData data = GetAudioData(type);
        if(data.source.isPlaying) { return; }
        data.source.Play();
    }

    public void StopSound(AudioType type)
    {
        AudioData data = GetAudioData(type);
        data.source.Stop();
    }

    public void StopAllSound()
    {
        foreach(AudioData audioData in audioDatas)
        {
            audioData.source.Stop();
        }
    }

    public AudioData GetAudioData(AudioType type)
    {
        for (int i = 0; i < audioDatas.Length; i++)
        {
            if (audioDatas[i].type == type)
            {
                return audioDatas[i];
            }
        }
        Debug.LogError("AudioManager: No clip found for type " + type);
        return new AudioData();
    }
}
