using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Room))]
public class SimpleRoomGenerator : MonoBehaviour
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
        ClearActors();

        var table = new Dictionary<string, string[]>()
        {
            { "Horn", new[] { "Deer", "Goat", "Ram" } },
            { "Color", new[] { "White", "Black", "Red" } },
            { "Type", new[] { "TypeA", "TypeB", "TypeC" } },
        };

        var categories = table.Keys.ToArray();
        var categoryIndex = UnityEngine.Random.Range(0, categories.Length);
        var category = categories[categoryIndex];

        table["Horn"] = table["Horn"].OrderBy(_ => UnityEngine.Random.value).ToArray();
        table["Color"] = table["Color"].OrderBy(_ => UnityEngine.Random.value).ToArray();
        table["Type"] = table["Type"].OrderBy(_ => UnityEngine.Random.value).ToArray();

        var otherValue = table[category][1];
        table[category] = Enumerable.Range(0, 3).Select(x => table[category][0]).ToArray();

        var masks = new List<ActorMask>();
        for (int i = 0; i < 3; i++)
        {
            masks.Add(new ActorMask
            {
                Horn = table["Horn"][i],
                Color = table["Color"][i],
                Type = table["Type"][i],
                HasGold = false,
                HasPattern = false,
            });
        }

        var answer = new Dictionary<string, string>()
        {
            { "Horn", table["Horn"][UnityEngine.Random.Range(0, table["Horn"].Length)] },
            { "Color", table["Color"][UnityEngine.Random.Range(0, table["Color"].Length)] },
            { "Type", table["Type"][UnityEngine.Random.Range(0, table["Type"].Length)] },
        };
        answer[category] = otherValue;

        masks.Add(new ActorMask
        {
            Horn = answer["Horn"],
            Color = answer["Color"],
            Type = answer["Type"],
            HasGold = false,
            HasPattern = false,
        });

        var actorSpawns = _actorSpawnsContainer.GetComponentsInChildren<Transform>();
        actorSpawns = actorSpawns.Where(x => x != _actorSpawnsContainer)
            .OrderBy(_ => Random.value).ToArray();

        for (int i = 0; i < masks.Count; i++)
        {
            var actor = Instantiate(_actorTemplate, actorSpawns[i].position, actorSpawns[i].rotation, transform);
            actor.ApplyCustomize(masks[i]);

            if (i == masks.Count - 1)
            {
                actor.SetCorrect(true);
            }

            Debug.Log($"Actor {i}: Type={masks[i].Type}, Color={masks[i].Color}, Horn={masks[i].Horn}, IsCorrect={i == masks.Count - 1}");
        }
    }

    private void ClearActors()
    {
        var existingActors = GetComponentsInChildren<Actor>();
        foreach (var actor in existingActors)
        {
            Destroy(actor.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        var actorSpawns = _actorSpawnsContainer
            .GetComponentsInChildren<Transform>()
            .Where(x => x != _actorSpawnsContainer);

        foreach (var spawn in actorSpawns)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(spawn.position, 0.1f);
            Gizmos.DrawLine(spawn.position, spawn.position + spawn.forward * 0.5f);
        }
    }
}