using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateNickNamePanel : LobbyPanelBase
{
    [Header("Create Nick Name Panel Vars")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button createNickNameBtn;
    private const int MAX_CHAR_FOR_NICKNAME = 2;

    //Se llama al momento de iniciar 
    public override void InitPanel(LobbyUIManager lobbyUIManager)
    {
        base.InitPanel(lobbyUIManager);
        createNickNameBtn.interactable = false;
        createNickNameBtn.onClick.AddListener(OnClickCreateNickName);
        inputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    //Habilita o desabilita el boton de guardar NickName
    private void OnInputValueChanged(string arg)
    {
        createNickNameBtn.interactable = arg.Length >= MAX_CHAR_FOR_NICKNAME;
    }

    //METODO Crear nickName y habilitar-desabilitar x panel
    private void OnClickCreateNickName()
    {
        var nickName = inputField.text;
        if(nickName.Length >=MAX_CHAR_FOR_NICKNAME)
        {
            base.ClosePanel();
            //MOSTRAMOS EL PANEL QUE QUEREMOS ABRIR
            lobbyUIManager.ShowPanel(LobbyPanelType.MiddleSectionPanel);

        }
    }
}
