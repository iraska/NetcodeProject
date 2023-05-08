using System;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class CreateLobby : MonoBehaviour
{
    public TMP_InputField LobbyName;
    public TMP_InputField LobbyCode;
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

        LobbyCode.text = lobby.LobbyCode;

        StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15f));
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
