using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Core;
using Unity.Services.Authentication;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GetLobbies : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject buttonsContainer;
    
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void GetLobbiesTest()
    {
        ClearContainer();
        
        try
        {
            QueryLobbiesOptions options = new();
            Debug.LogWarning("QueryLobbiesTest");

            options.Count = 25;

            // Filter for open lobbies only
            options.Filters = new List<QueryFilter>()
            {
                // Boşluk olan lobileri getir
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0"),

                // S1 conquestleri getir
                // new QueryFilter(QueryFilter.FieldOptions.S1, "Conquest", QueryFilter.OpOptions.EQ)
            };

            // Order by latest lobbies first, if u want newest make "asc: false"
            options.Order = new List<QueryOrder>()
            {
                new QueryOrder(
                    asc: true,
                    field: QueryOrder.FieldOptions.Created)
            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
            Debug.LogWarning("Get Lobbies Done COUNT: " + lobbies.Results.Count);

            foreach (Lobby findingLobby in lobbies.Results)
            {
                //Debug.Log("Lobby name : " + findingLobby.Name + "\n" + "Lobby creation time : " + findingLobby.Created + "\n" + "Lobby code = " + findingLobby.LobbyCode + "\n" + "Lobby ID = " + findingLobby.Id);
                LobbyStatic.LogLobby(findingLobby);
                CreateLobbyButton(findingLobby);
            }

            //GetComponent<JoinLobby>().JoinLobbyWithLobbyId(lobbies.Results[0].Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("e");
        }
    }

    private void CreateLobbyButton(Lobby lobby)
    {
        var button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);      // it means no rotation
        button.name = lobby.Name + "_button";
        button.GetComponentInChildren<TextMeshProUGUI>().text = lobby.Name;

        var rectTransform = button.GetComponent<RectTransform>();
        rectTransform.SetParent(buttonsContainer.transform);

        button.GetComponent<Button>().onClick.AddListener(delegate() { Lobby_OnClick(lobby); });
    }

    public void Lobby_OnClick(Lobby lobby)
    {
        Debug.Log("Clicked Lobby : " + lobby.Name);
        
        GetComponent<JoinLobby>().JoinLobbyWithLobbyId(lobby.Id);
    }

    private void ClearContainer()
    {
        if (buttonsContainer != null && buttonsContainer.transform.childCount > 0)      // it means, it has button inside
        {
            foreach (Transform Variable in buttonsContainer.transform)
            {
                Destroy(Variable.gameObject);
            }
        }
    }
}
