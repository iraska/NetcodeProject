using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEngine;

public class GetLobbies : MonoBehaviour
{
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void GetLobbiesTest()
    {
        try
        {
            QueryLobbiesOptions options = new();
            Debug.LogWarning("QueryLobbiesTest");
            options.Count = 25;

            // Filter for open lobbies only
            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0")
            };

            // Order by newest lobbies first
            options.Order = new List<QueryOrder>()
            {
                new QueryOrder(
                    asc: false,
                    field: QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
            Debug.LogWarning("Get Lobbies Done COUNT: " + lobbies.Results.Count);

            foreach (Lobby findingLobby in lobbies.Results)
            {
                Debug.Log("Lobby name : " + findingLobby.Name + "\n" + "Lobby creation time : " + findingLobby.Created);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("e");
        }
    }
}
