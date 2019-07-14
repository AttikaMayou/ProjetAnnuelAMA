using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

//Auteur: Margot

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject playerInventory;
    [SerializeField] private GameObject tutorial;
    [SerializeField] private Text nbPlayerText;
    [SerializeField] private int life;
    private bool canOpenInventory = false;


    void Start()
    {
        playerInventory.SetActive(false);
        tutorial.SetActive(false);
    }

    void Update()
    {
        //UI Inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(!canOpenInventory)
            {
                playerInventory.SetActive(true);
                canOpenInventory = true;
            }
            else if(canOpenInventory)
            {
                playerInventory.SetActive(false);
                canOpenInventory = false;
            }
        }
        //UI Tutorial
        if (Input.GetKey(KeyCode.F1))
        {
            tutorial.SetActive(true);
        }
        else
        {
            tutorial.SetActive(false);
        }
    }

    //Player currently in the game
    public void updateNbPlayer()
    {
        int nbPlayers = PhotonNetwork.PlayerList.Length;
        nbPlayerText.text = "Joueurs : " + nbPlayers.ToString();
    }
}
