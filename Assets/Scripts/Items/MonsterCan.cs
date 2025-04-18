using UnityEngine;

public class MonsterCan : UsableObject
{
    public float bonusDuration;
    private float elapsedTime;
    private PlayerControl playerControl;

    public MonsterCan(int _price, string _name, string _description, int _type) : base(_price, _name, _description, _type)
    {
    }

    public override void Action(GameObject _player)
    {
        playerControl = _player.GetComponent<PlayerControl>();
        AudioManager.Instance.PlaySound(AudioType.drinkSoda);
    }
}
