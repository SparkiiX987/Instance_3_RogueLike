using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private List<Cell> nextsCell = new List<Cell>();

    private Vector2 cellPosition;

    private void Awake()
    {
        cellPosition = transform.position;
    }

    public Vector2 GetCellPosition()
    {
        return cellPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < nextsCell.Count; i++)
        {
            Gizmos.DrawLine(cellPosition, nextsCell[i].GetCellPosition());
        }
    }
}
