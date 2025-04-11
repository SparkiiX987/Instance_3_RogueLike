using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    //[Header("stats"), HideInInspector]
    //public Stats ennemyStats

    [Header("States"), HideInInspector]
    public IState[] states = new IState[4];
    public  IState activeState;

    [Header("PathFinding")]
    public float detectionRange;
    private Grid grid;
    private Vector2 playerPosition;
    private Vector2 nextPointToMove;
    private GameObject currentTile;
    private GameObject targetTile;
    private LayerMask cellLayer;

    private Transform selfTransform;

    [Header("Attack")]
    [SerializeField] private float attackRad = 1f;
    private float timer = 0f;
    private float attackMaxTimer = 1f;

    [Header("Targets"), HideInInspector]
    public PlayerControl targetPlayer;
    public ITargetable target;

    private void Awake()
    {
        //ennemyStats = GetComponent<Stats>();
        selfTransform = transform;
    }

    void Start()
    {
        StateInitialization();
        activeState = states[0];
    }

    public void ChangeState(string _state)
    {
        switch (_state)
        {
            case "Idle":
                {
                    activeState = states[0];
                    Debug.Log("Idle");
                    break;
                }
            case "Patrol":
                {
                    activeState = states[1];
                    Debug.Log("Patrol");
                    break;
                }
            case "Chase":
                {
                    activeState = states[2];
                    Debug.Log("Chase");
                    break;
                }
            case "Attack":
                {
                    activeState = states[3];
                    Debug.Log("Attack");
                    break;
                }
        }
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

    #region States
    public void ResetState()
    {
        targetPlayer = null;
        target = null;
        ChangeState("Idle");
    }

    public void PerformAttack()
    {
        activeState = states[3];
        Attack attack = (Attack)activeState;
        attack.target = this.target;
        attack.enemy = this;
        attack.Action();

        timer -= attackMaxTimer;

        if (targetPlayer != null && Vector3.Distance(targetPlayer.transform.position, selfTransform.position) >= detectionRange * 3)
        {
            ResetState();
        }
        else if (targetPlayer != null && Vector3.Distance(targetPlayer.transform.position, selfTransform.position) <= detectionRange * 3)
        {
            if (target == null)
            {
                ChangeState("Chase");
            }
        }
    }

    #endregion

    #region PathFiding

    public void GetMap(Grid _grid)
    {
        grid = _grid;
    }

    private GameObject GetTileNextTo(Vector2 target)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(selfTransform.position, 10, cellLayer);
        if (hits.Length == 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            Debug.LogError("Error : No points next to the entity " + gameObject.name);
        }

        GameObject tile = hits[0].gameObject;
        if (hits.Length == 0)
        {
            return tile;
        }

        for (int i = 1; i < hits.Length; i++)
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

        int tryNumber = 0;

        foreach (Link startNeighbor in _startNode.GetLinks())
        {
            float g = Vector2.Distance(_startNode.GetCellPosition(), startNeighbor.nodeTo.GetCellPosition());
            float h = Vector2.Distance(startNeighbor.nodeTo.GetCellPosition(), _goalNode.GetCellPosition());

            Link startLink = new Link(startNeighbor.nodeTo, g, h);
            startLink.parentLink = new Link(_startNode, 0f, h);
            openLinks.Add(startLink);
        }
        print(openLinks.Count);
        closedNodes.Add(_startNode);

        while (openLinks.Count > 0)
        {
            if (tryNumber++ >= 1000)
            {
                Debug.LogError("Error : Infinite loop detected !");
                return null;
            }

            openLinks.Sort((a, b) => a.fCost.CompareTo(b.fCost));
            Link currentLink = openLinks[0];
            openLinks.RemoveAt(0);

            Node currentNode = currentLink.nodeTo;
            closedNodes.Add(currentNode);

            if (currentNode == _goalNode)
                return ReconstructPath(currentLink);

            foreach (Link neighborLink in currentNode.GetLinks())
            {
                if (closedNodes.Contains(neighborLink.nodeTo)) continue;

                float tentativeG = currentLink.gCost + Vector2.Distance(currentNode.GetCellPosition(), neighborLink.nodeTo.GetCellPosition());
                Link existing = openLinks.Find(l => l.nodeTo == neighborLink.nodeTo);

                if (existing == null || tentativeG < existing.gCost)
                {
                    float h = Vector2.Distance(neighborLink.nodeTo.GetCellPosition(), _goalNode.GetCellPosition());
                    Link newLink = new Link(neighborLink.nodeTo, tentativeG, h);
                    newLink.parentLink = currentLink;

                    if (existing != null)
                        openLinks.Remove(existing);

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
        Debug.Log(activeState);

        if (targetPlayer != null && Vector3.Distance(targetPlayer.transform.position, selfTransform.position) >= detectionRange*3)
        {
            ResetState();
        }
        

        if (timer < attackMaxTimer)
            timer += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ITargetable _tempTarget = collision.GetComponent<ITargetable>();

        if (_tempTarget != null)
        {
            PlayerControl _player = collision.GetComponent<PlayerControl>();
            if (_player != null && Vector3.Distance(selfTransform.position, collision.transform.position) <= attackRad && activeState == states[2])
            {
                if (targetPlayer == null)
                    targetPlayer = _player;
                target = _tempTarget;
                ChangeState("Attack");
                PerformAttack();
            }
            else if (_player != null && (activeState == states[0] || activeState == states[1]))
            {
                target = _tempTarget;
                ChangeState("Chase");
            }
            else
            {
                Obstacle _obstacle = collision.GetComponent<Obstacle>();
                Debug.Log(_obstacle);
                if ( _obstacle != null && collision.GetComponent<FoodObstacle>() && _obstacle.activated)
                {
                    target = _tempTarget;
                    ChangeState("Attack");
                    PerformAttack();
                }
                else if (targetPlayer != null && _obstacle != null && activeState == states[2] && _obstacle.activated)
                {
                    target = _tempTarget;
                    ChangeState("Attack");
                    PerformAttack();
                }
            }
        }
        else
        {
            if (targetPlayer != null && Vector3.Distance(targetPlayer.transform.position, selfTransform.position) >= detectionRange * 3)
            {
                ResetState();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<ITargetable>() != null)
        {
            if (targetPlayer != null && target != null) { ChangeState("Chase"); }
        }
    }
}
