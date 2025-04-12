using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomEditorTools : EditorWindow
{
    public RoomCreatorData roomCreatorData;
    private string roomName;
    private bool isEditingFloor;
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
            if (isEditingFloor)
            {
                if (GUILayout.Button("Stop editing"))
                {
                    isEditingFloor = false;
                    SceneView.duringSceneGui -= OnSceneGUI;
                }
            }
            else
            {
                if (GUILayout.Button("Edit floor"))
                {
                    isEditingFloor = true;
                    SceneView.duringSceneGui += OnSceneGUI;
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
                Debug.Log(snappedPos);
                if(!IsInGrid(instantiedRoom, snappedPos))
                {
                    return;
                }

                Transform floorParent = instantiedRoom.transform.GetChild(1);

                for (int i = 0; i < floorParent.childCount; i++)
                {
                    Transform child = floorParent.GetChild(i);
                    if (child.position == snappedPos)
                    {
                        DestroyImmediate(child.gameObject);
                        break;
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
                        GameObject wall = Instantiate(roomCreatorData.wallPrefabs[i], floorParent);
                        wall.transform.position = neighborPos;
                        wall.tag = "Tile";
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
        if(instantiedRoom == null)
        { return; }

        GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(instantiedRoom, path, InteractionMode.UserAction);
        Debug.Log("Room saved as prefab at: " + path);
    }

}
