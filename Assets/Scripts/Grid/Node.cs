using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private List<Link> links = new List<Link>();
    public Node parent;

    private Vector2 cellPosition;

    private void Awake()
    {
        cellPosition = transform.position;
    }

    public Vector2 GetCellPosition()
    {
        return cellPosition;
    }

    public List<Link> GetLinks() { return links; }

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.red;
        for (int i = 0; i < nextNodes.Count; i++)
        {
            Gizmos.DrawLine(cellPosition, nextNodes[i].GetCellPosition());
        }*/
    }
}
