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
    public Placeable(Player owner)
    {
        this.owner = owner;
    }

    protected Player owner;
}
