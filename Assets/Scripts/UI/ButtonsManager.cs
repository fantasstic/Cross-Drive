using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] private Sprite _defaultButton, _buttonPressed, _musicOn, _musicOff;

    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();

        if(gameObject.name == "MusicButton")
        {
            if (PlayerPrefs.GetString("Music") == "No")
            {
                transform.GetChild(0).GetComponent<Image>().sprite = _musicOff;
            }
        }
    }

    public void PressPlayButton()
    {
        if(PlayerPrefs.GetString("First Game") == "No")
            StartCoroutine(LoadScene("Game"));
        else
        {
            PlayerPrefs.SetString("First Game", "No");
            PlayerPrefs.SetInt("Slow Count", 3);
            StartCoroutine(LoadScene("Study"));
        }

        PlayButtonSound();
    }

    public void PressRestartButton()
    {
        StartCoroutine(LoadScene("Game"));
        PlayButtonSound();
    }

    public void PressShoptButton()
    {
        StartCoroutine(LoadScene("Shop"));
        PlayButtonSound();
    }

    public void PressClosetButton()
    {
        StartCoroutine(LoadScene("Main"));
        PlayButtonSound();
    }

    public void PressMusicButton()
    {
        if(PlayerPrefs.GetString("Music") == "No")
        {
            PlayerPrefs.SetString("Music", "Yes");
            transform.GetChild(0).GetComponent<Image>().sprite = _musicOn;
        }
        else
        {
            PlayerPrefs.SetString("Music", "No");
            transform.GetChild(0).GetComponent<Image>().sprite = _musicOff;
        }

        PlayButtonSound();
    }

    public void ChangeButtonSprite()
    {
        _image.sprite = _buttonPressed;
        transform.GetChild(0).localPosition -= new Vector3(0, 5, 0);
    }

    public void SetDefaultButtonSprite()
    {
        _image.sprite = _defaultButton; transform.GetChild(0).localPosition += new Vector3(0, 5, 0);
    }

    IEnumerator LoadScene(string nameScene)
    {
        float fadeTime = Camera.main.GetComponent<LoadSceneFading>().Fade(1f);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(nameScene);
    }

    private void PlayButtonSound()
    {
        if (PlayerPrefs.GetString("Music") != "No")
            GetComponent<AudioSource>().Play();
    }
}
