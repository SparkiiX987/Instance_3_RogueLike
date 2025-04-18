using UnityEngine;

public class EmptyBottle : UsableObject
{
    public override void Action()
    {
        AudioManager.Instance.PlaySound(AudioType.Throw);
    }
}
