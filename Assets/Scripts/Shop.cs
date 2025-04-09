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
            Debug.Log("trop de fois appelé");
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
