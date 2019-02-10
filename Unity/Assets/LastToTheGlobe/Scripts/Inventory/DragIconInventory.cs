using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//Auteur : Margot 

public class DragIconInventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject item;
    private Vector3 startPosition = Vector3.zero;
    private CanvasGroup canvasGroup;
    private Transform parentToReturnTo;
    private Transform canvas;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        item = gameObject;
        startPosition = transform.position;
        parentToReturnTo = transform.parent;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(transform.root);

        canvas = GameObject.FindGameObjectWithTag("UI Canvas").transform;
        transform.SetParent(canvas);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        item = null;
        canvasGroup.blocksRaycasts = true;
        if(transform.parent == canvas)
        {
            transform.position = startPosition;
            transform.SetParent(parentToReturnTo);
        }

    }
}
