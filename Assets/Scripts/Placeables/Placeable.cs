using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlaceableType
{
    Settlement,
    City,
    RoadNS,
    RoadNE,
    RoadNW
}

public abstract class Placeable : MonoBehaviour
{
    [SerializeField]
    public PlayerColor color;

    protected Player owner;

    public Dictionary<ResourceType, int> cost = new Dictionary<ResourceType, int>();
}


