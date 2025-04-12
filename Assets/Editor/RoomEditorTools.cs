using UnityEditor;
using UnityEngine;

public class RoomEditorTools : EditorWindow
{
    public RoomCreatorData roomCreatorData;
    private string roomName;
    private bool isEditingFloor;
    private bool isEditingDoors;
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


        GUILayout.Label("Room Name");
        roomName = EditorGUILayout.TextField(roomName);

        if (!isEditingRoom)
        {
            if (GUILayout.Button("Create new room"))
            {
                isEditingRoom = true;
                //SceneView.duringSceneGui += OnSceneGUI;
                CreateRoom();
            }
        }
        else
        {
            if (isEditingFloor || isEditingDoors)
            {
                if (GUILayout.Button("Stop editing"))
                {
                    isEditingFloor = false;
                    isEditingDoors = false;
                    SceneView.duringSceneGui -= OnSceneGUI;
                    SceneView.duringSceneGui -= CreateDoor;
                }
            }
            else
            {
                if (GUILayout.Button("Edit floor"))
                {
                    isEditingFloor = true;
                    SceneView.duringSceneGui += OnSceneGUI;
                }

                if (GUILayout.Button("Edit Doors"))
                {
                    isEditingDoors = true;
                    SceneView.duringSceneGui += CreateDoor;
                }

                if (GUILayout.Button("Save as prefab"))
                {
                    SaveAsPrefab("Assets/Prefabs/Rooms/" + roomName + ".prefab");
                    roomName = "";
                    isEditingRoom = false;
                    instantiedRoom = null;
                }

                if (GUILayout.Button("Delete"))
                {
                    isEditingRoom = false;
                    DestroyImmediate(instantiedRoom);
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
    private bool IsInGrid(GameObject _room, Vector3 _tilePosition)
    {
        Vector3 roomPosition = _room.transform.position;

        return (_tilePosition.x > (roomPosition.x - (4 * 0.32f))
             && _tilePosition.x < (roomPosition.x + (4 * 0.32f))
             && _tilePosition.y > (roomPosition.y - (4 * 0.32f))
             && _tilePosition.y < (roomPosition.y + (4 * 0.32f)));
    }

    public void SaveAsPrefab(string path)
    {
        if (instantiedRoom == null)
        { return; }

        GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(instantiedRoom, path, InteractionMode.UserAction);
        Debug.Log("Room saved as prefab at: " + path);
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
                if (!IsInGrid(instantiedRoom, snappedPos))
                {
                    return;
                }

                Transform floorParent = instantiedRoom.transform.GetChild(1);
                Transform wallParent = instantiedRoom.transform.GetChild(0);

                for (int i = 0; i < floorParent.childCount; i++)
                {
                    Transform child = floorParent.GetChild(i);
                    if (child.position == snappedPos)
                    {
                        DestroyImmediate(child.gameObject);
                        break;
                    }
                }
                for (int i = 0; i < wallParent.childCount; i++)
                {
                    Transform child = wallParent.GetChild(i);
                    if (child.position == snappedPos)
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }

                int randomTileIndex = Random.Range(0, roomCreatorData.floorPrefabs.Count);
                GameObject newTile = Instantiate(roomCreatorData.floorPrefabs[randomTileIndex], floorParent);
                newTile.transform.position = snappedPos;

                Vector3[] directions = new Vector3[]
                {
                    Vector3.down,
                    Vector3.left,
                    Vector3.right,
                    Vector3.up
                };

                for (int i = 0; i < directions.Length; i++)
                {
                    Vector3 neighborPos = snappedPos + directions[i] * 0.32f;
                    bool hasNeighbor = false;

                    for (int j = 0; j < floorParent.childCount; j++)
                    {
                        Transform child = floorParent.GetChild(j);
                        if (child.position == neighborPos)
                        {
                            hasNeighbor = true;
                            break;
                        }
                    }

                    if (!hasNeighbor && i < roomCreatorData.wallPrefabs.Count)
                    {
                        GameObject wall = Instantiate(roomCreatorData.wallPrefabs[i], wallParent);
                        wall.transform.position = neighborPos;
                    }
                }
            }
            else
            {
                Debug.LogWarning("Ray did not hit the Z=0 plane.");
            }

            @event.Use();
        }
    }

    private void CreateDoor(SceneView _sceneView)
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
                Transform tile = null;

                Transform floorParent = instantiedRoom.transform.GetChild(1);

                bool isFloorTile = false;
                for (int i = 0; i < floorParent.childCount; i++)
                {
                    Transform child = floorParent.GetChild(i);
                    if (child.position == snappedPos)
                    {
                        isFloorTile = true;
                        tile = child;
                        Debug.Log(isFloorTile);
                        break;
                    }
                }

                if (!isFloorTile || !tile)
                    return;

                Vector3[] directions = new Vector3[]
                {
                Vector3.down,
                Vector3.left,
                Vector3.right,
                Vector3.up
                };

                int wallsDestroyed = 0;
                Transform wallsParent = instantiedRoom.transform.GetChild(0);
                for (int i = 0; i < directions.Length; i++)
                {
                    Vector3 neighborPos = snappedPos + directions[i] * 0.32f;
                    Debug.Log(neighborPos);

                    for (int j = 0; j < wallsParent.childCount; j++)
                    {
                        Transform child = wallsParent.GetChild(j);
                        Debug.Log(child.position);
                        if (child.position == neighborPos)
                        {
                            DestroyImmediate(child.gameObject);
                            wallsDestroyed++;
                            break;
                        }
                    }
                }

                if (wallsDestroyed > 0)
                {
                    tile.SetParent(instantiedRoom.transform.GetChild(2));
                }
            }

            @event.Use();
        }
    }


}
