using System;
using TMPro;
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
            await LobbyService.Instance.JoinLobbyByCodeAsync(code);
            Debug.Log("Joined lobby with code : " + code);
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
            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            Debug.Log("Joined lobby with ID : " + lobbyId);
            Debug.LogWarning("Lobby Code : " + lobby.LobbyCode);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log("e");
        }
    }
}
