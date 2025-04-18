using UnityEngine;

public class MonsterCan : UsableObject
{
    public override void Action()
    {
        AudioManager.Instance.PlaySound(AudioType.drinkSoda);
    }
}
