using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneFading : MonoBehaviour
{
    private float _fadeSpeed = 0.8f;
    private int _drawDepth = -1000;
    private float _alpha = 1f;
    private float _fadeDirection = -1;
    
    public Texture2D Fading;

    private void OnGUI()
    {
        _alpha += _fadeDirection * _fadeSpeed * Time.deltaTime;
        _alpha = Mathf.Clamp01(_alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, _alpha);
        GUI.depth = _drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Fading);
    }

    public float Fade(float direction)
    {
        _fadeDirection = direction;
        return _fadeSpeed;
    }
}
