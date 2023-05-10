using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PopulateUI : MonoBehaviour
{
    private CurrentLobby _currentLobby;

    public TextMeshProUGUI lobbyName, lobbyCode, gameMode;
    public TMP_InputField newLobbyName, newPlayerLevel;

    public GameObject playerInfoContainer, playerInfoPrefab;

    private string lobbyId;

    void Start()
    {
        _currentLobby = GameObject.Find("Lobby Manager").GetComponent<CurrentLobby>();
        PopulateUIElements();

        lobbyId = _currentLobby.currentLobby.Id;
        InvokeRepeating(nameof(PollForLobbyUpdateAsync), 1.1f, 3f);

        LobbyStatic.LogPlayersInLobby(_currentLobby.currentLobby);
    }

    private void PopulateUIElements()
    {
        lobbyName.text = _currentLobby.currentLobby.Name;
        lobbyCode.text = _currentLobby.currentLobby.LobbyCode;
        gameMode.text = _currentLobby.currentLobby.Data["GameMode"].Value;

        ClearContainer();

        foreach (Player player in _currentLobby.currentLobby.Players)
        {
            CreatePlayerInfoCard(player);
        }
    }

    private void CreatePlayerInfoCard(Player player)
    {
        var text = Instantiate(playerInfoPrefab, Vector3.zero, Quaternion.identity);
        text.name = player.Joined.ToShortTimeString();
        text.GetComponent<TextMeshProUGUI>().text = player.Id + ";" + player.Data["PlayerLevel"].Value;

        var rectTransform = text.GetComponent<RectTransform>();
        rectTransform.SetParent(playerInfoContainer.transform);
    }

    private async void PollForLobbyUpdateAsync()
    {
        _currentLobby.currentLobby = await LobbyService.Instance.GetLobbyAsync(lobbyId);
        PopulateUIElements();
    }
    
    private void ClearContainer()
    {
        if (playerInfoContainer != null && playerInfoContainer.transform.childCount > 0)      // it means, it has button inside
        {
            foreach (Transform Variable in playerInfoContainer.transform)
            {
                Destroy(Variable.gameObject);
            }
        }
    }

    // Button Events
    public async void ChangeLobbyName()
    {
        var newLobbyName = this.newLobbyName.text;

        try
        {
            UpdateLobbyOptions options = new UpdateLobbyOptions();
            options.Name = newLobbyName;

            _currentLobby.currentLobby = await Lobbies.Instance.UpdateLobbyAsync(lobbyId, options);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void ChangePlayerName()
    {
        var playerLevel = newPlayerLevel.text;

        try
        {
            UpdatePlayerOptions options = new UpdatePlayerOptions();
            options.Data = new Dictionary<string, PlayerDataObject>()
            {
                {"PlayerLevel", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerLevel)}
            };

            await LobbyService.Instance.UpdatePlayerAsync(lobbyId, AuthenticationService.Instance.PlayerId, options);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
