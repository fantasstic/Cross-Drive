using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private Vector2 _randomTime;
    [SerializeField] private GameObject[] _carPrefabs;
    [SerializeField] private Transform _bottomCarsSpawn;
    [SerializeField] private Transform _upperCarsSpawn;
    [SerializeField] private Transform _leftCarsSpawn;
    [SerializeField] private Transform _rightCarsSpawn;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private TMP_Text _score, _bestScore, _coins;
    [SerializeField] private GameObject _horn;
    [SerializeField] private AudioSource _turnSignal;
    [SerializeField] private GameObject _adsManager;

    private int _countCars;
    private Coroutine _bottomCars, _leftCars, _rightCars, _upperCars;
    private bool _isLoseOnce;

    private static bool _isAddOnScene;

    public bool IsMainScene = false;
    public GameObject[] Maps;

    public static int LoseCount;

    private void Start()
    {
        if(!_isAddOnScene && PlayerPrefs.GetString("NoADs") != "Yes")
        {
            _isAddOnScene = true;
            Instantiate(_adsManager, Vector3.zero, Quaternion.identity);
        }

        if(PlayerPrefs.GetInt("NowMap") == 2)
        {
            Destroy(Maps[0]);
            Destroy(Maps[2]);
            Maps[1].SetActive(true);
        }
        else if(PlayerPrefs.GetInt("NowMap") == 3)
        {
            Destroy(Maps[0]);
            Destroy(Maps[1]);
            Maps[2].SetActive(true);
        }
        else
        {
            Destroy(Maps[1]);
            Destroy(Maps[2]);
            Maps[0].SetActive(true);
        }

        CarController.IsLose = false;
        CarController.CountCars = 0;

        if(IsMainScene)
        {
            _randomTime.x = 4f;
            _randomTime.y = 6f;
        }

        _bottomCars = StartCoroutine(BottomCars());
        _leftCars = StartCoroutine(LeftCars());
        _rightCars = StartCoroutine(RightCars());
        _upperCars = StartCoroutine(UpperCars());

        StartCoroutine(CreateHorn());
    }

    private void Update()
    {
        if(CarController.IsLose && !_isLoseOnce)
        {
            LoseCount++;

            StopCoroutine(_bottomCars);
            StopCoroutine(_leftCars);
            StopCoroutine(_rightCars);
            StopCoroutine(_upperCars);
            _score.text = "<color=#FF4A4A>Score:</color> " + CarController.CountCars;

            if(PlayerPrefs.GetInt("Score") < CarController.CountCars)
            {
                PlayerPrefs.SetInt("Score", CarController.CountCars);
            }

            _bestScore.text = "<color=#FF4A4A>Best:</color> " + PlayerPrefs.GetInt("Score");

            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + CarController.CountCars);
            _coins.text = PlayerPrefs.GetInt("Coins").ToString();

            _isLoseOnce = true;
            _losePanel.SetActive(true);
        }
    }

    IEnumerator BottomCars()
    {
        while(true)
        {
            float timeToSpawn = Random.Range(_randomTime.x, _randomTime.y);
            SpawnCar(_bottomCarsSpawn.position, 180f);

            yield return new WaitForSeconds(timeToSpawn);
        }
    }

    IEnumerator LeftCars()
    {
        while(true)
        {
            float timeToSpawn = Random.Range(_randomTime.x, _randomTime.y);
            SpawnCar(_leftCarsSpawn.position, 270f);

            yield return new WaitForSeconds(timeToSpawn);
        }
    }

    IEnumerator RightCars()
    {
        while(true)
        {
            float timeToSpawn = Random.Range(_randomTime.x, _randomTime.y);
            SpawnCar(_rightCarsSpawn.position, 90f);

            yield return new WaitForSeconds(timeToSpawn);
        }
    }

    IEnumerator UpperCars()
    {
        while(true)
        {
            float timeToSpawn = Random.Range(_randomTime.x, _randomTime.y);
            SpawnCar(_upperCarsSpawn.position, 0f);

            yield return new WaitForSeconds(timeToSpawn);
        }
    }

    private void SpawnCar(Vector3 spawnPoint, float carRotationY, bool ifMoveFromUp = false)
    {
        GameObject car = Instantiate(_carPrefabs[Random.Range(0, _carPrefabs.Length)], spawnPoint, Quaternion.Euler(0, carRotationY, 0));
        car.name = "Car - " + ++_countCars;

        int random = IsMainScene ? 2 : Random.Range(1, 4);
        
        switch (random)
        {
            case 1:
                // Move left
                car.GetComponent<CarController>().LeftTurn = true;

                if (PlayerPrefs.GetString("Music") != "No" && !_turnSignal.isPlaying)
                {
                    _turnSignal.Play();
                    Invoke("StopSound", 4f);
                }

                if (ifMoveFromUp)
                    car.GetComponent<CarController>().MoveFromUp = true;
                break;
            case 2:
                // Move right
                car.GetComponent<CarController>().RightTurn = true;

                if (PlayerPrefs.GetString("Music") != "No" && !_turnSignal.isPlaying)
                {
                    _turnSignal.Play();
                    Invoke("StopSound", 4f);
                }
                break;
            case 3:
                // Move forward
                break;

        }
    }

    private void StopSound()
    {
        _turnSignal.Stop();
    }

    IEnumerator CreateHorn()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(5, 9));
            if (PlayerPrefs.GetString("Music") != "No")
                Instantiate(_horn, Vector3.zero, Quaternion.identity);
        }
    }
}
