using System.Collections.Generic;
using UnityEngine;

public class Road : Placeable
{
    private void Awake()
    {
        cost.Add(ResourceType.Brick, 1);
        cost.Add(ResourceType.Lumber, 1);
    }
}
