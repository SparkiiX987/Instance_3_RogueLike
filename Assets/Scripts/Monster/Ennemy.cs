using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    [Header("stats"), HideInInspector]
    public Stats ennemyStats;
    public float amplificater = 1f;
    public float stunDuration;
    private float stunTimer;

    [Header("States"), HideInInspector]
    public IState[] states = new IState[4];
    public  IState activeState;
    public bool isStunned = false;

    [Header("PathFinding")]
    public float detectionRange;
    public Grid grid;

    [Header("PathFinding"), HideInInspector]
    public Vector2 nextPointToMove;
    private Vector2 playerPosition;

    private Transform selfTransform;
    private Animator animator;

    [Header("Attack")]
    [SerializeField] private float attackRad = 2f;
    private float timer = 0f;
    private float attackMaxTimer = 1f;

    [Header("Targets")]
    public PlayerControl targetPlayer;
    public ITargetable target;

    [Header("Idle"), HideInInspector]
    public float idleTimeMax = 3f;
    public float idleTimeMin = 1f;

    [Header("Path"), HideInInspector]
    public List<Node> nodes = new List<Node>();
    private Node startNode, endNode;
    public List<Node> path = new List<Node>();
    public int currentIndexNode = 0;

    [Header("Patrol")]
    private int currentPatrolsDone = 0;
    [SerializeField] private int patrolsDoneBeforeGoingToPlayer;
    [SerializeField] private PlayerControl playerControl;

    private void Awake()
    {
        ennemyStats = GetComponent<Stats>();
        selfTransform = transform;
    }

    void Start()
    {
        StateInitialization();
        StartCoroutine(GetNodesMap());
        ResetState();
        playerControl = selfTransform.parent.GetChild(0).GetComponent<PlayerControl>();
    }

    public void SetPlayerPosition(Vector2 _position)
    {
        playerPosition = _position;
    }

    #region Handling States
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
                    AudioManager.Instance.PlaySound(AudioType.monsterRoaming);
                    break;
                }
            case "Chase":
                {
                    activeState = states[2];
                    Chase chase = activeState as Chase;
                    chase.ennemy = this;
                    chase.multiplicater = 3f;
                    chase.Action();
                    AudioManager.Instance.StopSound(AudioType.monsterRoaming);
                    break;
                }
            case "Attack":
                {
                    activeState = states[3];
                    Attack attack = (Attack)activeState;
                    attack.target = this.target;
                    attack.enemy = this;
                    AudioManager.Instance.StopSound(AudioType.monsterRoaming);

                    if (timer >= attackMaxTimer)
                    {
                        PerformAttack(attack);
                        timer -= attackMaxTimer;
                    }
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

    public void ResetState()
    {
        targetPlayer = null;
        target = null;
        ChangeState("Idle");
        startNode = GetNearestNode(selfTransform.position);
        endNode = null;
        path.Clear();
        nextPointToMove = startNode.transform.position;
        amplificater = 1f;
    }

    public void HandlingPatrolState()
    {
        if (activeState != states[1]) { return; }

        if (path == null || currentIndexNode == path.Count || path.Count == 0)
        {
            SetNewPathToPatrol();
        }
    }
    private void HandleChaseState()
    {
        if (activeState != states[2]) { return; }
        if (targetPlayer == null) { ResetState(); return; }

        startNode = GetNearestNode(selfTransform.position);
        endNode = GetNearestNode(targetPlayer.transform.position);
        if (endNode == startNode) { return; }

        List<Node> testpath = FindPathToCell(startNode, endNode);

        if (testpath != null && PathChanged(testpath))
        {
            currentIndexNode = 0;
            path = testpath;
            nextPointToMove = startNode.transform.position;
        }
    }

    #endregion

    #region Attack
    public void PerformAttack(Attack attack)
    {
        attack.Action();
        if (targetPlayer != null && Vector3.Distance(targetPlayer.transform.position, selfTransform.position) >= detectionRange * 3) { ResetState(); }

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

    private Node GetNearestNode(Vector2 position)
    {
        if (nodes.Count < 2) { return null; }
        Node node = nodes[0];
        float distanceMin = Vector3.Distance(nodes[0].transform.position, position);
        float distance = 0f;
        for (int i = 1; i < nodes.Count; i++)
        {
            distance = Vector3.Distance(nodes[i].transform.position, position);
            if (distance < distanceMin)
            {
                node = nodes[i];
                distanceMin = distance;
            }
        }
        return node;
    }

    private IEnumerator GetNodesMap()
    {
        while (true)
        {
            GameObject graphGameObject = GameObject.Find("graph");
            if (!graphGameObject)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }
            Transform transformMap = graphGameObject.transform;
            foreach (Transform child in transformMap)
            {
                nodes.Add(child.GetComponent<Node>());
            }
            break;
        }
    }

    public List<Node> TestPathFinding(Node _startNode, Node _goalNode)
    {
        return FindPathToCell(_startNode, _goalNode);
    }

    private List<Node> FindPathToCell(Node _startNode, Node _goalNode)
    {
        if (_startNode == null || _goalNode == null) { return null; }
        List<Link> openLinks = new List<Link>();
        HashSet<Node> closedNodes = new HashSet<Node>();

        int tryNumber = 0;

        foreach (Link startNeighbor in _startNode.GetLinks())
        {
            if (_startNode is null || _goalNode is null) { continue; }
            float g = Vector2.Distance(_startNode.GetNodePosition(), startNeighbor.nodeTo.GetNodePosition());
            float h = Vector2.Distance(startNeighbor.nodeTo.GetNodePosition(), _goalNode.GetNodePosition());

            Link startLink = new Link(startNeighbor.nodeTo, g, h);
            startLink.parentLink = new Link(_startNode, 0f, h);
            openLinks.Add(startLink);
        }
        //print(openLinks.Count);
        closedNodes.Add(_startNode);

        while (openLinks.Count > 0)
        {
            if (tryNumber++ >= 10000)
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

    private bool PathChanged(List<Node> _newPath)
    {
        if (path.Count != _newPath.Count) {  return true; }
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i] != _newPath[i]) { return true; }
        }

        return false;
    }

    private void SetNewPathToPatrol()
    {
        currentIndexNode = 0;
        print((currentPatrolsDone + 1));
        print(patrolsDoneBeforeGoingToPlayer);
        currentPatrolsDone = (currentPatrolsDone + 1) % patrolsDoneBeforeGoingToPlayer;
        if (currentPatrolsDone > patrolsDoneBeforeGoingToPlayer - 1) { SetPathToPlayer();  return; }

        startNode = GetNearestNode(selfTransform.position);
        int tryNumbers = 0;
        do
        {
            endNode = nodes[Random.Range(0, nodes.Count)];
            path = TestPathFinding(startNode, endNode);
            tryNumbers++;
        } while ((startNode == endNode || path == null || path.Count > 20) && tryNumbers < 1000);

        if (path == null) { SetPathToPlayer(); }
    }

    private void SetPathToPlayer()
    {
        currentIndexNode = patrolsDoneBeforeGoingToPlayer - 1;
        startNode = GetNearestNode(selfTransform.position);
        int tryNumbers = 0;
        do
        {
            endNode = GetNearestNode(targetPlayer.transform.position);
            path = TestPathFinding(startNode, endNode);
            tryNumbers++;
        } while ((startNode == endNode || path == null || path.Count > 20) && tryNumbers < 1000);
    }

    #endregion

    #region Movement

    private void HandlingMovementEnemy()
    {
        if ( (activeState == states[1] || activeState == states[2]) && path != null && currentIndexNode < path.Count)
        {
            if (Vector3.Distance(nextPointToMove, selfTransform.position) >= 0.2f) { MooveTowardsTarget(nextPointToMove); }
            else
            {
                currentIndexNode++;
                if (activeState == states[1]) { ChangeState("Idle"); return; }
                if (currentIndexNode < path.Count) { nextPointToMove = path[currentIndexNode].transform.position; }
            }
        }
    }

    public void MooveTowardsTarget(Vector2 targetPosition)
    {
        if (isStunned) return;
        if (targetPosition != null)
        {
            LookAtTarget(targetPosition);

            Vector3 direction = (Vector3)targetPosition - selfTransform.position;
            selfTransform.position += direction.normalized * ennemyStats.speed * amplificater * Time.deltaTime;
        }
    }

    public void LookAtTarget(Vector2 targetPosition)
    {
        Vector3 look = (Vector3)targetPosition - transform.position;
        float angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
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
        if (isStunned)
        {
           stunTimer += Time.deltaTime; 
        }

        if (stunTimer >= stunDuration)
        {
            isStunned = !isStunned;
            stunTimer = 0f;
        }

        HandleChaseState();
        HandlingPatrolState();
        HandlingMovementEnemy();

        //Player's too far, ennemy stops chasing him
        if (targetPlayer != null && Vector3.Distance(targetPlayer.transform.position, selfTransform.position) >= detectionRange * 3) { ResetState(); Debug.Log("Resetting"); }

        //Timer for attack
        if (timer < attackMaxTimer) { timer += Time.deltaTime; }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ITargetable _tempTarget = collision.GetComponent<ITargetable>();

        if (_tempTarget != null)
        {
            PlayerControl _player = collision.GetComponent<PlayerControl>();
            if (_player != null)
            {
                if (Vector3.Distance(selfTransform.position, collision.transform.position) <= attackRad && (activeState == states[2] || activeState == states[3]))
                {
                    target = _tempTarget;
                    ChangeState("Attack");
                }
                else if (activeState == states[0] || activeState == states[1])
                {
                    if (targetPlayer == null) { targetPlayer = _player; }
                    target = _tempTarget;
                    ChangeState("Chase");
                }
            }

            Obstacle _obstacle = collision.GetComponent<Obstacle>();
            if (_obstacle != null)
            {
                if (collision.GetComponent<FoodObstacle>() && _obstacle.activated && Vector3.Distance(_obstacle.transform.position, selfTransform.position) <= attackRad)
                {
                    target = _tempTarget;
                    ChangeState("Attack");
                }
                else if (targetPlayer != null && _obstacle.activated && (activeState == states[2] || activeState == states[3]) && Vector3.Distance(_obstacle.transform.position, selfTransform.position) <= attackRad)
                {
                    target = _tempTarget;
                    ChangeState("Attack");
                }
            }
        }
        else
        {
            if (targetPlayer != null && Vector3.Distance(targetPlayer.transform.position, selfTransform.position) >= detectionRange * 3) { ResetState(); }
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
