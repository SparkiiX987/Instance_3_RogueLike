using System.Collections;
using UnityEngine;

public class MonsterCan : UsableObject
{
    [SerializeField] private float bonusDuration;
    private float elapsedTime;
    private PlayerControl playerControl;
    public override void Action(GameObject _player)
    {
        playerControl = _player.GetComponent<PlayerControl>();
        StartCoroutine(staminaBuff());
    }

    private IEnumerator staminaBuff()
    {
        if (!(bonusDuration > elapsedTime)) yield break;
        elapsedTime += Time.deltaTime;
        playerControl.stamina = playerControl.staminaMax;
        yield return null;
        StartCoroutine(staminaBuff());
    }
}
