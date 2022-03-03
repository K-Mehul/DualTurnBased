using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Panels")]
    public GameObject characterPanel;
    public GameObject lobbyPanel;
    public GameObject mainPanel;
    public GameObject loadingPanel;

    public Transform[] spawnPoint;
    public GameObject[] playerGameObjectList;
    Dictionary<int,GameObject> playerObjectList;
    public GameObject startGameBtn;

    string levelName = "Main";

    private void Awake() {
        PhotonNetwork.ConnectUsingSettings();
        ActivatePanel(loadingPanel.name);

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #region  Photon Method
    public override void OnJoinedRoom()
    {
        ActivatePanel(lobbyPanel.name);

        if(PhotonNetwork.LocalPlayer.IsMasterClient){
            startGameBtn.SetActive(true);
        }
        else{
            startGameBtn.SetActive(false);
        }
        
        if(playerObjectList == null)
            playerObjectList = new Dictionary<int,GameObject>();

        //string playerPrefabName = PlayerPrefs.GetString("playerName","Rogue_05");
        int playerVal = PlayerPrefs.GetInt("playerValue",0);
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            GameObject playerSpawn = Instantiate(playerGameObjectList[0]);
            playerSpawn.transform.SetParent(spawnPoint[0]);
            playerSpawn.transform.localScale = Vector3.one;
            playerObjectList.Add(player.ActorNumber,playerSpawn);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //string playerPrefabName = PlayerPrefs.GetString("playerName","Rogue_05");

       // int playerVal = PlayerPrefs.GetInt("playerValue",0);
        foreach(Player player in PhotonNetwork.PlayerList){
            GameObject playerSpawn = Instantiate(playerGameObjectList[0]);
            playerSpawn.transform.SetParent(spawnPoint[0]);
            playerSpawn.transform.localScale = Vector3.one;
            
            if(!playerObjectList.ContainsKey(player.ActorNumber))
                playerObjectList.Add(player.ActorNumber,playerSpawn);
        }
    }

    public override void OnLeftRoom()
    {
        ActivatePanel(mainPanel.name);

        foreach(GameObject player in playerObjectList.Values){
            Destroy(player);
        }

        playerObjectList.Clear();
        playerObjectList = null;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerObjectList[otherPlayer.ActorNumber].gameObject);
        playerObjectList.Remove(otherPlayer.ActorNumber);
        
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
            startGameBtn.SetActive(true);
    }


    public override void OnConnected()
    {
        ActivatePanel(loadingPanel.name);
    }
    public override void OnConnectedToMaster()
    {
        ActivatePanel(mainPanel.name);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }
    #endregion

    #region  Private Method
    private void CreateRoom(){
        string roomName = $"{Random.Range(0,1000)}"; 
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomName,options);
    }

    private void Level(){
        PhotonNetwork.LoadLevel(levelName);
    }
    #endregion
    #region Public Method
    public void QuickMatch(){
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    
    public void ActivatePanel(string panelName)
    {
        loadingPanel.SetActive(panelName.Equals(loadingPanel.name));
        mainPanel.SetActive(panelName.Equals(mainPanel.name));
        characterPanel.SetActive(panelName.Equals(characterPanel.name));
        lobbyPanel.SetActive(panelName.Equals(lobbyPanel.name));
    }

    public void QuitRoom(){
        PhotonNetwork.LeaveRoom();
    }

    public void StartMatch(){
        PhotonNetwork.LoadLevel(levelName);
    }

    #endregion
}
