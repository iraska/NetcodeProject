using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class CreateLobby : MonoBehaviour
{
    public TMP_InputField LobbyName;
    public TMP_Dropdown MaxPlayers;
    public Toggle isLobbyPrivate;

    public async void CreateLobbyMethod()
    {
        string lobbyName = LobbyName.text;
        int maxPlayers = Convert.ToInt32(MaxPlayers.options[MaxPlayers.value].text);
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = isLobbyPrivate.isOn;

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
        DontDestroyOnLoad(this);
        Debug.Log("Create lobby done!");
    }
}
