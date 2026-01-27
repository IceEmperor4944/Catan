using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacePoint : MonoBehaviour
{
    [SerializeField]
    public PlaceableType typeAvailable;
    public Placeable? PlacedObject { get; private set; }
    private List<PlayerColor> settlementsInRadius = new();
    private List<PlayerColor> connectingRoads = new();

    public bool CanPlaceAt(PlayerColor color, int requiredType = 0)
    {
        if ((requiredType == 1 && typeAvailable != PlaceableType.Settlement) 
            || (requiredType == 2 && (typeAvailable == PlaceableType.Settlement || typeAvailable == PlaceableType.City))) 
            return false;
        if (PlacedObject)
        {
            if (typeAvailable != PlaceableType.City || PlacedObject.GetComponent<City>()) return false;
            return color == PlacedObject.color;
        }
        if (typeAvailable == PlaceableType.Settlement && settlementsInRadius.Count > 0) return false;
        if (typeAvailable == PlaceableType.RoadNS || typeAvailable == PlaceableType.RoadNE || typeAvailable == PlaceableType.RoadNW)
        {
            return connectingRoads.Contains(color) || settlementsInRadius.Contains(color);
        }
        return true;
    }

    public void PlaceObject(Placeable newObject)
    {
        if (typeAvailable == PlaceableType.Settlement) typeAvailable = PlaceableType.City;
        else if (typeAvailable == PlaceableType.City) Destroy(PlacedObject.gameObject);
        PlacedObject = newObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Settlement>())
        {
            settlementsInRadius.Add(other.GetComponentInParent<Placeable>().color);
        }
        else if (other.GetComponentInParent<Road>())
        {
            connectingRoads.Add(other.GetComponentInParent<Placeable>().color);
        }
    }
}
