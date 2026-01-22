using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Brick,
    Lumber,
    Ore,
    Grain,
    Wool,
    None
}

public class HexTile : MonoBehaviour
{
    public HexTile(ResourceType type, int number)
    {
        Type = type;
        Number = number;
    }

    public ResourceType Type { get; private set; }
    public int Number { get; private set; }

    private List<Building> buildings = new();


    public void CollectResources()
    {
        foreach (Building building in buildings)
        {
            building.CollectResources(Type);
        }
    }
}
