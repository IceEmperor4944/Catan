using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    public GameBoard board;

    private List<Player> players = new();
    private int currentPlayerIndex = 0;
    private bool checkForPlacement;
    private int setupStep = 0;

    void Start()
    {
        StartGame(2);
    }

    void Update()
    {
        if (checkForPlacement && Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
            {
                PlacePoint point;
                if (hit.collider.TryGetComponent<PlacePoint>(out point)) 
                {
                    int requiredType = (setupStep < 16 ? setupStep % 2 == 0 ? 1 : 2 : 0);
                    if (point.CanPlaceAt(players[currentPlayerIndex].Color, requiredType))
                    {
                        if (board.PlaceObject(point, players[currentPlayerIndex]) != null)
                        {

                            if (setupStep < 16) SetUp();
                        }
                    }
                }
            }
        }
    }

    void StartGame(int playerCount)
    {
        setupStep = 0;
        board.RandomizeBoard();
        players.Clear();

        players.Add(new Player(PlayerColor.Red));
        players.Add(new Player(PlayerColor.Blue));
        players.Add(new Player(PlayerColor.White));
        players.Add(new Player(PlayerColor.Yellow));

        checkForPlacement = true;
    }

    void SetUp()
    {
        switch (setupStep)
        {
            case 1: case 11:
                currentPlayerIndex = 1;
                break;
            case 3: case 9:
                currentPlayerIndex = 2;
                break;
            case 5:
                currentPlayerIndex = 3;
                break;
            case 13:
                currentPlayerIndex = 0;
                break;
        }
        setupStep++;
    }

    bool NextTurn() // returns whether the game was won
    {
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Count) currentPlayerIndex = 0;
        return players[currentPlayerIndex].VictoryPoints == 10;
    }
}
