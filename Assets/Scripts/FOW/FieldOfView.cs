using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private Mesh mesh;
    private Vector3 origin;
    private float startingAngle;

    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;

    [Header("Parameters")]
    public LayerMask layerMask;
    public float fov = 90f;
    public float viewDistance = 6f;
    public int rayCount = 10;


    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void LateUpdate()
    {
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;

        vertices = new Vector3[rayCount + 1 + 1];
        uv = new Vector2[vertices.Length];
        triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i < rayCount; i++)
        {
            float angleRad = angle * (Mathf.PI / 180f);
            Vector3 dir = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
            Vector3 vertex;

            RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, dir, viewDistance, layerMask);

            if (raycastHit2D.collider == null)
            {
                vertex = origin + dir * viewDistance;
            }
            else
            {
                vertex = raycastHit2D.point;
            }
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
    }

    public void SetOrigin(Vector3 _origin)
    {
        origin = _origin;
    }

    public void SetAimDirection(float _angle)
    {
        if (_angle < 0) _angle += 360;
        startingAngle = _angle + (fov / 2f);
    }
}
