using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFirstCar : MonoBehaviour
{
    [SerializeField] private GameObject _firstHelp;
    [SerializeField] private GameObject _secondCar;
    [SerializeField] private GameObject _secondHelp;

    private bool _isFirstHelp;
    private CarController _controller;
    private Camera _mainCamera;

    private void Start()
    {
        _controller = GetComponent<CarController>();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if(transform.position.x < 8f && !_isFirstHelp)
        {
            _isFirstHelp = true;
            _controller.Speed = 0;
            _firstHelp.SetActive(true);
        }

        if (Input.touchCount == 0)
            return;

        Ray ray = _mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                

                _controller.Speed = 15f;
                _firstHelp.SetActive(false);
                _secondHelp.SetActive(true);
                _secondCar.GetComponent<CarController>().Speed = 12f;
            }
        }
    }

    

    public void FirstCarClick()
    {
        if (_isFirstHelp)
            return;

        _controller.Speed = 15f;
        _firstHelp.SetActive(false);
        _secondHelp.SetActive(true);
        _secondCar.GetComponent<CarController>().Speed = 12f;
    }
}
