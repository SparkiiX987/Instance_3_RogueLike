using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Obstacle obstacle;

    public void UseObstacle(InputAction.CallbackContext context)
    {
        if (obstacle == null) { return; }

        if (context.started)
        {
            obstacle.Action();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        Obstacle _tempObstacle = collision.gameObject.GetComponent<Obstacle>();
        if (obstacle == null && _tempObstacle != null)
            obstacle = _tempObstacle;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Obstacle _tempObstacle = collision.gameObject.GetComponent<Obstacle>();
        if (obstacle != null && _tempObstacle != null)
            obstacle = null;
    }
}
