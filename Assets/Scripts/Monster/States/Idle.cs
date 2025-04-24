using UnityEngine;
using UnityEngine.Events;

public class Idle : IState
{
    public Ennemy ennemy;
    public UnityEvent onIdle = new();
    public void Action()
    {
        //Debug.Log("start idle");
        //Play animation of idle
        onIdle.Invoke();
    }
}
