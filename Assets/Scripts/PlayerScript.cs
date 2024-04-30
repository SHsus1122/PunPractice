using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviourPun, IPunObservable
{
    public int value, valuePerClick, ClickUpGradeCost, ClickUpGradeAdd, valuePerSecond, AutoUpGradeCost, AutoUpGradeAdd;
    NetworkManager NM;
    PhotonView PV;

    void Start()
    {
        PV = photonView;
        NM = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        NM.ValueText.text = value.ToString();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(value);
        }
        else
        {
            value = (int)stream.ReceiveNext();
        }
    }
}
