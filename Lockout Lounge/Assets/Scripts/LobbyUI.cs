using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Added for TextMeshPro

public class LobbyUI : MonoBehaviour
{
    public TMP_Text roomCodeText;
    public Transform playerListContent;
    public GameObject playerEntryPrefab;

    private ConnectionApproval approval;
    private Dictionary<ulong, GameObject> entries = new();

    void Start()
    {
        approval = FindObjectOfType<ConnectionApproval>();
        roomCodeText.text = $"Room Code: {approval.RoomCode.Value}";
        approval.RoomCode.OnValueChanged += (_, newCode) =>
            roomCodeText.text = $"Room Code: {newCode}";

        NetworkManager.Singleton.OnClientConnectedCallback += AddPlayer;
        NetworkManager.Singleton.OnClientDisconnectCallback += RemovePlayer;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
            AddPlayer(client.ClientId);
    }

    private void AddPlayer(ulong clientId)
    {
        if (entries.ContainsKey(clientId)) return;
        var go = Instantiate(playerEntryPrefab, playerListContent);
        go.GetComponent<TMP_Text>().text = $"Player {clientId}"; // Changed from Text to TMP_Text
        entries[clientId] = go;
    }

    private void RemovePlayer(ulong clientId)
    {
        if (!entries.ContainsKey(clientId)) return;
        Destroy(entries[clientId]);
        entries.Remove(clientId);
    }
}
