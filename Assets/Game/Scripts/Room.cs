using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private Transform _playerSpawn;

    public void Complete()
    {
        _door.Open();
    }
}