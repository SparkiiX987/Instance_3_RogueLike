using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private List<Link> links = new List<Link>();
    public Node parent;

    public Vector2 GetNodePosition() => transform.position;

    public List<Link> GetLinks() { return links; }

    public void AddLink(Link _newLink)
    {
        if(links.Contains(_newLink))
        { return; }

        links.Add(_newLink);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var link in links)
        {
            if (link.nodeTo != null)
                Gizmos.DrawLine(transform.position, link.nodeTo.transform.position);
        }
    }

}
