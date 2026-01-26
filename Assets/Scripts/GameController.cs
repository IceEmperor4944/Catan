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

    void Start()
    {
        StartGame(2);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                PlacePoint point;
                if (hit.collider.TryGetComponent<PlacePoint>(out point)) 
                {
                    board.PlaceObject(hit.transform, point.typeAvailable);
                }
            }
        }
    }

    void StartGame(int playerCount)
    {
        board.RandomizeBoard();
        players.Clear();

        for (int i = 0; i < playerCount; i++)
        {
            players.Add(new Player());
        }
    }

    bool NextTurn() // returns whether the game was won
    {
        return players[currentPlayerIndex].VictoryPoints == 10;
    }
}
