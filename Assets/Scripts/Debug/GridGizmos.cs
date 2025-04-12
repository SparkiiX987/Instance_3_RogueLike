using UnityEngine;

[ExecuteAlways]
public class GridGizmo : MonoBehaviour
{
    public float cellSize = 32f;
    public int gridWidth = 100;
    public int gridHeight = 100;
    public Color gridColor = Color.green;

    float totalWidth => gridWidth * cellSize;
    float totalHeight => gridHeight * cellSize;

    Vector3 bottomLeft => new Vector3(
        transform.position.x - totalWidth / 2f,
        transform.position.y - totalHeight / 2f,
        transform.position.z
    );

    private void OnDrawGizmos()
    {
        Gizmos.color = gridColor;

        Vector3 origin = transform.position;

        for (int x = 0; x <= gridWidth; x++)
        {
            float xPos = bottomLeft.x + x * cellSize;
            Vector3 start = new Vector3(xPos, bottomLeft.y, origin.z);
            Vector3 end = new Vector3(xPos, bottomLeft.y + totalHeight, origin.z);
            Gizmos.DrawLine(start, end);
        }

        for (int y = 0; y <= gridHeight; y++)
        {
            float yPos = bottomLeft.y + y * cellSize;
            Vector3 start = new Vector3(bottomLeft.x, yPos, origin.z);
            Vector3 end = new Vector3(bottomLeft.x + totalWidth, yPos, origin.z);
            Gizmos.DrawLine(start, end);
        }
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Floor((position.x - bottomLeft.x) / cellSize) * cellSize + bottomLeft.x + (cellSize / 2);
        float y = Mathf.Floor((position.y - bottomLeft.y) / cellSize) * cellSize + bottomLeft.y + (cellSize / 2);

        return new Vector3(x, y, position.z);
    }
}
