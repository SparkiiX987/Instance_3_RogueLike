using UnityEngine;

public class Chase : IState
{
    public Ennemy ennemy;
    public float multiplicater;
    public void Action()
    {
        if (ennemy.amplificater == 1f) { ennemy.amplificater *= multiplicater; }
    }
}
