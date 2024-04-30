using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")]
    public GameObject DisconnectPanel;
    public InputField NicknameInput;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ValueText, PlayersText;

    void Start()
    {
        Screen.SetResolution(540, 960, false);
    }

    public void Connect()
    {
        PhotonNetwork.LocalPlayer.NickName = NicknameInput.text;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }

    void ShowPanel(GameObject CulPanel)
    {
        DisconnectPanel.SetActive(false);
        RoomPanel.SetActive(false);
        CulPanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        ShowPanel(RoomPanel);
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
    }

    PlayerScript FindPlayer()
    {
        foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Player.GetPhotonView().IsMine)
                return Player.GetComponent<PlayerScript>();
        }
        return null;
    }

    public void Click()
    {
        PlayerScript Player = FindPlayer();
        Player.value += 1;
    }

    void ShowPlayers()
    {
        string playersText = "";
        foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player"))
        {
            playersText = Player.GetPhotonView().Owner.NickName + " / " +
                Player.GetComponent<PlayerScript>().value.ToString() + "\n";
        }
        PlayersText.text = playersText;
    }

    private void Update()
    {
        ShowPlayers();
    }
}
