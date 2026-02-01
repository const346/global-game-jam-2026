using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Room))]
public class UnpairedRoomGenerator : MonoBehaviour
{
    [SerializeField] private Actor _actorTemplate;
    [SerializeField] private Transform _actorSpawnsContainer;
    [SerializeField] private int fk = 3;

    private Room _room;

    private void Awake()
    {
        _room = GetComponent<Room>();
        _room.OnGenerate.AddListener(Generate);
    }

    private void SetCategoryValue(ref ActorMask mask, string category, string value)
    {
        if (category == "Horn")
        {
            mask.Horn = value;
        }
        if (category == "Color")
        {
            mask.Color = value;
        }
        if (category == "Type")
        {
            mask.Type = value;
        }
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

        var categories = table.Keys.ToArray().OrderBy(_ => UnityEngine.Random.value).ToArray();
        var category1 = categories[0]; // 1 из 3
        var category2 = categories[1]; // 1 из 2
        var category3 = categories[2]; // 1 из 2

        table["Horn"] = table["Horn"].OrderBy(_ => UnityEngine.Random.value).ToArray();
        table["Color"] = table["Color"].OrderBy(_ => UnityEngine.Random.value).ToArray();
        table["Type"] = table["Type"].OrderBy(_ => UnityEngine.Random.value).ToArray();

        var masks = new List<ActorMask>();
        for (int i = 0; i < fk; i++)
        {
            var mask1 = new ActorMask
            {
                HasGold = false,
                HasPattern = false,
            };
            SetCategoryValue(ref mask1, category1, table[category1][i]);
            SetCategoryValue(ref mask1, category2, table[category2][0]);
            SetCategoryValue(ref mask1, category3, table[category3][0]);

            var mask2 = new ActorMask
            {
                HasGold = false,
                HasPattern = false,
            };

            SetCategoryValue(ref mask2, category1, table[category1][i]);
            SetCategoryValue(ref mask2, category2, table[category2][1]);
            SetCategoryValue(ref mask2, category3, table[category3][1]);

            masks.Add(mask1);
            masks.Add(mask2);
        }

        var answer = new ActorMask
        {
            HasGold = false,
            HasPattern = false,
        };

        SetCategoryValue(ref answer, category1, table[category1][UnityEngine.Random.Range(0, table[category1].Length)]);
        SetCategoryValue(ref answer, category2, table[category2][0]);
        SetCategoryValue(ref answer, category3, table[category3][1]);

        masks.Add(answer);

        var actorSpawns = _actorSpawnsContainer.GetComponentsInChildren<Transform>();
        actorSpawns = actorSpawns.Where(x => x != _actorSpawnsContainer)
            .OrderBy(_ => Random.value).ToArray();

        for (int i = 0; i < masks.Count; i++)
        {
            var actor = Instantiate(_actorTemplate, actorSpawns[i].position, actorSpawns[i].rotation, transform);
            actor.ApplyCustomize(masks[i]);

            if (i == masks.Count - 1)
            {
                _room.CorrectActor = actor;
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
        var actorSpawns = _actorSpawnsContainer.GetComponentsInChildren<Transform>();
        foreach (var spawn in actorSpawns)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(spawn.position, 0.1f);
            Gizmos.DrawLine(spawn.position, spawn.position + spawn.forward * 0.5f);
        }
    }
}
