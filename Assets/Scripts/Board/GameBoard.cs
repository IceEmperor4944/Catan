using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    private List<HexTile> board = new();
    private List<PlacePoint> placePoints = new();
    private List<ResourceType> tileSet = new();

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void RandomizeBoard()
    {
        ResetTileSet();
    }

    private void ResetTileSet()
    {
        tileSet.Clear();
        for(int i = 0; i < 3; i++)
        {
            tileSet.Add(ResourceType.Brick);
            tileSet.Add(ResourceType.Ore);
        }
        for (int i = 0; i < 4; i++)
        {
            tileSet.Add(ResourceType.Lumber);
            tileSet.Add(ResourceType.Wool);
            tileSet.Add(ResourceType.Grain);
        }
        tileSet.Add(ResourceType.None);
    }

    private ResourceType GetRandomTileType()
    {
        int tileTypeIndex = Random.Range(0, tileSet.Count -1);
        ResourceType type = tileSet[tileTypeIndex];
        tileSet.RemoveAt(tileTypeIndex);
        return type;
    }
}
