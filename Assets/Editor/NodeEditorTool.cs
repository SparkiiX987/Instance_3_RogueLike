using System;
using UnityEditor;
using UnityEngine;

public class NodeEditorTool : EditorWindow
{
    private GameObject nodePrefab;
    private float connectionRange;

    [MenuItem("Tool/Node Editor")]
    public static void ShowWindow()
    {
        GetWindow<NodeEditorTool>("Node Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Node Tool", EditorStyles.boldLabel);

        nodePrefab = (GameObject)EditorGUILayout.ObjectField("Node Prefab", nodePrefab, typeof(GameObject), false);
        connectionRange = Mathf.Max(0f, EditorGUILayout.FloatField("Auto-Connect range", connectionRange));

        if(GUILayout.Button("place node at scene view camera"))
        {
            PlaceNode();
        }
    }

    private void PlaceNode()
    {
        if(nodePrefab == null)
        {
            Debug.LogWarning("No Node prefab assigned !");
            return;
        }

        Vector3 position = SceneView.lastActiveSceneView.camera.transform.position + SceneView.lastActiveSceneView.camera.transform.forward * 5f;
        GameObject newNodeGO = (GameObject)PrefabUtility.InstantiatePrefab(nodePrefab);
        newNodeGO.transform.position = position;

        Node newNode = newNodeGO.GetComponent<Node>();
        if (newNode == null)
        {
            Debug.LogError("Prefab must have a Node component.");
            Destroy(newNode);
            return;
        }

        ConnectToNearbyNodes(newNode);
        Undo.RegisterCreatedObjectUndo(newNodeGO, "Place Node");
    }


    private void ConnectToNearbyNodes(Node newNode)
    {
        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        foreach(Node node in allNodes)
        {
            if(node == newNode)
            { continue; }

            float dist = Vector2.Distance(newNode.GetNodePosition(), node.GetNodePosition());
            if(dist <= connectionRange)
            {
                newNode.AddLink(new Link(node, dist, 0f));
                node.AddLink(new Link(newNode, dist, 0f));
            }
        }
    }
}
