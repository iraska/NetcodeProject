using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class JoinLobby : MonoBehaviour
{
    public TMP_InputField inputField;
    
    public async void JoinLobbyWithLobbyCode(string lobbyCode)
    {
        var code = inputField.text;

        try
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();

            options.Player = new Player(AuthenticationService.Instance.PlayerId);
            options.Player.Data = new Dictionary<string, PlayerDataObject>() 
            { 
                {"PlayerLevel", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "8")}
            };
            
            Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);

            Debug.Log("Joined lobby with code : " + code);

            DontDestroyOnLoad(this);        // Bu sayede Lobby Manager sahne reload dahi silinmez.
            GetComponent<CurrentLobby>().currentLobby = lobby;

            LobbyStatic.LogPlayersInLobby(lobby);
            LobbyStatic.LoadLobbyRoom();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("e");
        }
    }

    public async void JoinLobbyWithLobbyId(string lobbyId)
    {
        try
        {
            JoinLobbyByIdOptions options = new JoinLobbyByIdOptions();

            options.Player = new Player(AuthenticationService.Instance.PlayerId);
            options.Player.Data = new Dictionary<string, PlayerDataObject>() 
            { 
                {"PlayerLevel", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "8")}
            };

            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId, options);

            Debug.Log("Joined lobby with ID : " + lobbyId);
            Debug.LogWarning("Lobby Code : " + lobby.LobbyCode);

            DontDestroyOnLoad(this);
            GetComponent<CurrentLobby>().currentLobby = lobby;

            LobbyStatic.LogPlayersInLobby(lobby);
            LobbyStatic.LoadLobbyRoom();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void QuickJoinMethodAsync()
    {
        try
        {
            Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            GetComponent<CurrentLobby>().currentLobby = lobby;
            DontDestroyOnLoad(this);
            
            Debug.Log("Joined Lobby with Quick Join : " + lobby.Id);
            Debug.LogWarning("Lobby Code : " + lobby.LobbyCode);

            LobbyStatic.LoadLobbyRoom();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
