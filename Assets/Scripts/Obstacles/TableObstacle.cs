using UnityEngine;
using UnityEngine.InputSystem;

public class TableObstacle : Obstacle
{
    private bool isHoldingTable;
    private Transform transformTable;

    private void Start()
    {
        transformTable = GetComponent<Transform>();
        activated = true;
    }

    private void Update()
    {
        if (isHoldingTable && player == null)
        {
            isHoldingTable = false;
            transformTable.SetParent(null);
        }
    }

    public override void Action()
    {
        if (!isHoldingTable)
        {
            isHoldingTable = true;
            transformTable.SetParent(player.GetComponent<Transform>());
        }
        else
        {
            isHoldingTable = false;
            transformTable.SetParent(null);
        }
    }

}
