using UnityEngine;

public class DayManager : MonoBehaviour
{
    public int dayNumber = 1;
    public static DayManager Instance;
    [Range(0, 4)] public int dayRemaining;
    public int impotAdditions = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void IncrementDay()
    {
        dayNumber++;
        IncrementDayRemaining();
    }

    private void IncrementDayRemaining()
    {
        if(dayRemaining == 4)
        {
            impotAdditions++;
            dayRemaining = 1;
        } else
        {
            dayRemaining = dayRemaining + 1;
        }
    }
}
