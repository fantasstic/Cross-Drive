using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyMapForCoins : MonoBehaviour
{
    private AudioSource _buyButtonAudioSource;

    public Animation CoinAnimation;
    public GameObject CityCoins, MegapolisCoins, CityCash, MegapolisCash, CheckCity, CheckMegapolis;
    public TMP_Text CoinsCount, SlowCount;
    public AudioClip Success, FaildBuy;

    private void Start()
    {
        _buyButtonAudioSource = GetComponent<AudioSource>();
    }

    public void BuyNewMap(int needCoins)
    {
        int coins = PlayerPrefs.GetInt("Coins");
        int slowCount = PlayerPrefs.GetInt("Slow Count");
        if(coins < needCoins)
        {
            if (PlayerPrefs.GetString("Music") != "No")
            {
                _buyButtonAudioSource.clip = FaildBuy;
                _buyButtonAudioSource.Play();

            }

            CoinAnimation.Play();
        }
        else
        {
            switch(needCoins)
            {
                case 1:
                    slowCount++;
                    break;
                case 1000:
                    PlayerPrefs.SetString("City", "Open");
                    PlayerPrefs.SetInt("NowMap", 2);
                    GetComponent<CheckMaps>().WhichMapSelected();

                    CityCoins.SetActive(false);
                    CityCash.SetActive(false);
                    CheckCity.SetActive(true);
                    break;
                case 5000:
                    PlayerPrefs.SetString("Megapolis", "Open");
                    PlayerPrefs.SetInt("NowMap", 3);
                    GetComponent<CheckMaps>().WhichMapSelected();

                    MegapolisCoins.SetActive(false);
                    MegapolisCash.SetActive(false);
                    CheckMegapolis.SetActive(true);
                    break;

            }

            int nowCoins = coins - needCoins;
            CoinsCount.text = nowCoins.ToString();
            SlowCount.text = slowCount.ToString();  
            PlayerPrefs.SetInt("Coins", nowCoins);
            PlayerPrefs.SetInt("Slow Count", slowCount);

            if (PlayerPrefs.GetString("Music") != "No")
            {
                _buyButtonAudioSource.clip = Success;
                _buyButtonAudioSource.Play();
            }
        }
    }
}
