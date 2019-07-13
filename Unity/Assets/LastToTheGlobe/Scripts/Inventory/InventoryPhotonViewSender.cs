using System.Collections;
using System.Collections.Generic;
using LastToTheGlobe.Scripts.Inventory;
using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

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
}
