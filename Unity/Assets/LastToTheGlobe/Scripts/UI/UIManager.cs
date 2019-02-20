using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

//Auteur: Margot

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas playerInventory;
    [SerializeField] private Canvas tutorial;
    [SerializeField] private Text nbPlayerText;
    [SerializeField] private int life;

    void Start()
    {
        playerInventory.enabled = false;
        tutorial.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            playerInventory.enabled = true;
        }

        if (Input.GetKey(KeyCode.F1))
        {
            tutorial.enabled = true;
        }
        else
        {
            tutorial.enabled = false;
        }
    }
    
    //Player currently in the game
    public void updateNbPlayer()
    {
        int nbPlayers = PhotonNetwork.PlayerList.Length;
        nbPlayerText.text = "Joueurs : " + nbPlayers.ToString();
    }
}
