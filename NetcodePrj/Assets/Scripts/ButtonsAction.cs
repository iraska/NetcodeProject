using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ButtonsAction : MonoBehaviour
{
    private NetworkManager NetworkManager;
    public TextMeshProUGUI moveText;

    void Start()
    {
        NetworkManager = GetComponentInParent<NetworkManager>();
    }

    public void StartHost()
    {
        NetworkManager.StartHost();
        InitializeMovementText();
    }

    public void StartClient()
    {
        NetworkManager.StartClient();
        InitializeMovementText();
    }

    public void SubmitNewPosition()
    {
        var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        var player = playerObject.GetComponent<PlayerMovement>();
        player.Move();
    }

    private void InitializeMovementText()
    {
        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
            moveText.text = "MOVE";
    
        else if (NetworkManager.Singleton.IsClient)
            moveText.text = "Request Move";
    }
}
