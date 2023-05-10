using UnityEngine.SceneManagement;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public static class LobbyStatic
{
    public static void LogPlayersInLobby(Lobby lobby)
    {
        foreach (Player player in lobby.Players)
        {
            Debug.Log("Player ID = " + player.Id);
            Debug.Log("Player Level = " + player.Data["PlayerLevel"].Value);
        }
    }

    public static void LogLobby(Lobby lobby)
    {
        Debug.Log("Lobby Id : " + lobby.Id + "\n" + "GameMode = " + lobby.Data["GameMode"].Value);
    }

    public static void LoadLobbyRoom()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}