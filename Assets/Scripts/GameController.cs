using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameBoard board = new();
    private List<Player> players = new();
    private int currentPlayerIndex = 0;

    void Start()
    {   
    }

    void Update()
    {
        
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
