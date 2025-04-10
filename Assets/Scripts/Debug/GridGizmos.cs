using UnityEngine;

[ExecuteAlways]
public class GridGizmos : MonoBehaviour
{
    public float cellSize = 0.32f;
    public int gridWidth = 100;
    public int gridHeight = 100;
    public Color gridColor = Color.green;

    private void OnDrawGizmos()
    {
        Gizmos.color = gridColor;

        Vector3 origin = transform.position;

        for (int x = StartPoint(gridWidth); x <= gridWidth / 2; x++)
        {
            float xPos = origin.x + x * cellSize;
            Vector3 start = new Vector3(xPos, origin.y - gridHeight / 4, origin.z);
            Vector3 end = new Vector3(xPos, origin.y + gridHeight / 2 * cellSize, origin.z);
            Gizmos.DrawLine(start, end);
        }

        for (int y = StartPoint(gridHeight); y <= gridHeight / 2; y++)
        {
            float yPos = origin.y + y * cellSize;
            Vector3 start = new Vector3(origin.x - gridWidth / 4, yPos, origin.z);
            Vector3 end = new Vector3(origin.x + gridWidth / 2 * cellSize, yPos, origin.z);
            Gizmos.DrawLine(start, end);
        }
    }

    public int StartPoint(int _point)
    {
        return (_point / 2) * -1;
    }
}
