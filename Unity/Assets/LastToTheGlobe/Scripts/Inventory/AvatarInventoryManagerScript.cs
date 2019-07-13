using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Inventory;
using Photon.Pun;
using UnityEngine;

public class AvatarInventoryManagerScript : MonoBehaviour
{
    public CharacterExposerScript selfExposer;
    public PlayerInventoryExposer InventoryExposer;

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
        foreach (var item in InventoryExposer.playerSlot)
        {
            if (item.transform.childCount > 2)
            {
                string tag = item.transform.GetChild(2).tag;
                if (!item.transform.GetChild(2).CompareTag("Untagged"))
                {
                    inventoryPhotonView.RPC("AddItemToInventory", RpcTarget.MasterClient, tag, selfExposer.Id);
                }
            }
        }
        
    }
}
