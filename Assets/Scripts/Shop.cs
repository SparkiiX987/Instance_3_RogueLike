using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance;

    public ShopOffer activeOffer;
    public ShopOffer[] offers = new ShopOffer[5];

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("trop de fois appel�");
        }
        Instance = this;
    }

    public void SetActiveOffer(ShopOffer activeOffers)
    {

    }

    public void ActualiseOffers()
    {

    }
}
