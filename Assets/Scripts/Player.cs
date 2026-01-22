using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Dictionary<ResourceType, int> inventory = new();
    public int VictoryPoints { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
