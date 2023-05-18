using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerController : NetworkBehaviour, IPlayerJoined,IPlayerLeft
{
    [SerializeField] private NetworkPrefabRef playerNetworkPrefab = NetworkPrefabRef.Empty;
    [SerializeField] private Transform[] spawnPoints;

    public override void Spawned() // Funcion para instanciar el personaje del host
    {
        if (Runner.IsServer)
        {
            foreach (var item in Runner.ActivePlayers) 
            { 
                SpawnPlayer(item);
            }
        }
    }

    private void SpawnPlayer(PlayerRef playerRef) //Funcion para generar los personajes de los clientes
    {
        if (Runner.IsServer) // el host es el encargado de instanciar al jugador
        {
            var index = (int)playerRef % spawnPoints.Length; // validacion en dado caso que la cantidad de jugadores sea mayor al numero de puntos de spawn
            var spawnPoint = spawnPoints[index].transform.position;
            var playerObject = Runner.Spawn(playerNetworkPrefab, Vector3.zero, Quaternion.identity, playerRef);

            Runner.SetPlayerObject(playerRef, playerObject); // funcion para setear variables para saber que este es mi jugador local
        }
    }

    private void DespawnPlayer(PlayerRef playerRef)
    {
        if (Runner.IsServer)
        {
            if (Runner.TryGetPlayerObject(playerRef, out var playerNetworkObject))
            {
                Runner.Despawn(playerNetworkObject);
            }

            //Reset Player Object
            Runner.SetPlayerObject(playerRef, null); 

        }
    }

    //Cuando ingrese a la sala, llamara la funcion para instanciar al player
    public void PlayerJoined(PlayerRef player)
    {
        SpawnPlayer(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        DespawnPlayer(player);
    }
}
