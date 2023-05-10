using System;
using Unity.Netcode;
using UnityEngine;

public class TestServerRPC : NetworkBehaviour
{
    // RPC: remote procedure call
    // Bir cihazın başka bir cihaz üzerine kod çalıştırmasını sağlayan bir protokoldür.
    // Multiplayer oyunlarda cihazlar arası haberleşme yöntemidir.
    // RPC(Remote Procedure Call) için fonksiyon yazarken method ismi sonuna Rpc eklenmelidir ve methodun üstüne tag eklenmelidir.

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            TestServerRpc(0);
        }
    }

    [ClientRpc]
    void TestClientRPC(int value)
    {
        if (IsClient)
        {
            Debug.Log("Client Received the RPC #" + value);
            TestServerRpc(value + 1);
        }
    }

    [ServerRpc]
    void TestServerRpc(int value)
    {
        Debug.Log("Server Received the RPC #" + value);
        TestClientRPC(value);
    }
}
