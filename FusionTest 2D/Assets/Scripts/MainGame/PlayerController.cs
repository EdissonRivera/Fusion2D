using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour, IBeforeUpdate
{
    [SerializeField] private float moveSpeed = 6;
    [SerializeField] private float jumpForce = 1000;
    [Networked] private NetworkButtons buttonsPrev { get; set; }
    private float horizontal;
    private Rigidbody2D rigid;

    private enum PlayerInputButtons
    {
        None,
        Jump
    }

    public override void Spawned()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    //se ejecuta antes de que fusion realize cualquier cambio en la red
    public void BeforeUpdate()
    {
        //si somos el jugador local
        if (Object.HasInputAuthority)
        {
            const string HORIZONTAL = "Horizontal";
            horizontal = Input.GetAxisRaw(HORIZONTAL);
        }
    }

    public override void FixedUpdateNetwork() //Actualizacion en tiempo de red
    {
        if (Runner.TryGetInputForPlayer<PlayerData>(Object.InputAuthority, out var input)) //extrae datos de la estructura PlayerData y valida si yo soy la autoridad de entrada de este personaje
        {
            rigid.velocity = new Vector2(input.HorizontalInput * moveSpeed, rigid.velocity.y); //si esto s ecumple muevo al personaje
            CheckJumpInput(input);
        }
    }


    private void CheckJumpInput(PlayerData input)
    {
        var preseed = input.NetworkButtons.GetPressed(buttonsPrev);
        if (preseed.WasPressed(buttonsPrev, PlayerInputButtons.Jump))
        {
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        }

        buttonsPrev = input.NetworkButtons;
    }

    //extrae valores para asignarlos luego en locallInputPoller
    public PlayerData GetPlayerNetworkInput()
    {
        PlayerData data = new PlayerData();
        data.HorizontalInput = horizontal;
        data.NetworkButtons.Set(PlayerInputButtons.Jump, Input.GetKey(KeyCode.Space));
        return data;
    }

}
