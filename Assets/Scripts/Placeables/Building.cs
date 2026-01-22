using UnityEngine;

public abstract class Building : Placeable
{
    protected Building(Player owner) : base(owner) { }

    public abstract void CollectResources(ResourceType type);
}
