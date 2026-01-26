using UnityEngine;

public abstract class Building : Placeable
{
    public abstract void CollectResources(ResourceType type);
}
