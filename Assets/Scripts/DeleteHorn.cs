using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteHorn : MonoBehaviour
{
    [SerializeField] private float _timeToDelete;

    private void Start()
    {
        Destroy(gameObject, _timeToDelete);
    }
}
