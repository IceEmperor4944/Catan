using UnityEngine;

public class Settlement : Building
{
    public Settlement(Player owner) : base(owner)
    {
    }

    public override void CollectResources(ResourceType type)
    {
        owner.GainResource(type, 1);
    }
}
