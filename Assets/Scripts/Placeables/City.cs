using System.Buffers;
using UnityEngine;

public class City : Building
{
    private void Awake()
    {
        cost.Add(ResourceType.Ore, 3);
        cost.Add(ResourceType.Grain, 2);

        VPValue = 1;
    }

    public override void CollectResources(ResourceType type)
    {
        Owner.GainResource(type, 2);
    }
}
