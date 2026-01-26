using System.Buffers;
using UnityEngine;

public class City : Building
{
    public override void CollectResources(ResourceType type)
    {
        owner.GainResource(type, 2);
    }
}
