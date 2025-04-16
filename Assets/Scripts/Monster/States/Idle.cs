using UnityEngine;
using UnityEngine.Events;

public class Idle : IState
{
    public Ennemy ennemy;
    public UnityEvent onIdle = new();
    public void Action()
    {
        //Play animation of idle
        onIdle.Invoke();
    }
}
