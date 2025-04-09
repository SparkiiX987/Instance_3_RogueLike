using System.Threading;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class FoodObstacle : Obstacle
{
    [SerializeField] private Transform transformFood;
    [SerializeField] private float timerMax;
    private float timer = 0f;

    private void Update()
    {
        Action();
    }

    public override void Action()
    {
        if (enemy != null)
        {
            //Monster attack it
            Debug.Log("Monster in a zone");
        }
    }
}
