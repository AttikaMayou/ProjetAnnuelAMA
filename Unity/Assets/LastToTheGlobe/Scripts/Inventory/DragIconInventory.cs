using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


//Auteur : Margot 
//Modification : Abdallah

public class DragIconInventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform rect;
    public static GameObject item;
    public static string name;
    public enum _typeOfItem
    {
        Consumable,
        Bonus,
        Skill
    };

    public static _typeOfItem itemType = _typeOfItem.Consumable;
    public static float lifePoint;
    private Vector3 startPosition = Vector3.zero;
    private CanvasGroup canvasGroup;
    private Transform parentToReturnTo;
    private Transform canvas;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        rect.localScale = new Vector3(1,1,1);
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
