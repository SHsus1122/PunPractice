using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviourPunCallbacks, IPunObservable
{
    NetworkManager NM;
    PhotonView PV;
    public PlayerScript player;

    void Start()
    {
        PV = photonView;
        NM = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();

        SetPlayer();
        SetParent();
    }

    void SetPlayer()
    {
        Debug.Log("[ Weapon ] SetPlayer Call");
        foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (this.PV.Owner.NickName == Player.GetPhotonView().Owner.NickName)
            {
                player = Player.GetComponent<PlayerScript>();
            }
        }
    }

    void SetParent()
    {
        Debug.Log("[ Weapon ] SetParent Call");
        for (int i = 0; i < NM.PlayerList.Count; i++) 
        {
            if (NM.PlayerList[i].GetPhotonView().Owner.NickName == this.PV.Owner.NickName)
            {
                Debug.Log("[ Weapon ] SetParent Find User");
                this.transform.parent = NM.PlayerList[i].transform;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
