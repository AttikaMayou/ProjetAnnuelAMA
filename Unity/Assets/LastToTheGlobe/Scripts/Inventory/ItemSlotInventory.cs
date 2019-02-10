using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Auteur : Margot

public class ItemSlotInventory : MonoBehaviour, IDropHandler
{
    public GameObject item
    {
        //properties
        get
        {
            if(transform.childCount>0)
            {
                return transform.GetChild(0).gameObject;
            }

            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(!item)
        {
            DragIconInventory.item.transform.SetParent(transform);
        }


    }
}
