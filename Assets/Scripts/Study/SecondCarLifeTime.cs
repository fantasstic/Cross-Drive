using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecondCarLifeTime : MonoBehaviour
{
    private void OnDestroy()
    {
        SceneManager.LoadScene("Game");
    }
}
