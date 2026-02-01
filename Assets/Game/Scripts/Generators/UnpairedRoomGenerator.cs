using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Room))]
public class UnpairedRoomGenerator : MonoBehaviour
{
    [SerializeField] private Actor _actorTemplate;
    [SerializeField] private Transform _actorSpawnsContainer;

    private void Awake()
    {
        var room = GetComponent<Room>();
        room.OnGenerate.AddListener(Generate);
    }

    private void Generate()
    {
        Debug.Log("UnpairedRoomGenerator Generate called");
    }

    private void OnDrawGizmos()
    {
        var actorSpawns = _actorSpawnsContainer.GetComponentsInChildren<Transform>();
        foreach (var spawn in actorSpawns)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(spawn.position, 0.1f);
            Gizmos.DrawLine(spawn.position, spawn.position + spawn.forward * 0.5f);
        }
    }
}
