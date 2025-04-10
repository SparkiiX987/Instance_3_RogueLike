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
        if (obstacle == null)
            obstacle = collision.gameObject.GetComponent<Obstacle>();
    }

    private void nTriggerExit2D(Collider2D collision)
    {
        if (obstacle != null)
            obstacle = null;
    }
}
