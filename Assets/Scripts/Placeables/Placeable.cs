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

    public int VPValue { get; protected set; } = 0;

    public Player Owner { get; set; }

    public Dictionary<ResourceType, int> cost = new Dictionary<ResourceType, int>();
}


