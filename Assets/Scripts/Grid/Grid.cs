using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private List<Node> cells = new List<Node>();

    public void AddCell(Node _cell)
    {
        cells.Add(_cell);
    }

    public Node GetCellAt(int _index)
    {
        return cells[_index];
    }

    public List<Node> GetCellList()
    {
        return cells;
    }
}
