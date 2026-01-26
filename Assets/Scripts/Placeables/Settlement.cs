using UnityEngine;

public class Settlement : Building
{
    public override void CollectResources(ResourceType type)
    {
        owner.GainResource(type, 1);
    }
}
