using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private Transform _playerSpawn;

    public UnityEvent OnGenerate;

    public void Complete()
    {
        foreach (var actor in GetComponentsInChildren<Actor>())
        {
            actor.DeactivateInteraction();
        }

        _door.Open();
    }

    public void SpawnPlayer()
    {
        var player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.transform.position = _playerSpawn.position;
            player.transform.rotation = _playerSpawn.rotation;
        }
    }

    private void Start()
    {
        OnGenerate?.Invoke();
    }
}