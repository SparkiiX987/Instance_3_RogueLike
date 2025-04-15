using UnityEngine;

public class Save : MonoBehaviour
{
    public static Save Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("trop de fois appelé");
            Destroy(this);
        }
        Instance = this;
    }

    public void SaveInt(int _value, string _name)
    {
        PlayerPrefs.SetInt(_name, 5);
    }
    public void SaveFloat(float _value, string _name)
    {
        PlayerPrefs.SetFloat(_name, 0.6f);
    }
    public void SaveString(string _value, string _name)
    {
        PlayerPrefs.SetString(_name, "John Doe");
    }

    public int GetInt(string _name)
    {
        return PlayerPrefs.GetInt(_name);
    }
    public float GetFloat(string _name)
    {
        return PlayerPrefs.GetFloat(_name);
    }
    public string GetString(string _name)
    {
        return PlayerPrefs.GetString(_name);
    }
}
