using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using UnityEngine;

public class CreateLobby : MonoBehaviour
{
    public TMP_InputField LobbyName, LobbyCode;
    public TMP_Dropdown MaxPlayers, GameMode;
    public Toggle isLobbyPrivate;

    public async void CreateLobbyMethod()
    {
        string lobbyName = LobbyName.text;
        int maxPlayers = Convert.ToInt32(MaxPlayers.options[MaxPlayers.value].text);
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = isLobbyPrivate.isOn;

        // Player creation
        options.Player = new Player(AuthenticationService.Instance.PlayerId);
        options.Player.Data = new Dictionary<string, PlayerDataObject>() 
        { 
            {"PlayerLevel", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "5")}
        };

        // Lobby data
        options.Data = new Dictionary<string, DataObject>()
        {
            {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, GameMode.options[GameMode.value].text, DataObject.IndexOptions.S1)}
        };

        Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);
        GetComponent<CurrentLobby>().currentLobby = lobby;

        DontDestroyOnLoad(this);        // Bu sayede Lobby Manager sahne reload dahi silinmez.
        Debug.Log("Create lobby done!");

        LobbyStatic.LogLobby(lobby);
        LobbyStatic.LogPlayersInLobby(lobby);
        LobbyCode.text = lobby.LobbyCode;

        StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15f));

        LobbyStatic.LoadLobbyRoom();
    }

    IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)      /// if you do not do this, lobby shuts down
    {
        var delay = new WaitForSeconds(waitTimeSeconds);
        while(true)
        {
            LobbyService.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
}
