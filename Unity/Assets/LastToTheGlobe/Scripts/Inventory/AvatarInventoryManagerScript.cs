using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Inventory;
using Photon.Pun;
using UnityEngine;

//Auteur : Abdallah

public class AvatarInventoryManagerScript : MonoBehaviour
{
    public CharacterExposerScript selfExposer;

    [SerializeField] private PhotonView inventoryPhotonView;

    private ObjectScript objectToAdd;

    // Start is called before the first frame update
    void Start()
    {
        selfExposer.ChestInventory.SetActive(false);
        selfExposer.PlayerInventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if an item have been added
        foreach (var item in selfExposer.InventoryExposer.playerSlot)
        {
            if (item.transform.childCount > 2 && !item.transform.GetChild(2).CompareTag("Untagged"))
            {
                item.transform.GetChild(2).SetAsFirstSibling();
                //Send RPC to add item
                
                inventoryPhotonView.RPC("AddItemToInventory", RpcTarget.MasterClient, item.transform.GetChild(0).tag, selfExposer.Id);
                
            }
        }
        
    }
}
