using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Ennemy : MonoBehaviour
{
    [Header("stats"), HideInInspector]
    public Stats ennemyStats;

    [Header("States"), HideInInspector]
    public IState[] states = new IState[4];
    public  IState activeState;

    [Header("PathFinding")]
    public float detectionRange;
    public Grid grid;

    [Header("PathFinding"), HideInInspector]
    public Vector2 nextPointToMove;
    private Vector2 playerPosition;
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

    [Header("Idle"), HideInInspector]
    public float idleTimeMax = 3f;
    public float idleTimeMin = 1f;

    [Header("Path")]
    public List<Node> nodes = new List<Node>();
    private Node startNode, endNode;
    public List<Node> path = new List<Node>();
    public int currentIndexNode = 0;


    private void Awake()
    {
        ennemyStats = GetComponent<Stats>();
        selfTransform = transform;
    }

    void Start()
    {
        StateInitialization();
        ChangeState("Idle");
        GetNodesMap();
    }

    public void ChangeState(string _state)
    {
        switch (_state)
        {
            case "Idle":
                {
                    activeState = states[0];

                    Idle idleEnnemy = (Idle)activeState;
                    idleEnnemy.ennemy = this;
                    idleEnnemy.onIdle.AddListener(() => PerformIdle());
                    idleEnnemy.Action();

                    break;
                }
            case "Patrol":
                {
                    activeState = states[1];

                    Patrol patrol = activeState as Patrol;
                    patrol.ennemy = this;
                    patrol.Action();
                    break;
                }
            case "Chase":
                {
                    activeState = states[2];
                    break;
                }
            case "Attack":
                {
                    activeState = states[3];
                    PerformAttack();
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

    #region Attack

    public void ResetState()
    {
        targetPlayer = null;
        target = null;
        ChangeState("Idle");
        startNode = GetNearestNode();
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

    private Node GetNearestNode()
    {
        Node node = null;
        float distanceMin = Vector3.Distance(nodes[0].transform.position, selfTransform.position);
        float distance = 0f;
        for (int i = 1; i < nodes.Count; i++)
        {
            distance = Vector3.Distance(nodes[i].transform.position, selfTransform.position);
            if (distance < distanceMin)
            {
                node = nodes[i];
                distanceMin = distance;
            }
        }
        return node;
    }

    private void GetNodesMap()
    {
        Transform transformMap = GameObject.Find("graph").transform;
        foreach (Transform child in transformMap)
        {
            nodes.Add(child.GetComponent<Node>());
        }
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
            float g = Vector2.Distance(_startNode.GetNodePosition(), startNeighbor.nodeTo.GetNodePosition());
            float h = Vector2.Distance(startNeighbor.nodeTo.GetNodePosition(), _goalNode.GetNodePosition());

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

                float tentativeG = currentLink.gCost + Vector2.Distance(currentNode.GetNodePosition(), neighborLink.nodeTo.GetNodePosition());
                Link existing = openLinks.Find(l => l.nodeTo == neighborLink.nodeTo);

                if (existing == null || tentativeG < existing.gCost)
                {
                    float h = Vector2.Distance(neighborLink.nodeTo.GetNodePosition(), _goalNode.GetNodePosition());
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

    #region movement

    public void OnMovement(Vector2 targetPosition)
    {
        if (targetPosition != null)
        {
            Vector3 look = (Vector3)targetPosition - transform.position;
            float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

            Vector3 direction = (Vector3)targetPosition - selfTransform.position;
            selfTransform.position += direction.normalized * ennemyStats.speed * Time.deltaTime;
        }
    }

    #endregion

    #region Idle

    public void PerformIdle()
    {
        StartCoroutine(OnIdle());
    }

    public IEnumerator OnIdle()
    {
        float time = Random.Range(idleTimeMin, idleTimeMax);
        yield return new WaitForSecondsRealtime(time);
        ChangeState("Patrol");
    }

    #endregion

    void Update()
    {
        Debug.Log(activeState);

        if (targetPlayer != null && Vector3.Distance(targetPlayer.transform.position, selfTransform.position) >= detectionRange * 3)
        {
            ResetState();
        }

        if (path == null || currentIndexNode == path.Count || path.Count == 0)
        {
            currentIndexNode = 0;
            if (nodes.Count >= 2)
            {
                if (startNode != null && endNode == null)
                {
                    do
                    {
                        endNode = nodes[Random.Range(0, nodes.Count)];
                        path = TestPathFinding(startNode, endNode);
                    } while (startNode == endNode || path == null);
                }
                else
                {
                    do
                    {
                        startNode = endNode ?? nodes[Random.Range(0, nodes.Count)];
                        endNode = nodes[Random.Range(0, nodes.Count)];
                        path = TestPathFinding(startNode, endNode);
                    } while (startNode == endNode || path == null);
                }

            }
        }

        if (activeState != states[0] && activeState != states[3] && path != null && currentIndexNode < path.Count)
        {
            if (Vector3.Distance(nextPointToMove, selfTransform.position) >= 0.2f)
                OnMovement(nextPointToMove);
            else
            {
                currentIndexNode++;
                ChangeState("Idle");
            }
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
                { 
                    targetPlayer = _player;
                }
                target = _tempTarget;
                ChangeState("Attack");
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
                }
                else if (targetPlayer != null && _obstacle != null && activeState == states[2] && _obstacle.activated)
                {
                    target = _tempTarget;
                    ChangeState("Attack");
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
