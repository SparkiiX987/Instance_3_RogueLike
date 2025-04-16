using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    public ShopOffer activeOffer;
    public ShopOffer[] offers = new ShopOffer[5];

    public Quest[] questsAvailables = new Quest[3];

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("trop de fois appel�");
            Destroy(this);
        }
        Instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetActiveOffer(ShopOffer activeOffers)
    {

    }

    public void ActualiseOffers()
    {

    }


}
