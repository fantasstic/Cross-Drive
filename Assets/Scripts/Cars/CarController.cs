using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [SerializeField] private float _rotateSpeedRight;
    [SerializeField] private float _rotateSpeedLeft;
    [SerializeField] private LayerMask _carsLayer;
    [SerializeField] private GameObject _turnLeftSignal;
    [SerializeField] private GameObject _turnRightSignal;
    [SerializeField] private float _hitForce;


    private Rigidbody _rigidbody;
    private float _startRotationY;
    private Camera _mainCamera;
    private bool _isMovingFast, _carCrashed;

    [NonSerialized] public bool CarPassed;
    [NonSerialized] public static bool IsLose;
    [NonSerialized] public static int CountCars;

    public float Speed;
    public GameObject CrashEffect;
    public GameObject SpeedUpEffect;
    public bool RightTurn;
    public bool LeftTurn;
    public bool MoveFromUp;
    public AudioClip CrashSound;
    public AudioClip[] SpeedUpSound;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startRotationY = transform.eulerAngles.y;
        _mainCamera = Camera.main;

        if (RightTurn)
        {
            StartCoroutine(TurnSignals(_turnRightSignal));
        }
        else if (LeftTurn)
            StartCoroutine(TurnSignals(_turnLeftSignal));
    }

    private void FixedUpdate()
    {
        CarMover();
    }

    private void Update()
    {
        if (Input.touchCount == 0)
            return;

        Ray ray = _mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100f, _carsLayer))
        {
            string carName = hit.transform.gameObject.name;
            if(Input.GetTouch(0).phase == TouchPhase.Began && !_isMovingFast && gameObject.name == carName)
            {
                GameObject speedUpEffect = Instantiate(SpeedUpEffect, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.Euler(90, 0, 0));
                Destroy(speedUpEffect, 2f);
                Speed *= 2f;
                _isMovingFast = true;
                if (PlayerPrefs.GetString("Music") != "No")
                {
                    GetComponent<AudioSource>().clip = SpeedUpSound[UnityEngine.Random.Range(0, SpeedUpSound.Length)];
                    GetComponent<AudioSource>().Play();
                }
            }
        }

    }

    private void CarMover()
    {
        if (IsLose)
            return;

        _rigidbody.MovePosition(transform.position - transform.forward * Speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Car") && !_carCrashed)
        {
            _carCrashed = true;
            IsLose = true;
            Speed = 0f;
            collision.gameObject.GetComponent<CarController>().Speed = 0f;

            GameObject crashEffect = Instantiate(CrashEffect, transform.position, Quaternion.identity);
            Destroy(crashEffect, 5f);

            if (_isMovingFast)
                _hitForce *= 1.2f; 

            _rigidbody.AddRelativeForce(Vector3.back * _hitForce);
            if (PlayerPrefs.GetString("Music") != "No")
            {
                GetComponent<AudioSource>().clip = CrashSound;
                GetComponent<AudioSource>().Play();
            }
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if(collider.transform.CompareTag("TurnRight") && RightTurn)
        {
            RotateCar(_rotateSpeedRight);
        }
        else if(collider.transform.CompareTag("TurnLeft") && LeftTurn)
        {
            RotateCar(_rotateSpeedLeft, -1);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Car") && collider.GetComponent<CarController>().CarPassed)
            collider.GetComponent<CarController>().Speed = Speed;
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.transform.CompareTag("Active collider"))
        {
            if (CarPassed)
                return;

            CarPassed = true;
            Collider[] colliders = GetComponents<BoxCollider>();
            foreach (Collider carCollider in colliders)
                carCollider.enabled = true;

            CountCars++;
        }

        if(collider.transform.CompareTag("TurnRight") && RightTurn)
        {
            _rigidbody.rotation = Quaternion.Euler(0, _startRotationY + 90f, 0);
        }
        else if((collider.transform.CompareTag("TurnLeft") && LeftTurn))
        {
            _rigidbody.rotation = Quaternion.Euler(0, _startRotationY - 90f, 0);
        }
        else if(collider.transform.CompareTag("Delete Trigger"))
        {
            Destroy(gameObject);
        }
    }

    private void RotateCar(float rotateCarSpeed, int direction = 1)
    {
        if (IsLose)
            return;

        if (direction == -1 && transform.localRotation.eulerAngles.y < _startRotationY - 90f)
            return;
        if (direction == -1 && MoveFromUp && transform.localRotation.eulerAngles.y > 250f && direction == -1 && transform.localRotation.eulerAngles.y < 270f)
            return;

            float rotateSpeed = Speed * rotateCarSpeed * direction;
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, rotateSpeed, 0) * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }

    IEnumerator TurnSignals(GameObject turnSignal)
    {
        while(!CarPassed)
        {
            turnSignal.SetActive(!turnSignal.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
