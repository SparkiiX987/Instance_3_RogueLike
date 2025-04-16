using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Transform selfTransform;
    private bool closed = true;
    private bool isMoving;
    private Vector3 finalRotation;

    void Start()
    {
        selfTransform = transform;
    }

    public void UseDoor(Vector2 _playerPosition)
    {
        if(isMoving is true) { return; }

        isMoving = true;
        float dir = 1;
        if (closed is not true)
        {
            dir *= -1;
        }
        RotateDoor(dir);
    }

    private void RotateDoor(float _dir)
    {
        finalRotation.Set(selfTransform.rotation.eulerAngles.x, selfTransform.rotation.eulerAngles.y, selfTransform.rotation.eulerAngles.z + _dir * 90);
        float time = 0.5f;
        selfTransform.DORotate(finalRotation, time);
        StartCoroutine(OpenningCooldown(time));
    }

    private IEnumerator OpenningCooldown(float _time)
    {
        yield return new WaitForSeconds(_time);
        isMoving = false;
        closed = !closed;
    }
}
