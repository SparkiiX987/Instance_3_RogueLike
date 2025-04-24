using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [Header("Ennemy Body")]
    [SerializeField] private Ennemy ennemy;
    [SerializeField] private float speed;

    [Header("Target"), HideInInspector]
    public Transform target;

    //private void Start()
    //{
    //    if (ennemy.activeState == ennemy.states[2])
    //    {
    //        OnMovement
    //    }
    //}

    //public void OnMovement(Vector2 targetPosition)
    //{
    //    //Look at the target
    //    ennemy.transform.LookAt(targetPosition);

    //    //Move towards the player
    //    Vector3 direction = (Vector3)targetPosition - ennemy.transform.position;
    //    direction.z = 0;
    //    ennemy.transform.position += direction.normalized * speed * Time.deltaTime;
    //}
}
