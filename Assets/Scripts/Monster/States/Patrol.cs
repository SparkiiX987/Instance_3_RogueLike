using UnityEngine;

public class Patrol : IState
{
    public Ennemy ennemy;

    public void Action()
    {
        if (ennemy.path != null && ennemy.path.Count > 0 )
        {
            ennemy.nextPointToMove = ennemy.path[ennemy.currentIndexNode].transform.position;
        }
    }
}
