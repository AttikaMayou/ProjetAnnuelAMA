using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LastToTheGlobe.Scripts.Inventory
{
    public class UIChest : MonoBehaviour
    {
        private bool openChest = false;
        private bool canOpenChest = false;
        [SerializeField] private Image pressE;
        [SerializeField] private Canvas playerInventory;
        [SerializeField] private Canvas chestInventory;

        void Start()
        {
            playerInventory.enabled = false;
            chestInventory.enabled = false;
        }

        void Update()
        {
            if(canOpenChest)
            {
                pressE.enabled = true;
            }
            else
            {
                pressE.enabled = false;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if(canOpenChest)
                {
                    playerInventory.enabled = true;
                    chestInventory.enabled = true;
                    canOpenChest = false;
                    pressE.enabled = false;
                    openChest = true;
                }
                else if(!canOpenChest)
                {
                    playerInventory.enabled = false;
                    chestInventory.enabled = false;
                    pressE.enabled = true;
                    canOpenChest = true;
                    openChest = false;
                }
            }
        }


    void OnTriggerEnter(Collider chest)
        {
            if (chest.CompareTag("Player"))
            {
                //Affiche touche E
                canOpenChest = true;
            }
        }

        void OnTriggerExit(Collider chest)
        {
            if (chest.CompareTag("Player"))
            {
                //Ferme touche E
                canOpenChest = false;
            }
        }
    }
}
