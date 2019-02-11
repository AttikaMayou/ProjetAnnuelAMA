using LastToTheGlobe.Scripts.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Auteur : Margot

public class ItemSlotInventory : MonoBehaviourSingleton<ItemSlotInventory>, IDropHandler
{
    [SerializeField]
    private Button removeButton;
    [SerializeField]
    private Image removeButtonIcon;

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
            DragIconInventory.item.transform.SetParent(transform);
            removeButton.interactable = true;
            removeButtonIcon.enabled = true;
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

    public void OnRemoveButton()
    {
        Debug.Log("Press Remove Button");
        //DragIconInventory._instance.Remove(item);
    }
}

