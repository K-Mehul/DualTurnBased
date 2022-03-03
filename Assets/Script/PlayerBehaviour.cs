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
    public Image healthBar;
    public Text winLoseTxt;
    public GameObject leaveBtn;

    string level;
    private void Start()
    {
        leaveBtn = GameObject.Find("LEAVE");
        healthBar = GameObject.Find("HEALTH").GetComponent<Image>();
        btn = GameObject.Find("ATTACK");
        winLoseTxt = GameObject.Find("WINLOSE").GetComponent<Text>();
        winLoseTxt.enabled = false;
        totalHealth = maxHealth;
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            btn.GetComponent<Button>().interactable = true;
        else
            btn.GetComponent<Button>().interactable = false;

        btn.GetComponent<Button>().onClick.RemoveAllListeners();
        btn.GetComponent<Button>().onClick.AddListener(Attack);

        leaveBtn.GetComponent<Button>().onClick.RemoveAllListeners();
        leaveBtn.GetComponent<Button>().onClick.AddListener(LeftRoom);
        RefreshHealth();
    }

    [PunRPC]
    public void Change()
    {
        if (photonView.IsMine)
            btn.GetComponent<Button>().interactable = true;
        else
            btn.GetComponent<Button>().interactable = false;
    }

    public void Attack()
    {
        int damageVAlue = Random.Range(10,20);
        photonView.RPC("Change", RpcTarget.All);
        photonView.RPC("Damage",RpcTarget.All,damageVAlue);
    }

    [PunRPC]
    public void Damage(int damage){
        
        totalHealth -= damage;
        if(photonView.IsMine)
        {
            RefreshHealth();
        }

        if(totalHealth <= Mathf.Epsilon){
            DIE();
        }
    }

    void RefreshHealth()
    {
        float bar = (float)totalHealth/(float)maxHealth;
        healthBar.fillAmount = bar;
    }

    void DIE()
    {
        winLoseTxt.enabled = true;
        btn.SetActive(false);
        if (photonView.IsMine)
        {
            if (healthBar.fillAmount <= Mathf.Epsilon)
                winLoseTxt.text = "YOU LOSE";
        }
        else
        {
            winLoseTxt.text = "YOU WIN";
        }
        leaveBtn.SetActive(true);    
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(level);
    }

    private void LeftRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
