using LastToTheGlobe.Scripts.Singleton;
using System.Collections;
using System.Collections.Generic;
using LastToTheGlobe.Scripts.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Auteur : Margot
//Modification : Abdallah

public class ItemSlotInventory : MonoBehaviourSingleton<ItemSlotInventory>, IDropHandler
{
    [SerializeField]private Button removeButton;
    [SerializeField]private Image removeButtonIcon;
    public InventoryScript inventoryScript;
    
    public GameObject item
    {
        get
        {
            if(transform.childCount > 1)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void Awake()
    {
        removeButtonIcon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Ajout de l'icon dans le slot si vide
        if(!item)
        {
             
            ObjectScript objToAdd = ScriptableObject.CreateInstance<ObjectScript>();
            DragIconInventory.item.transform.SetParent(transform);
            removeButton.interactable = true;
            removeButtonIcon.enabled = true;
            if (DragIconInventory.itemType == DragIconInventory._typeOfItem.Consumable)
            {
                
                objToAdd.itemType = ObjectScript._typeOfItem.Consumable;
                objToAdd.objectName = DragIconInventory.name;
            }
            else if (DragIconInventory.itemType == DragIconInventory._typeOfItem.Bonus)
            {
                objToAdd.itemType = ObjectScript._typeOfItem.Bonus;
                objToAdd.objectName = DragIconInventory.name;
            }
            else
            {
                objToAdd.itemType = ObjectScript._typeOfItem.Skill;
                objToAdd.objectName = DragIconInventory.name;              
            }
        }
    }

    public void ClearSlot()
    {
        if(item)
        {
            removeButton.enabled = false;
            removeButton.interactable = false;
        }
    }

    private void Update()
    {
        if(item)
        {
            removeButton.enabled = true;
            removeButton.interactable = true;
        }
        transform.GetChild(0);
    }

    public void OnRemoveButton()
    {
        ClearSlot();
    }

    public void OnUsedItem()
    {
        
    }
}

