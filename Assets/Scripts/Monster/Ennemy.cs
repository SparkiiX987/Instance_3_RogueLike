using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ennemy : MonoBehaviour
{
    //[Header("stats"), HideInInspector]
    //public Stats ennemyStats

    [Header("States"), HideInInspector]
    private IState[] states = new IState[4];
    private IState[] activeState;

    [Header("PathFinding")]
    public float detectionRange;
    private Grid grid;
    private Vector2 playerPosition;
    private Vector2 nextPointToMove;
    private GameObject currentTile;
    private GameObject targetTile;
    private LayerMask cellLayer;

    private Transform selfTransform;

    private void Awake()
    {
        //ennemyStats = GetComponent<Stats>();
        selfTransform = transform;
    }

    void Start()
    {
        StateInitialization();
    }


    private void StateInitialization()
    {
        states[0] = new Idle();
        states[1] = new Patrol();
        states[2] = new Chase();
        states[3] = new Attack();
    }

    public void SetPlayerPosition(Vector2 _position)
    {
        playerPosition = _position;
    }

    #region PathFiding

    public void GetMap(Grid _grid)
    {
        grid = _grid;
    }

    private GameObject GetTileNextTo(Vector2 target)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(selfTransform.position, 10, cellLayer);
        if(hits.Length == 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            throw new Exception("Error : No points next to the entity " + gameObject.name);
        }

        GameObject tile = hits[0].gameObject;
        if(hits.Length == 0)
        {
            return tile;
        }

        for(int i = 1; i < hits.Length; i++)
        {
            if (Vector2.Distance(target, tile.transform.position) < Vector2.Distance(target, hits[i].transform.position))
            {
                tile = hits[i].gameObject;
            }
        }

        return tile;
    }

    public List<Node> TestPathFinding(Node _startNode, Node _goalNode)
    {
        return FindPathToCell(_startNode, _goalNode);
    }
    private List<Node> FindPathToCell(Node _startNode, Node _goalNode)
    {
        List<Link> openLinks = new List<Link>();
        HashSet<Node> closedNodes = new HashSet<Node>();

        Link startLink = new Link(_startNode, _startNode.GetCellPosition(), _goalNode.GetCellPosition());

        openLinks.Add(startLink);

        while(openLinks.Count > 0)
        {
            openLinks.Sort((linkA, linkB) => linkA.fCost.CompareTo(linkB.fCost));
            Link currentLink = openLinks[0];
            openLinks.RemoveAt(0);

            Node currentNode = currentLink.nodeTo;
            closedNodes.Add(currentNode);

            if(currentNode == _goalNode)
            { return ReconstructPath(currentLink); }

            foreach(Link neighborLink in currentNode.GetLinks())
            {
                if (closedNodes.Contains(neighborLink.nodeTo)) { continue; }

                float tentaiveG = currentLink.gCost + neighborLink.gCost;
                Link existingLinkToNeighbor = openLinks.Find(openLinks => openLinks.nodeTo == neighborLink.nodeTo);

                if(existingLinkToNeighbor == null || tentaiveG < existingLinkToNeighbor.gCost)
                {
                    Link newLink = new Link(currentNode, neighborLink.nodeTo.GetCellPosition(), _goalNode.GetCellPosition(), tentaiveG);
                    newLink.parentLink = currentLink;

                    if(existingLinkToNeighbor != null)
                    {
                        openLinks.Remove(existingLinkToNeighbor);
                    }

                    openLinks.Add(newLink);
                }
            }
        }

        return null;
    }

    private List<Node> ReconstructPath(Link _endLink)
    {
        List<Node> path = new List<Node>();
        Link current = _endLink;

        while (current != null)
        {
            if (current.nodeTo != null)
            {
                path.Add(current.nodeTo);
            }
            current = current.parentLink;
        }

        path.Reverse();
        return path;
    }

    #endregion

    void Update()
    {
        
    }
}
