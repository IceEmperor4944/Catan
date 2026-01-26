using UnityEngine;

public class Settlement : Building
{

    private void Awake()
    {
        cost.Add(ResourceType.Brick, 1);
        cost.Add(ResourceType.Lumber, 1);
        cost.Add(ResourceType.Wool, 1);
        cost.Add(ResourceType.Grain, 1);
    }
    public override void CollectResources(ResourceType type)
    {
        owner.GainResource(type, 1);
    }
}
