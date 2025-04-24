using UnityEngine;

public class MonsterCan : UsableObject
{
    private float bonusDuration;

    public MonsterCan(int _price, string _name, string _description, int _type, float _bonusDuration) : base(_price, _name, _description, _type)
    {
        bonusDuration = _bonusDuration;
    }

    public override void Action(GameObject _player)
    {
        _player.GetComponent<PlayerControl>().StartCafeinatePlayer(bonusDuration);
        AudioManager.Instance.PlaySound(AudioType.drinkSoda);
    }
}
