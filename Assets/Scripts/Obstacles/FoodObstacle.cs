using System.Threading;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class FoodObstacle : Obstacle
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Action();
    }
    public override void Action()
    {
         //Target of ennemy is now food (== this gameObject)
         activated = true;
         Debug.Log("Monster targeting food");
    }
}
