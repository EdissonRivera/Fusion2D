using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerController : MonoBehaviour,INetworkRunnerCallbacks
{
    public event Action OnStartedRunnerConnection; //Inicio una conneccion con fusion
    public event Action OnPlayerJoinedSucessFully; //Jugador se unio correctamente

    [SerializeField] private NetworkRunner networkRunnerPrefab;
    private NetworkRunner networkRunnerInstance;

    //Funcion para Iniciar NetworkRunner y conectarme a x sala
    public async void StartGame(GameMode gameMode, string roomName)
    {
        OnStartedRunnerConnection?.Invoke();

        if (networkRunnerInstance == null)
        {
            networkRunnerInstance = Instantiate(networkRunnerPrefab);
        }
        //Registra callbacks
        networkRunnerInstance.AddCallbacks(this);

        // asegurarnos de que el jugador esta proporcionando la informacion (el jugador recoge la entrada y luego la envia al servidor)
        networkRunnerInstance.ProvideInput = true; 

        var startGameArgs = new StartGameArgs()
        {
            GameMode = gameMode, //Host or client
            SessionName = roomName, //Code Room
            PlayerCount = 4,  // MaxPlayers
            SceneManager = networkRunnerInstance.GetComponent<INetworkSceneManager>() //Default Scene
        };

        //espera a que termine la funcion
        var result = await networkRunnerInstance.StartGame(startGameArgs);
        //Finish 
        if (result.Ok)
        {
            //se unio correctamente
            const String SCENE_NAME = "MainGame";
            networkRunnerInstance.SetActiveScene(SCENE_NAME);
        }
        else
        {
            Debug.LogError($"Failed to start{result.ShutdownReason}");
        }
    }

    public void ShutDownRunner()
    {
        networkRunnerInstance.Shutdown(); //Desconectarnos de la red
    }



    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("ConnectedToServer");
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log("ConnectFailed");

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        Debug.Log("ConnectedRequest");
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        Debug.Log("CustomAuthentication");
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("OnDisconnectedFromServer");
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        Debug.Log("OnHostMigration");
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        Debug.Log("OnInput");
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        Debug.Log("InputMissin");
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("PlayerJoin");
        OnPlayerJoinedSucessFully?.Invoke();
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("PlayerLeft");
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
        Debug.Log("ConnectedToServer");
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("SceneLoadDone");
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        Debug.Log("SceneLoadStart");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("SessionListUpdated");
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log("Shutdown");

        const string LOBBY_SCENE = "Lobby";
        SceneManager.LoadScene(LOBBY_SCENE);
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        Debug.Log("ConnectedToServer");
    }

}
