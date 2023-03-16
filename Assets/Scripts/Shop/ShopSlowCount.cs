using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopSlowCount : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TMP_Text>().text = PlayerPrefs.GetInt("Slow Count").ToString();
    }
}
