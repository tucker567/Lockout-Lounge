using System.Text;
using Unity.Netcode;
using UnityEngine;

public class ConnectionApproval : MonoBehaviour
{
    public NetworkVariable<string> RoomCode = new NetworkVariable<string>(
        "------",
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private void Awake()
    {
        // keep this alive across scene loads
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        var nm = NetworkManager.Singleton;
        if (nm == null)
        {
            Debug.LogError("🔥 No NetworkManager.Singleton found in ConnectionApproval.Start()");
            return;
        }

        nm.ConnectionApprovalCallback += ApproveConnection;

        if (nm.IsServer)
        {
            RoomCode.Value = GenerateRoomCode();
            Debug.Log($"🛡️ Room code: {RoomCode.Value}");
        }
    }

    private void ApproveConnection(NetworkManager.ConnectionApprovalRequest req, NetworkManager.ConnectionApprovalResponse res)
    {
        var clientCode = Encoding.UTF8.GetString(req.Payload);
        res.Approved           = clientCode == RoomCode.Value;
        res.CreatePlayerObject = false;
        res.Pending            = false;
    }

    private string GenerateRoomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var sb  = new StringBuilder(6);
        var rnd = new System.Random();
        for (int i = 0; i < 6; i++)
            sb.Append(chars[rnd.Next(chars.Length)]);
        return sb.ToString();
    }
}
