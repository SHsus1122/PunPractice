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
    public Text ValueText, PlayersText, ClickUpgradeText, AutoUpgradeText, ValuePerClickText, ValuePerSecondText;
    public Button ClickUpgradeBtn, AutoUpgradeBtn;

    float nextTime;

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
        Player.value += Player.valuePerClick;
    }


    public void ClickUpgrade()
    {
        PlayerScript Player = FindPlayer();

        if (Player.value >= Player.ClickUpGradeCost)
        {
            Player.value -= Player.ClickUpGradeCost;
            Player.valuePerClick += Player.ClickUpGradeAdd;
            Player.ClickUpGradeCost += (Player.ClickUpGradeAdd * 10);
            Player.ClickUpGradeAdd += 2;

            ClickUpgradeText.text = "비용 : " + Player.ClickUpGradeCost + "\n+" + Player.ClickUpGradeAdd + "/클릭";
            ValuePerClickText.text = Player.valuePerClick.ToString();
        }
    }

    public void AutoUpgrade()
    {
        PlayerScript Player = FindPlayer();

        if (Player.value >= Player.AutoUpGradeCost)
        {
            Player.value -= Player.AutoUpGradeCost;
            Player.valuePerSecond += Player.AutoUpGradeAdd;
            Player.AutoUpGradeCost += 500;
            Player.AutoUpGradeAdd += 2;

            AutoUpgradeText.text = "비용 : " + Player.AutoUpGradeCost + "\n+" + Player.AutoUpGradeAdd + "/초";

            ValuePerSecondText.text = Player.valuePerSecond.ToString();
        }
    }

    void ShowPlayers()
    {
        string playersText = "";
        foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player"))
        {
            playersText += Player.GetPhotonView().Owner.NickName + " / " +
                Player.GetComponent<PlayerScript>().value.ToString() + "\n";
        }
        PlayersText.text = playersText;
    }

    void EnableUpgrade()
    {
        PlayerScript Player = FindPlayer();
        ClickUpgradeBtn.interactable = Player.value >= Player.ClickUpGradeCost;
        AutoUpgradeBtn.interactable = Player.value >= Player.AutoUpGradeCost;
    }

    void ValuePerSecond()
    {
        PlayerScript Player = FindPlayer();
        Player.value += Player.valuePerSecond;
    }

    private void Update()
    {
        if (!PhotonNetwork.InRoom)
            return;

        ShowPlayers();
        EnableUpgrade();

        if (Time.time > nextTime)
        {
            nextTime = Time.time + 1;
            ValuePerSecond();
        }
    }
}
