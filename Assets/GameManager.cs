using System;
using UnityEngine;

public class GameManager : OdinserializedSingletonBehaviour<GameManager>
{
    public Transform playerSpawnPosition;

    public Player Player { get; private set; }

    private void Start()
    {
        TrySpawnPlayer();
    }

    private void TrySpawnPlayer()
    {
        Player = FindObjectOfType<Player>();

        if (!Player)
            Player = Instantiate(Player, playerSpawnPosition);
    }
}
