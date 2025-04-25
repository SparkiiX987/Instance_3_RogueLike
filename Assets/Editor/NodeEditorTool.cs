using UnityEditor;
using UnityEngine;

public class NodeEditorTool : EditorWindow
{
    private GameObject nodePrefab;
    private Transform nodesParent;
    private float connectionRange;
    private bool isPlacingNode = false;


    [MenuItem("Tools/Node Editor")]
    public static void ShowWindow()
    {
        GetWindow<NodeEditorTool>("Node Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Node Tool", EditorStyles.boldLabel);

        nodePrefab = (GameObject)EditorGUILayout.ObjectField("Node Prefab", nodePrefab, typeof(GameObject), false);
        nodesParent = (Transform)EditorGUILayout.ObjectField("Node Parent", nodesParent, typeof(Transform), true);
        connectionRange = Mathf.Max(0f, EditorGUILayout.FloatField("Auto-Connect range", connectionRange));

        if (!isPlacingNode)
        {
            if (GUILayout.Button("Start Placing Node"))
            {
                isPlacingNode = true;
                SceneView.duringSceneGui += OnSceneGUI;
            }
        }
        else
        {
            GUILayout.Label("Click in scene to place the node...");
            if (GUILayout.Button("Cancel Placement"))
            {
                isPlacingNode = false;
                SceneView.duringSceneGui -= OnSceneGUI;
            }
        }
    }

    private void OnSceneGUI(SceneView _sceneView)
    {
        Event @event = Event.current;

        if (@event.type == EventType.MouseDown && @event.button == 0 && !@event.alt)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(@event.mousePosition);

            Plane plane = new Plane(Vector3.forward, new Vector3(0, 0, 0));
            if (plane.Raycast(worldRay, out float distance))
            {
                Vector3 worldPos = worldRay.GetPoint(distance);
                Debug.Log("Clicked at: " + worldPos);
                PlaceNode(worldPos);
            }
            else
            {
                Debug.LogWarning("Ray did not hit the Z=0 plane.");
            }

            @event.Use();
        }
    }

    private void PlaceNode(Vector3 _position)
    {
        Debug.Log(_position);
        if (nodePrefab == null)
        {
            Debug.LogWarning("No prefab assigned.");
            return;
        }

        GameObject newNodeGO = (GameObject)PrefabUtility.InstantiatePrefab(nodePrefab, nodesParent);
        newNodeGO.transform.position = _position;

        Node newNode = newNodeGO.GetComponent<Node>();
        if (newNode == null)
        {
            Debug.LogError("Prefab must have a Node component.");
            return;
        }

        //ConnectToNearbyNodes(newNode);
        Undo.RegisterCreatedObjectUndo(newNodeGO, "Place Node");
    }



    private void ConnectToNearbyNodes(Node newNode)
    {
        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        foreach (Node node in allNodes)
        {
            if (node == newNode)
            { continue; }

            float dist = Vector2.Distance(newNode.GetNodePosition(), node.GetNodePosition());
            if (dist <= connectionRange)
            {
                newNode.AddLink(new Link(node, 0, 0f));
                node.AddLink(new Link(newNode, 0, 0f));
            }
        }
    }
}
