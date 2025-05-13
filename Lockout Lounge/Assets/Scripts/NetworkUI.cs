using System.Text;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkUI : MonoBehaviour
{
    public InputField codeInputField;
    private ConnectionApproval approval;

    void Start()
    {
        approval = FindObjectOfType<ConnectionApproval>();
        NetworkManager.Singleton.OnClientConnectedCallback += _ =>
            SceneManager.LoadScene("Lobby");
    }

    public void HostGame()
    {
        NetworkManager.Singleton.StartHost();
        // no more hostCodeText — the code is generated in ConnectionApproval
        SceneManager.LoadScene("Lobby");
    }

    public void JoinGame()
    {
        var payload = Encoding.UTF8.GetBytes(codeInputField.text);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = payload;
        NetworkManager.Singleton.StartClient();
    }
}
