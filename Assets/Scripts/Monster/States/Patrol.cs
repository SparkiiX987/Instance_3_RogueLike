using UnityEngine;

public class Patrol : IState
{
    public Ennemy ennemy;

    public void Action()
    {
        Debug.Log("start Patrol");

    }
}
