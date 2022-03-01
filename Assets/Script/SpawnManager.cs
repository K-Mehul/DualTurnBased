using UnityEngine;
using Photon.Pun;
public class SpawnManager : MonoBehaviourPun
{
    public Transform[] spawnPoint;
    string playerPrefab;
    void Start()
    {
        playerPrefab = PlayerPrefs.GetString("playerName","Rogue_05");
        Spawn();
    }

    void Spawn(){
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
            PhotonNetwork.Instantiate(playerPrefab,spawnPoint[0].position,Quaternion.identity);
        else
            PhotonNetwork.Instantiate(playerPrefab,spawnPoint[1].position,Quaternion.Euler(0,-180,0));
    }
}
