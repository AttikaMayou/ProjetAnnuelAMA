using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Canvas playerInventory;
    [SerializeField] private Canvas tutorial;

    void Start()
    {
        playerInventory.enabled = false;
        tutorial.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            playerInventory.enabled = true;
        }

        if (Input.GetKey(KeyCode.F1))
        {
            tutorial.enabled = true;
        }
        else
        {
            tutorial.enabled = false;
        }
    }
}
