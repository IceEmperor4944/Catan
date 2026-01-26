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
    [SerializeField]
    public ResourceType Type;

    public int Number { get; set; }

    private List<Building> buildings = new();


    public void CollectResources()
    {
        foreach (Building building in buildings)
        {
            building.CollectResources(Type);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Building>(out Building newBuilding))
        {
            buildings.Add(newBuilding);
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.collider.TryGetComponent<Building>(out Building newBuilding))
        {
            buildings.Remove(newBuilding);
        }
    }
}
