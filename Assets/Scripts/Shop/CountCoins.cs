using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountCoins : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TMP_Text>().text = PlayerPrefs.GetInt("Coins").ToString();
    }
}
