using System.Collections.Generic;
using UnityEngine;

public class PathfindingDebugger : MonoBehaviour
{
    public Node startNode;
    public Node goalNode;
    public Color pathColor = Color.green;
    public float sphereRadius = 0.2f;

    private List<Node> path;

    public Ennemy ennemy;

    private void OnDrawGizmosSelected()
    {
        if (startNode == null || goalNode == null) { return; }
        path = ennemy.TestPathFinding(startNode, goalNode);

        if (path == null)
        {
            print("pas de paf");
            return;
        }

        print("ya le paf !");
        Gizmos.color = pathColor;
        for (int i = 0; i < path.Count - 1; i++)
        {
            Gizmos.DrawLine(path[i].GetNodePosition(), path[i + 1].GetNodePosition());
            Gizmos.DrawSphere(path[i].GetNodePosition(), sphereRadius);
        }

        if (path.Count > 0)
        {
            Gizmos.DrawSphere(path[path.Count - 1].GetNodePosition(), sphereRadius);
        }
    }

    private void Update()
    {
        
    }
}
