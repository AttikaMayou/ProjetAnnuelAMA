using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Inventory;
using UnityEngine;

public class AvatarInventoryManagerScript : MonoBehaviour
{
    public CharacterExposerScript selfExposer;
    public PlayerInventoryExposer InventoryExposer;

    private ObjectScript objectToAdd;
    [SerializeField] private InventoryScript inventoryScript;

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

            if (item.transform.childCount > 1)
            {
                string tag = item.transform.GetChild(1).tag;
                if (!item.transform.GetChild(1).CompareTag("Untagged"))
                {
                    objectToAdd = new ObjectScript();
                    objectToAdd.objectName = tag;
                    switch (tag)
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
                    inventoryScript.AddObjectInInventory(objectToAdd);
                }
            }
        }
        
    }
}
