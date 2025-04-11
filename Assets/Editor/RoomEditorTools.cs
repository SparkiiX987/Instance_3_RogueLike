using UnityEditor;
using UnityEngine;

public class RoomEditorTools : EditorWindow
{
    public RoomCreatorData roomCreatorData;
    private bool isEditingFloor;
    private bool isEditingWall;
    private bool isEditingRoom;
    private GameObject emptyRoom;
    private GameObject instantiedRoom;


    [MenuItem("Tools/Room Editor")]
    public static void ShowWindow()
    {
        GetWindow<RoomEditorTools>("Room Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Room Editor", EditorStyles.boldLabel);

        roomCreatorData = (RoomCreatorData)EditorGUILayout.ObjectField("Room Creator Data", roomCreatorData, typeof(RoomCreatorData), false);
        emptyRoom = (GameObject)EditorGUILayout.ObjectField("Empty Room", emptyRoom, typeof(GameObject), false);

        if (!isEditingRoom)
        {
            if (GUILayout.Button("Create New Room"))
            {
                isEditingRoom = true;
                //SceneView.duringSceneGui += OnSceneGUI;
                CreateRoom();
            }
        }
        else
        {
            if (isEditingFloor)
            {
                if (GUILayout.Button("Stop Editing"))
                {
                    isEditingFloor = false;
                    SceneView.duringSceneGui -= OnSceneGUI;
                }
            }
            else if (isEditingWall)
            {
                if (GUILayout.Button("Stop Editing"))
                {
                    isEditingWall = false;
                }
            }
            else
            {
                if (GUILayout.Button("Edit Floor"))
                {
                    isEditingFloor = true;
                    SceneView.duringSceneGui += OnSceneGUI;
                }

                if (GUILayout.Button("Edit Wall"))
                {
                    isEditingWall = true;
                }
                if (GUILayout.Button("Finish"))
                {
                    isEditingRoom = false;
                    //SceneView.duringSceneGui -= OnSceneGUI;
                    instantiedRoom = null;
                }
            }
        }
    }

    public void CreateRoom()
    {
        Vector3 position = SceneView.lastActiveSceneView.camera.transform.position + SceneView.lastActiveSceneView.camera.transform.forward * 5f;
        instantiedRoom = (GameObject)PrefabUtility.InstantiatePrefab(emptyRoom);
        instantiedRoom.transform.position = position;
    }

    private void OnSceneGUI(SceneView _sceneView)
    {
        Event @event = Event.current;

        if (@event.type == EventType.MouseDown && @event.button == 0 && !@event.alt)
        {
            Ray worldRay = HandleUtility.GUIPointToWorldRay(@event.mousePosition);

            Plane plane = new Plane(Vector3.forward, Vector3.zero);
            if (plane.Raycast(worldRay, out float distance))
            {
                Vector3 worldPos = worldRay.GetPoint(distance);
                Vector3 snappedPos = instantiedRoom.GetComponent<GridGizmo>().SnapToGrid(worldPos);

                Transform floorParent = instantiedRoom.transform.GetChild(1);
                for (int i = 0; i < floorParent.childCount; i++)
                {
                    Transform child = floorParent.GetChild(i);
                    if (child.CompareTag("Tile") && child.position == snappedPos)
                    {
                        DestroyImmediate(child.gameObject);
                        break;
                    }
                }

                int randomTileIndex = Random.Range(0, roomCreatorData.floorPrefabs.Count);
                GameObject newTile = Instantiate(roomCreatorData.floorPrefabs[randomTileIndex], floorParent);
                newTile.transform.position = snappedPos;
            }
            else
            {
                Debug.LogWarning("Ray did not hit the Z=0 plane.");
            }

            @event.Use();
        }
    }
}
