using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    private List<HexTile> board = new();
    private List<PlacePoint> placePoints = new();
    private List<ResourceType> tileSet = new();
    private List<int> numbers = new();
    private Transform[] tileHolders;

    [Header("Objects")]
    [SerializeField]
    public GameObject tileGrid;

    [Header("Tile Types")]
    #region tiles
    [SerializeField]
    public HexTile brickTile;
    [SerializeField]
    public HexTile grainTile;
    [SerializeField]
    public HexTile lumberTile;
    [SerializeField]
    public HexTile desertTile;
    [SerializeField]
    public HexTile oreTile;
    [SerializeField]
    public HexTile woolTile;
    #endregion

    [Header("Number Tiles")]
    [SerializeField]
    public GameObject numberTile2;
    public GameObject numberTile3;
    public GameObject numberTile4;
    public GameObject numberTile5;
    public GameObject numberTile6;
    public GameObject numberTile8;
    public GameObject numberTile9;
    public GameObject numberTile10;
    public GameObject numberTile11;
    public GameObject numberTile12;

    void Start()
    {
        tileHolders = tileGrid.GetComponentsInChildren<Transform>();
        RandomizeBoard();
    }

    void Update()
    {
        
    }

    public void RandomizeBoard()
    {
        ResetTileSet();
        ResetNumbers();
        foreach (Transform tilePlace in tileHolders)
        {
            int resourceTypeIndex = Random.Range(0, tileSet.Count);
            ResourceType type = tileSet[resourceTypeIndex];

            HexTile tile = null;

            switch (type)
            {
                case ResourceType.Brick:
                    tile = Instantiate(brickTile, tilePlace);
                    break;
                case ResourceType.Grain:
                    tile = Instantiate(grainTile, tilePlace);
                    break;
                case ResourceType.Lumber:
                    tile = Instantiate(lumberTile, tilePlace);
                    break;
                case ResourceType.None:
                    tile = Instantiate(desertTile, tilePlace);
                    break;
                case ResourceType.Ore:
                    tile = Instantiate(oreTile, tilePlace);
                    break;
                case ResourceType.Wool:
                    tile = Instantiate(woolTile, tilePlace);
                    break;
            }

            if (type != ResourceType.None)
            {
                int numberIndex = Random.Range(0, numbers.Count);
                int number = numbers[numberIndex];

                switch (number)
                {
                    case 2:
                        Instantiate(numberTile2, tilePlace);
                        break;
                    case 3:
                        Instantiate(numberTile3, tilePlace);
                        break;
                    case 4:
                        Instantiate(numberTile4, tilePlace);
                        break;
                    case 5:
                        Instantiate(numberTile5, tilePlace);
                        break;
                    case 6:
                        Instantiate(numberTile6, tilePlace);
                        break;
                    case 8:
                        Instantiate(numberTile8, tilePlace);
                        break;
                    case 9:
                        Instantiate(numberTile9, tilePlace);
                        break;
                    case 10:
                        Instantiate(numberTile10, tilePlace);
                        break;
                    case 11:
                        Instantiate(numberTile11, tilePlace);
                        break;
                    case 12:
                        Instantiate(numberTile12, tilePlace);
                        break;
                }
                tile.Number = number;
                numbers.RemoveAt(numberIndex);
            }

            tileSet.RemoveAt(resourceTypeIndex);

            board.Add(tile);
        }
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

    private void ResetNumbers()
    {
        numbers.Clear();
        numbers.Add(2);
        for(int i = 3; i < 12; i++)
        {
            if (i != 7)
            {
                numbers.Add(i);
                numbers.Add(i);
            }
        }
        numbers.Add(12);
    }

    private ResourceType GetRandomTileType()
    {
        int tileTypeIndex = Random.Range(0, tileSet.Count -1);
        ResourceType type = tileSet[tileTypeIndex];
        tileSet.RemoveAt(tileTypeIndex);
        return type;
    }
}
