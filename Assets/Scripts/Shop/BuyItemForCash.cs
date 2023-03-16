using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyItemForCash : MonoBehaviour
{
    public enum Types
    {
        REMOVE_ADS, OPEN_CITY, OPEN_MEGAPOLIS
    }

    public Types type;

    public void BuyItem()
    {
        switch(type)
        {
            case Types.REMOVE_ADS:
                IAPManager.Instance.BuyNoAds();
                break;  
            case Types.OPEN_CITY:
                IAPManager.Instance.BuyCityMap();
                break;
            case Types.OPEN_MEGAPOLIS:
                IAPManager.Instance.BuyMegapolisMap();
                break;
        }
    }
}
