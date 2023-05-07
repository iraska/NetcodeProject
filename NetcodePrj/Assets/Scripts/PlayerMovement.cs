using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    // ağ değişkeni, sadece bulunduğu script'i değil bütün ağı ilgilendiren değişkenlere denir
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            Move();
        }
    }

    void Update()
    {
        transform.position = Position.Value;
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)          // If we server
        {
            var randomPos = GetRandomPositionOnPlane();     // generate a random pos
            transform.position = randomPos;                 // equate that randomPos to own position
            Position.Value = randomPos;                     // since the position you produce is defined only on your computer, you should equate this value to Position.Value
        }
        else                                            // if we client
        {
            SubmitPositionRequestServerRpc();
        }
    }

    [ServerRpc]            // Add tag to this function
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Position.Value = GetRandomPositionOnPlane();
    }

    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(UnityEngine.Random.Range(-3f, 3f), 1f, UnityEngine.Random.Range(-3f, 3f));
    }
}
