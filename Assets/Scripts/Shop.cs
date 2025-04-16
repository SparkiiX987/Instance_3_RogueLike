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
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
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
