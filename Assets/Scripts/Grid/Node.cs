using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Link> links = new List<Link>();
    public Node parent;
    public float connectionRange;

    public Vector2 GetNodePosition() => transform.position;

    public List<Link> GetLinks() { return links; }

    public void AddLink(Link _newLink)
    {
        if(links.Contains(_newLink))
        { return; }

        links.Add(_newLink);
    }

    public void ConnectToNearbyNodes()
    {
        Node[] allNodes = FindObjectsByType<Node>(FindObjectsSortMode.None);
        foreach (Node node in allNodes)
        {
            if (node == this)
            { continue; }

            float dist = Vector2.Distance(GetNodePosition(), node.GetNodePosition());
            if (dist <= connectionRange)
            {
                AddLink(new Link(node, dist, 0f));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Link link in links)
        {
            if (link.nodeTo != null)
                Gizmos.DrawLine(transform.position, link.nodeTo.transform.position);
        }
    }

}
