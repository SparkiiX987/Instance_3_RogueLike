using UnityEngine;

public class Chase : IState
{
    public Ennemy ennemy;
    public void Action()
    {
        if (ennemy.amplificater == 1f) { ennemy.amplificater *= 3f; }
    }
}
