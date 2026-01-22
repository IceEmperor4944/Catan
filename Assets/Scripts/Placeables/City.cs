using System.Buffers;
using UnityEngine;

public class City : Building
{
    public City(Player owner) : base(owner)
    {
    }

    public override void CollectResources(ResourceType type)
    {
        owner.GainResource(type, 2);
    }
}
