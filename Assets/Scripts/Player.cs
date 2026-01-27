using System.Collections.Generic;
using UnityEngine;

public enum PlayerColor
{
    Red,
    White,
    Blue,
    Yellow
}

public class Player
{
    public PlayerColor Color { get; set; }
    private Dictionary<ResourceType, int> inventory = new() { { ResourceType.Brick, 0 }, { ResourceType.Lumber, 0 }, { ResourceType.Wool, 0 }, { ResourceType.Grain, 0 }, { ResourceType.Ore, 0 } };
    public int VictoryPoints { get; set; }
    public PlayerPanelController PanelController { get; set; }

    public Player(PlayerColor color)
    {
        Color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GainResource(ResourceType type, int count)
    {
        if (inventory.ContainsKey(type)) inventory[type] += count;
        else inventory.Add(type, count);
    }

    public int GetResourceCount(ResourceType type)
    {
        if (inventory.ContainsKey(type)) return inventory[type];
        return 0;
    }

    /// <summary>
    /// Checks if the player has enough resources to place the given placeable. <br/>
    /// If so, deducts the resources from the player's inventory and returns true.
    /// </summary>
    /// <param name="placeable">The placeable you are trying to place</param>
    /// <returns>True if can place, otherwise false</returns>
    public bool DoesPlayerHaveResourcesForPlaceable(Placeable placeable)
    {
        var cost = placeable.cost;

        foreach (var resource in cost)
        {
            if (!inventory.ContainsKey(resource.Key) || inventory[resource.Key] < resource.Value)
            {
                return false;
            }
            
        }

        // If we reach here, the player has enough resources for all types

        foreach (var resource in cost)
        {
            inventory[resource.Key] -= resource.Value;
        }

        PanelController.UpdateResourcesText();

        return true;
       
    }
}
