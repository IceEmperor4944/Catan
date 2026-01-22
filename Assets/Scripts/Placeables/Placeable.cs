using UnityEngine;

public enum PlaceableType
{
    Road,
    Settlement,
    City
}

public abstract class Placeable
{
    public Placeable(Player owner)
    {
        this.owner = owner;
    }

    protected Player owner;
}
