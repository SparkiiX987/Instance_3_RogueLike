using UnityEngine;

public class PepperSpray : UsableObject
{
    public GameObject sprayPrefab;
    private PepperSprayProjectile pepperSpray;

    public PepperSpray(int _price, string _name, string _description, int _type, GameObject _sprayPrefabs) : base(_price, _name, _description, _type)
    {
        sprayPrefab = _sprayPrefabs;
    }  

    public override void Action(GameObject _player)
    {
        AudioManager.Instance.PlaySound(AudioType.lacrymogene);
        pepperSpray = (Object.Instantiate(sprayPrefab, _player.transform.position, _player.transform.rotation)).GetComponent<PepperSprayProjectile>();
        pepperSpray.GetComponent<Collider2D>().enabled = false;
    }

    public void SetAchivement(AchievementsManager _achievementsManager)
    {
        pepperSpray.SetAchivementManager(_achievementsManager);
    }
}
