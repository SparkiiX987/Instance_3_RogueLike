using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class WoodenPlankBinObstacle : Obstacle
{
    [SerializeField] private Transform transformObstacle;
    [SerializeField] private Transform start;
    [SerializeField] private Transform destination;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private float speed;

    public override void Action()
    {
        //Play animation

        Vector3 newPos = new Vector2((destination.position.x - destination.localScale.x / 2) - (start.position.x - start.localScale.y/2), destination.position.y);
        Vector3 newScale = new Vector3(newPos.x, 1);
        newPos.x = destination.position.x / 2;

        transformObstacle.localPosition = newPos;
        transformObstacle.localScale = newScale;

        boxCollider2D.size = Vector2.one;
        boxCollider2D.offset = Vector2.zero;

        activated = true;
    }
}
