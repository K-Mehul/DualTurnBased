using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviourPunCallbacks
{
    private int maxHealth = 100;
    public int totalHealth;
    public GameObject btn;
    public GameObject healthBar;
    private void Start()
    {
        healthBar = GameObject.Find("HEALTH");
        btn = GameObject.Find("ATTACK");
        totalHealth = maxHealth;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            btn.GetComponent<Button>().interactable = true;
        else
            btn.GetComponent<Button>().interactable = false;

        btn.GetComponent<Button>().onClick.RemoveAllListeners();
        btn.GetComponent<Button>().onClick.AddListener(OnBitton);
    }

    [PunRPC]
    public void Change()
    {
        if (photonView.IsMine)
            btn.GetComponent<Button>().interactable = true;
        else
            btn.GetComponent<Button>().interactable = false;
    }

    public void OnBitton()
    {
        int damageVAlue = Random.Range(1,10);
        photonView.RPC("Change", RpcTarget.All);
        photonView.RPC("Damage",RpcTarget.All,damageVAlue);
    }

    [PunRPC]
    public void Damage(int damage){
        
        if(photonView.IsMine)
        {
            totalHealth -= damage;
            RefreshHealth();
        }

        if(totalHealth <= 0){
            photonView.RPC("DIE",RpcTarget.All);
        }
        
    }

    void RefreshHealth()
    {
        float bar = (float)totalHealth/(float)maxHealth;
        healthBar.transform.localScale = new Vector3(bar,1,1);
    }

    [PunRPC]
    void DIE(){
        Debug.Log("PLayer Die");
    }
}
