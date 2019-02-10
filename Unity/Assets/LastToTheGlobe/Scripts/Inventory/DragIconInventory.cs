using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//Auteur : Margot 

public class DragIconInventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject item;
    private Vector3 startPosition = Vector3.zero;
    private Transform startParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        item = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        //GetComponent<CanvasGroup>().blocksRaycasts = false;
        //transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        item = null;
        //GetComponent<CanvasGroup>().blocksRaycasts = true;
        //if(transform.parent == startParent || transform.parent == transform.root)
        //{
            transform.position = startPosition;
            //transform.SetParent(startParent);
        //}


    }
}
