using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomCreatorData", menuName = "Scriptable Objects/RoomCreatorData")]
public class RoomCreatorData : ScriptableObject
{
    public List<GameObject> floorPrefabs = new List<GameObject>();
    public List<GameObject> wallPrefabs = new List<GameObject>();
    public List<Sprite> propsSpites = new List<Sprite>();

}
