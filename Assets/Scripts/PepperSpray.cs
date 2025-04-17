using UnityEngine;

public class PepperSpray : UsableObject
{
    public override void Action()
    {
        AudioManager.Instance.PlaySound(AudioType.lacrymogene);
    }
}
