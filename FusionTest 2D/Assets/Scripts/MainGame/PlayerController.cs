using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour,IBeforeUpdate
{
    [SerializeField] private float moveSpeed = 6;
    private float horizontal;
    private Rigidbody2D rigid;

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
        if (Runner.TryGetInputForPlayer<PlayerData>(Object.InputAuthority,out var input)) //extrae datos de la estructura PlayerData y valida si yo soy la autoridad de entrada de este personaje
        {
            rigid.velocity = new Vector2(input.HorizontalInput * moveSpeed, rigid.velocity.y); //si esto s ecumple muevo al personaje
        }
    }

    //extrae valores para asignarlos luego en locallInputPoller
    public PlayerData GetPlayerNetworkInput()
    {
        PlayerData data = new PlayerData();
        data.HorizontalInput = horizontal;
        return data;
    }

}
