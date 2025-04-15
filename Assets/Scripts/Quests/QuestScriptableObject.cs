using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/Quest")]
public class QuestScriptableObject : ScriptableObject
{
    public int id;
    public int rewards;
    public string description;
    public SellableObject goalObject;
    public List<Sprite> customerAvailables;
}
