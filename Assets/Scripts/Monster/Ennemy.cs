using System;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    //[Header("stats"), HideInInspector]
    //public Stats ennemyStats

    [Header("States"), HideInInspector]
    private IState[] states = new IState[4];
    private IState[] activeState;

    [Header("PathFinding")]
    public float detectionRange;
    private List<GameObject> mapNodes;
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

    public void GetMap(List<GameObject> _map)
    {
        mapNodes = _map;
    }

    private void GetCurrentTile()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(selfTransform.position, 10, cellLayer);
        if(hits.Length == 0)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            throw new Exception("Error : No points next to the entity " + gameObject.name);
        }

        currentTile = hits[0].gameObject;
        if(hits.Length == 0)
        {
            return;
        }

        for(int i = 1; i < hits.Length; i++)
        {
            if (Vector2.Distance(selfTransform.position, currentTile.transform.position) < Vector2.Distance(selfTransform.position, hits[i].transform.position);
            {
                currentTile = hits[i].gameObject;
            }
        }
    }

    private void FindPathToPlayer()
    {

    } 

    #endregion

    void Update()
    {
        
    }
}
