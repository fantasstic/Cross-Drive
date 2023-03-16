using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseMap : MonoBehaviour
{
    [SerializeField] private AudioClip _clickSound;

    public void ChooseNemMap(int numberMap)
    {
        if (PlayerPrefs.GetString("Music") != "No")
        {
            GetComponent<AudioSource>().clip = _clickSound;
            GetComponent<AudioSource>().Play();

        }

        PlayerPrefs.SetInt("NowMap", numberMap);
        GetComponent<CheckMaps>().WhichMapSelected();
    }
}
