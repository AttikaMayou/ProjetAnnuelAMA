using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//Auteur : Margot 

public class DragIconInventory : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public static GameObject item;
    Vector3 startPosition;
    Transform startParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        item = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        item = null;

        if(transform.parent != startParent)
        {
            transform.position = startPosition;
        }

    }
}
