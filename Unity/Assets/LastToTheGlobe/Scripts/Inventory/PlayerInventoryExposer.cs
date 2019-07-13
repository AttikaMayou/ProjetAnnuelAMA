using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

//Auteur : Abdallah

public class PlayerInventoryExposer : MonoBehaviour
{
    //Set in characterExposer
    public int playerId;
    public List<GameObject> playerSlot;

    [SerializeField] private PhotonView photonView;

    public void OnUsedItem(int id)
    {
        if (playerSlot[id].transform.childCount > 2 && playerSlot[id].transform.GetChild(0).CompareTag("Potion"))
        {
            photonView.RPC("WantToUseItem", RpcTarget.MasterClient, playerSlot[id].transform.GetChild(0).tag, playerId);
        }
        else
        {
            print("Not a usable item");
        }
    }

    
}
