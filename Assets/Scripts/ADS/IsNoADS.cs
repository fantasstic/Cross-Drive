using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsNoADS : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetString("NoAds") == "Yes")
            Destroy(gameObject);
    }
}
