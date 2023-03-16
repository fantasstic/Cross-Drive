using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlowMotionGame : MonoBehaviour
{
    [SerializeField] private float _timeScaleChangeSpeed = 1;
    [SerializeField] private float _slowMotionDuration = 5;
    [SerializeField] private int _slowMotionCount;
    [SerializeField] private TMP_Text _slowMotionCountText;
    [SerializeField] private Button _slowMotionButton;
    [SerializeField] private float _slowMotionMax = 0.1f;

    private bool _isSlowMotion;
    private float _currentSlowMotionDuration;

    private void Update()
    {
        int slowCount = PlayerPrefs.GetInt("Slow Count");
        _slowMotionCountText.text = slowCount.ToString();

        if (_isSlowMotion)
        {
            DoSlowMotion();
        }

        if (slowCount <= 0)
            _slowMotionButton.interactable = false;
        else
            _slowMotionButton.interactable = true;
    }
    
    public void SlowMotionButton()
    {
        int slowCount = PlayerPrefs.GetInt("Slow Count");
        if (slowCount > 0)
        {
            StartSlowMotion();
            slowCount--;
            PlayerPrefs.SetInt("Slow Count", slowCount);
        }
    }

    public void StartSlowMotion()
    {
        _isSlowMotion = true;

        _currentSlowMotionDuration = _slowMotionDuration;
    }

    public void StopSlowMotion()
    {
        _isSlowMotion = false;

        Time.timeScale = 1;
    }

    private void DoSlowMotion()
    {
        Time.timeScale = Mathf.Clamp(Time.timeScale - _timeScaleChangeSpeed * Time.unscaledDeltaTime, _slowMotionMax, 1);

        _currentSlowMotionDuration -= Time.unscaledDeltaTime;

        if (_currentSlowMotionDuration < 0)
        {
            StopSlowMotion();
        }
    }

}
