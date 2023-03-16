using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckMaps : MonoBehaviour
{
    private BuyMapForCoins _mapCoins;

    public Image[] Maps;
    public Sprite SelectedMap, NotSelectedMap;

    private void Start()
    {
        WhichMapSelected();

        _mapCoins = GetComponent<BuyMapForCoins>();

        if(PlayerPrefs.GetString("City") == "Open")
        {
            _mapCoins.CityCoins.SetActive(false);
            _mapCoins.CityCash.SetActive(false);
            _mapCoins.CheckCity.SetActive(true);
        }

        if (PlayerPrefs.GetString("Megapolis") == "Open")
        {
            _mapCoins.MegapolisCoins.SetActive(false);
            _mapCoins.MegapolisCash.SetActive(false);
            _mapCoins.CheckMegapolis.SetActive(true);
        }
    }

    public void WhichMapSelected()
    {
        switch(PlayerPrefs.GetInt("NowMap"))
        {
            case 2:
                Maps[0].sprite = NotSelectedMap;
                Maps[1].sprite = SelectedMap;
                Maps[2].sprite = NotSelectedMap;
                break;
            case 3:
                Maps[0].sprite = NotSelectedMap;
                Maps[1].sprite = NotSelectedMap;
                Maps[2].sprite = SelectedMap;
                break;
            default:
                Maps[0].sprite = SelectedMap;
                Maps[1].sprite = NotSelectedMap;
                Maps[2].sprite = NotSelectedMap;
                break;
        }
    }    
}
