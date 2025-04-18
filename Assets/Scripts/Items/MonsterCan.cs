using UnityEngine;

public class MonsterCan : UsableObject
{
    public float bonusDuration;
    private float elapsedTime;
    private PlayerControl playerControl;

    public MonsterCan(int _price, string _name, string _description) : base(_price, _name, _description)
    {
    }

    public override void Action(GameObject _player)
    {
        playerControl = _player.GetComponent<PlayerControl>();
        AudioManager.Instance.PlaySound(AudioType.drinkSoda);
    }
}
