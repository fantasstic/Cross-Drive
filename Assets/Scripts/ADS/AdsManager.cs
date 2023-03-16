using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
    private InterstitialAd _interstitialAd;
    private int _nowLoses;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        DestroyAndStartNew(true);
    }

    private void Update()
    {
        if(_interstitialAd.CanShowAd() && GameController.LoseCount % 3 == 0 && GameController.LoseCount != 0 && GameController.LoseCount != _nowLoses)
        {
            _nowLoses = GameController.LoseCount;
            _interstitialAd.Show();
        }
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        DestroyAndStartNew();
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        DestroyAndStartNew();

    }

    private void DestroyAndStartNew(bool isFirst = false)
    {
        if(!isFirst)
            _interstitialAd.Destroy();

        _interstitialAd = new InterstitialAd(_adUnitId);
        _interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        _interstitialAd.OnAdClosed += HandleOnAdClosed;

        AdRequest request = new AdRequest.Builder().Build();

        _interstitialAd.LoadAd(request);
    }
}
