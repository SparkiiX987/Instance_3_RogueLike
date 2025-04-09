using System.Collections.Generic;
using UnityEngine;

public class PathfindingDebugger : MonoBehaviour
{
    public Node startNode;
    public Node goalNode;
    public Color pathColor = Color.green;
    public float sphereRadius = 0.2f;

    private List<Node> _path;

    public Ennemy ennemy;

    private void OnDrawGizmosSelected()
    {
        if (startNode == null || goalNode == null) { return; }

        _path = ennemy.TestPathFinding(startNode, goalNode);

        if (_path == null)
        {
            print("pas de paf");
            return;
        }

        Gizmos.color = pathColor;
        for (int i = 0; i < _path.Count - 1; i++)
        {
            Gizmos.DrawLine(_path[i].GetCellPosition(), _path[i + 1].GetCellPosition());
            Gizmos.DrawSphere(_path[i].GetCellPosition(), sphereRadius);
        }

        if (_path.Count > 0)
        {
            Gizmos.DrawSphere(_path[_path.Count - 1].GetCellPosition(), sphereRadius);
        }
    }
}
