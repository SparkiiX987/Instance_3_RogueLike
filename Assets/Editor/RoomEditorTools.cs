using UnityEditor;
using UnityEngine;

public class RoomEditorTools : EditorWindow
{
    public RoomCreatorData roomCreatorData;
    private string roomName;
    private bool isEditingFloor;
    private bool isEditingDoors;
    private bool isPlacingProps;
    private bool isEditingRoom;
    private GameObject emptyRoom;
    private GameObject instantiedRoom;
    private int selectedSpriteIndex = 0;
    private GameObject previewObject;
    private Quaternion currentRotation = Quaternion.identity;

    [MenuItem("Tools/Room Editor")]
    public static void ShowWindow()
    {
        GetWindow<RoomEditorTools>("Room Editor");
    }
    private void OnDisable()
    {
        if (previewObject != null)
            { DestroyImmediate(previewObject); }
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
                    SceneView.duringSceneGui -= PlaceFloorTile;
                    SceneView.duringSceneGui -= CreateDoor;
                    { DestroyImmediate(previewObject); }
                }
            }
            else if (isPlacingProps)
            {
                SpritePropsSelection();

                GUILayout.Label("You can rotate by using A and E");

                if (GUILayout.Button("Stop placing"))
                {
                    isPlacingProps = false;
                    SceneView.duringSceneGui -= PlaceProps;
                    if (previewObject != null)
                    { DestroyImmediate(previewObject); }
                }
            }
            else
            {
                if (GUILayout.Button("Edit floor"))
                {
                    isEditingFloor = true;
                    SceneView.duringSceneGui += PlaceFloorTile;
                }

                if (GUILayout.Button("Edit Doors"))
                {
                    isEditingDoors = true;
                    SceneView.duringSceneGui += CreateDoor;
                }

                if (GUILayout.Button("Place props"))
                {
                    isPlacingProps = true;
                    SceneView.duringSceneGui += PlaceProps;
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

    private void SpritePropsSelection()
    {
        string[] spriteNames = new string[roomCreatorData.propsSpites.Count];
        for (int i = 0; i < roomCreatorData.propsSpites.Count; i++)
        {
            spriteNames[i] = roomCreatorData.propsSpites[i] != null ? roomCreatorData.propsSpites[i].name : "None";
        }

        selectedSpriteIndex = EditorGUILayout.Popup("Select Sprite", selectedSpriteIndex, spriteNames);

        if (selectedSpriteIndex < roomCreatorData.propsSpites.Count && roomCreatorData.propsSpites[selectedSpriteIndex] != null)
        {
            GUILayout.Label("Preview:");
            GUILayout.Label(AssetPreview.GetAssetPreview(roomCreatorData.propsSpites[selectedSpriteIndex]), GUILayout.Width(64), GUILayout.Height(64));
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

        return  _tilePosition.x > (roomPosition.x - 4)
             && _tilePosition.x < (roomPosition.x + 4)
             && _tilePosition.y > (roomPosition.y - 4)
             && _tilePosition.y < (roomPosition.y + 4);
    }

    public void SaveAsPrefab(string path)
    {
        if (instantiedRoom == null)
        { return; }

        if(roomName == "" || roomName == null)
        {
            Debug.Log("You should give a name to your room before save it");
            return;
        }

        GameObject prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(instantiedRoom, path, InteractionMode.UserAction);
        Debug.Log("Room saved as prefab at: " + path);
    }

    private void PlaceProps(SceneView _sceneView)
    {
        Event @event = Event.current;

        Ray worldRay = HandleUtility.GUIPointToWorldRay(@event.mousePosition);
        Plane plane = new Plane(Vector3.forward, Vector3.zero);

        if (plane.Raycast(worldRay, out float distance))
        {
            Vector3 worldPos = worldRay.GetPoint(distance);
            Vector3 snappedPos = instantiedRoom.GetComponent<GridGizmo>().SnapToGrid(worldPos);

            if (previewObject == null)
            {
                previewObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
                previewObject.name = "PreviewProp";
                previewObject.hideFlags = HideFlags.HideAndDontSave;
                DestroyImmediate(previewObject.GetComponent<Collider>());
            }

            previewObject.transform.position = snappedPos + new Vector3(0, 0, -0.1f);
            previewObject.transform.rotation = currentRotation;

            Sprite selectedSprite = roomCreatorData.propsSpites.Count > selectedSpriteIndex ? roomCreatorData.propsSpites[selectedSpriteIndex] : null;
            if (selectedSprite != null)
            {
                Material mat = new Material(Shader.Find("Sprites/Default"));
                mat.mainTexture = selectedSprite.texture;
                previewObject.GetComponent<MeshRenderer>().sharedMaterial = mat;
            }

            if (@event.type == EventType.KeyDown)
            {
                if (@event.keyCode == KeyCode.A)
                {
                    currentRotation *= Quaternion.Euler(0, 0, 90f);
                    @event.Use();
                }
                else if (@event.keyCode == KeyCode.E)
                {
                    currentRotation *= Quaternion.Euler(0, 0, -90f);
                    @event.Use();
                }
            }

            if (@event.type == EventType.MouseDown && @event.button == 0 && !@event.alt)
            {
                if (!IsInGrid(instantiedRoom, snappedPos))
                    { return; }

                GameObject emptyProp = new GameObject("Prop");
                emptyProp.transform.position = snappedPos;
                emptyProp.transform.rotation = currentRotation;
                emptyProp.transform.parent = instantiedRoom.transform.GetChild(3);

                SpriteRenderer sr = emptyProp.AddComponent<SpriteRenderer>();
                sr.sprite = selectedSprite;
                sr.sortingOrder = 1;

                @event.Use();
            }
        }

        SceneView.RepaintAll();
    }

    private void PlaceFloorTile(SceneView _sceneView)
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

                for (int i = wallParent.childCount - 1; i >= 0; i--)
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
                    Vector3 neighborPos = snappedPos + directions[i];
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
                        for (int j = wallParent.childCount - 1; j >= 0; j--)
                        {
                            Transform existingWall = wallParent.GetChild(j);
                            if (existingWall.position == snappedPos)
                            {
                                Vector3 existingDir = existingWall.up;
                                if (Vector3.Angle(existingDir, directions[i]) < 1f)
                                {
                                    DestroyImmediate(existingWall.gameObject);
                                }
                            }
                        }

                        GameObject wall = Instantiate(roomCreatorData.wallPrefabs[i], wallParent);
                        wall.transform.position = snappedPos;
                        wall.transform.rotation = Quaternion.LookRotation(Vector3.forward, directions[i]);
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
                    Vector3 neighborPos = snappedPos + directions[i];
                    Debug.Log(neighborPos);

                    for (int j = 0; j < wallsParent.childCount; j++)
                    {
                        Transform child = wallsParent.GetChild(j);
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
