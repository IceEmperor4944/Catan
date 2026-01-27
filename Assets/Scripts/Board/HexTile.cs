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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Building>())
        {
            buildings.Add(other.GetComponentInParent<Building>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Building>())
        {
            buildings.Remove(other.GetComponentInParent<Building>());
        }
    }
}
