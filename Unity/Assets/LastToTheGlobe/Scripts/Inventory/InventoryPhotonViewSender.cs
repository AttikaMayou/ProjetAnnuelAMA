using System.Collections;
using System.Collections.Generic;
using LastToTheGlobe.Scripts.Inventory;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

//Auteur : Abdallah

public class InventoryPhotonViewSender : MonoBehaviour
{
    [PunRPC]
    void AddItemToInventory(string itemName, int playerid)
    {
        Debug.LogFormat("Le joueur avec l'id {0} veux ajouter un {1}", playerid, itemName);
        var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerid);
        
        var objectToAdd = new ObjectScript();
        objectToAdd.objectName = itemName;
        switch (itemName)
        {
            case "Potion":
                objectToAdd.itemType = ObjectScript._typeOfItem.Consumable;
                break;
            case "Dash":
                objectToAdd.itemType = ObjectScript._typeOfItem.Skill;
                break;
        }

        objectToAdd.isConsume = false;
        objectToAdd.isInInventory = true;
        
        player.inventoryScript.AddObjectInInventory(objectToAdd);   
    }

    [PunRPC]
    void WantToUseItem(string itemName, int playerid)
    {
        var player = ColliderDirectoryScript.Instance.GetCharacterExposer(playerid);
        
        print("Hi !");
        if(!player.inventoryScript.isItemInInventory(itemName))
        {
            print("Healed :D");
        }
        else
        {
            print("You don't have this item dude");
        }

    }
}
