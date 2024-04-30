using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public int value, valuePerClick, ClickUpGradeCost, ClickUpGradeAdd, valuePerSecond, AutoUpGradeCost, AutoUpGradeAdd;
    public GameObject weaponPrefab;
    public int cnt;

    NetworkManager NM;
    PhotonView PV;

    void Start()
    {
        PV = photonView;
        NM = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();

        NM.PlayerList.Add(this.gameObject);
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

    public void AddWeapon(int photonViewId)
    {
        PV.RPC("SpawnWeaponOnOtherClients", RpcTarget.All, photonViewId);
    }

    [PunRPC]
    public void SpawnWeaponOnOtherClients(int photonViewId)
    {
        foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Player.GetPhotonView().ViewID == photonViewId && Player.GetPhotonView().IsMine)
            {
                Debug.Log("[ PlayerScripts ] Spawn : " + photonViewId);
                Weapon weapon = PhotonNetwork.Instantiate(weaponPrefab.name, Vector3.zero, Quaternion.identity).GetComponent<Weapon>();
            }
        }
    }

    public void SetParentWeapon()
    {
        if (!PhotonNetwork.LocalPlayer.IsLocal)
            return;

        string owenerName = "";
        foreach (GameObject Player in GameObject.FindGameObjectsWithTag("Player"))
        {
            owenerName = Player.GetPhotonView().Owner.NickName;
            foreach (GameObject Weapon in GameObject.FindGameObjectsWithTag("Weapon"))
            {
                Debug.Log("Parent is : " + Weapon.transform.parent == null);
                if (Weapon.transform.parent == null)
                {
                    Debug.Log("Parent is null");
                    Weapon.transform.parent = Player.transform;
                }
            }
        }
    }
}
