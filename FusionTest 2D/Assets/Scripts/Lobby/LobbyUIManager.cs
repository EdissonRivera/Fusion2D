using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private LoadingCanvasController loadingCanvasControllerPrefab;
    [SerializeField] private LobbyPanelBase[] lobbyPanel;
    // Start is called before the first frame update
    void Start()
    {
        //INICIALIZA VARIABLES DE TIPO LOBBYYUMANAGER
        foreach (var lobby in lobbyPanel) 
        {
            lobby.InitPanel(this);
        }

        Instantiate(loadingCanvasControllerPrefab);
    }

    //MOSTRAR UN PANEL DETERMINADO
    public void ShowPanel(LobbyPanelBase.LobbyPanelType type)
    {
        foreach(var lobby in lobbyPanel)
        {
            if (lobby.PanelType == type)
            {
                //Mostrar x panel
                lobby.ShowPanel();
                break;
            }
        }
    }
}
