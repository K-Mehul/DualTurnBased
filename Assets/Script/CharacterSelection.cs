using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    public List<GameObject> playerPrefab = new List<GameObject>();
    private int selectedPlayer;

private void Start() {

    selectedPlayer = PlayerPrefs.GetInt("playerValue",0);
    for(int i= 0;i<playerPrefab.Count;i++){
        if(i == selectedPlayer)
            playerPrefab[selectedPlayer].SetActive(true);
    }
}

    public void Next(){
        playerPrefab[selectedPlayer].SetActive(false);
        selectedPlayer ++;
        if(selectedPlayer > playerPrefab.Count-1){
            selectedPlayer = 0;
        }
        playerPrefab[selectedPlayer].SetActive(true);
    }

    public void Previos(){
        playerPrefab[selectedPlayer].SetActive(false);
        selectedPlayer--;
        if(selectedPlayer < 0){
            selectedPlayer = playerPrefab.Count -1;
        }
        playerPrefab[selectedPlayer].SetActive(true);
    }

    public void Submit(){
        string selectedPlayerName = playerPrefab[selectedPlayer].name;
        PlayerPrefs.SetString("playerName",selectedPlayerName);
        PlayerPrefs.SetInt("playerValue",selectedPlayer);
    }
}
