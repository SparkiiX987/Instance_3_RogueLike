using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private List<Cell> cells = new List<Cell>();

    public void AddCell(Cell _cell)
    {
        cells.Add(_cell);
    }

    public Cell GetCellAt(int _index)
    {
        return cells[_index];
    }
}
