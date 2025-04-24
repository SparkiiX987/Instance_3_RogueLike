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

        Vector3 newPos = Vector3.zero;
        Vector3 newScale = new Vector3(Vector3.Distance(start.position, destination.position) - ((start.localScale.x / 2) + (destination.localScale.x / 2)),1);
        newPos = (destination.position - start.position) / 2;
        
        transformObstacle.localPosition = newPos;
        transformObstacle.localScale = newScale;

        boxCollider2D.size = Vector2.one;
        boxCollider2D.offset = Vector2.zero;

        activated = true;
    }
}
