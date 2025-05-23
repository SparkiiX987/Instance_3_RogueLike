using UnityEngine;

[System.Serializable]
public class Link
{
    public float gCost;
    public float hCost;
    public float fCost => gCost + hCost;

    public Link parentLink;

    public Node nodeTo;

    public Link(Node _nodeTo, Vector2 _position, Vector2 _goalPosition)
    {
        nodeTo = _nodeTo;
        CalculateGCost(_position);
        CalculateHCost(_position, _goalPosition);
    }

    public Link(Node _nodeTo, Vector2 _position, Vector2 _goalPosition, float _gCost)
    {
        nodeTo = _nodeTo;
        gCost = _gCost;
        CalculateHCost(_position, _goalPosition);
    }

    public Link(Node _startNode, float _gCost, float _hCost)
    {
        nodeTo = _startNode;
        gCost = _gCost;
        hCost = _hCost;
    }


    public void CalculateGCost(Vector2 _position)
    {
        gCost = Vector2.Distance(_position, nodeTo.GetNodePosition());
    }

    public void CalculateHCost(Vector2 _position, Vector2 _goalPosition)
    {
        hCost = Vector2.Distance(_position, _goalPosition);
    }
}
