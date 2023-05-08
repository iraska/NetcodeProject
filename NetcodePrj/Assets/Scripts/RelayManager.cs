using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    public TextMeshProUGUI IdText;
    public TextMeshProUGUI JoinCodeText;
    public TMP_Dropdown playerCount;
    public TMP_InputField inputField;
    private RelayHostData _hostData;
    private RelayJoinData _joinData;

    private string PlayerID;

    async void Start()
    {
        await UnityServices.InitializeAsync();

        Debug.Log("Unity Service Init");

        SignIn();
    }

    async void SignIn()
    {
        Debug.Log("Signing In");

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        PlayerID = AuthenticationService.Instance.PlayerId;

        Debug.Log("Signing In: " + PlayerID);
        IdText.text = PlayerID;
    }

    public async void OnHostClick()
    {   
        int maxPlayerCount = Convert.ToInt32(playerCount.options[playerCount.value].text);

        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayerCount);
        _hostData = new RelayHostData()
        {
            IPv4Address = allocation.RelayServer.IpV4,
            Port = (ushort)allocation.RelayServer.Port,

            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            Key = allocation.Key
        };

        _hostData.JoinCode = await RelayService.Instance.GetJoinCodeAsync(_hostData.AllocationID);
        Debug.Log("Allocate Complete :" + _hostData.AllocationID);

        Debug.LogWarning("Join Code = " + _hostData.JoinCode);
        JoinCodeText.text = _hostData.JoinCode;

        UnityTransport transport = NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();

        transport.SetRelayServerData(_hostData.IPv4Address, _hostData.Port, _hostData.AllocationIDBytes, _hostData.Key, _hostData.ConnectionData);
        NetworkManager.Singleton.StartHost();
    }

    public async void OnJoinClick()
    {
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(inputField.text);

        _joinData = new RelayJoinData()
        {
            IPv4Address = allocation.RelayServer.IpV4,
            Port = (ushort)allocation.RelayServer.Port,

            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            Key = allocation.Key
        };

        Debug.Log("Join Success : " + _joinData.AllocationID);

        UnityTransport transport = NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>();

        transport.SetRelayServerData(_joinData.IPv4Address, _joinData.Port, _joinData.AllocationIDBytes, _joinData.Key, _joinData.ConnectionData, _joinData.HostConnectionData);
        NetworkManager.Singleton.StartClient();
    }
}

public struct RelayHostData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] Key;
    }

    public struct RelayJoinData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] HostConnectionData;
        public byte[] Key;
    }
