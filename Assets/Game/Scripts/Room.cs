using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private Transform _playerSpawn;
    [SerializeField] private Transform[] _actorSpawns;

    [SerializeField] private Actor _actorTemplate;

    public void Complete()
    {
        _door.Open();
    }

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        var table = new Dictionary<string, string[]>()
        {
            { "Horn", new[] { "Deer", "Goat", "Ram" } },
            { "Color", new[] { "White", "Back", "Red" } },
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

        var spawns = _actorSpawns.OrderBy(_ => Random.value).ToList();

        var xe = new List<string>();

        for (int i = 0; i < masks.Count; i++)
        {
            //var actor = Instantiate(_actorTemplate, _actorSpawns[i].position, _actorSpawns[i].rotation, transform);
            //actor.ApplyCustomize(masks[i]);

            //if (i == masks.Count - 1)
            //{
            //    actor.SetCorrect(true);
            //}

            xe.Add($"Actor {i}: Type={masks[i].Type}, Color={masks[i].Color}, Horn={masks[i].Horn}, IsCorrect={i == masks.Count - 1}");
        }

        foreach(var txt in xe.OrderBy(_ => Random.value).ToList())
        {
                       Debug.Log(txt);

        }
    }
}