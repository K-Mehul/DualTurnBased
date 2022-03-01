using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class UIManager : MonoBehaviourPunCallbacks
{

    public static UIManager instance;

    [Header("Panels")]
    public GameObject characterPanel;
    public GameObject lobbyPanel;
    public GameObject mainPanel;
    public GameObject loadingPanel;

    public Text roomNameTxt;

    private void Awake() {
        if(instance == null)
            instance = this;

        DontDestroyOnLoad(this);
    }

    private void Start() {
        roomNameTxt = GameObject.Find("ROOM").GetComponent<Text>();


        roomNameTxt.text = PhotonNetwork.CurrentRoom.Name;
    }

    private void Update() {
        if(roomNameTxt == null){
            roomNameTxt = GameObject.Find("ROOM").GetComponent<Text>();
            roomNameTxt.text = PhotonNetwork.CurrentRoom.Name;
        }
    }

    public override void OnConnected()
    {
        ActivatePanel(loadingPanel.name);
    }
    public override void OnConnectedToMaster()
    {
        ActivatePanel(mainPanel.name);
    }
    public void ActivatePanel(string panelName)
    {
        loadingPanel.SetActive(panelName.Equals(loadingPanel.name));
        mainPanel.SetActive(panelName.Equals(mainPanel.name));
        characterPanel.SetActive(panelName.Equals(characterPanel.name));
        lobbyPanel.SetActive(panelName.Equals(lobbyPanel.name));
    }
}
