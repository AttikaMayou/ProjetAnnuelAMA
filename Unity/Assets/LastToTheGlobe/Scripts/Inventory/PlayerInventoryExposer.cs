using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

//Auteur : Abdallah

public class PlayerInventoryExposer : MonoBehaviour
{
    public int id;
    public List<GameObject> playerSlot;

    [SerializeField] private PhotonView photonView;

    public void OnUsedItem(int id)
    {
        print("Hi !");
        if (playerSlot[id].transform.childCount > 2 && playerSlot[id].transform.GetChild(2).CompareTag("Potion"))
        {
            
        }
        else
        {
            print("Not a usable item");
        }
    }

    
}
