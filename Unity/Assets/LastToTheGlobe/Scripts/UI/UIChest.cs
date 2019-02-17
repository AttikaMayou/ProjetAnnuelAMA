using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LastToTheGlobe.Scripts.Inventory
{
    public class UIChest : MonoBehaviour
    {
        private bool OpenChest = false;
        private bool CanOpenChest = false;
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
            if(CanOpenChest)
            {
                pressE.enabled = true;
            }
            else
            {
                pressE.enabled = false;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if(CanOpenChest)
                {
                    CanOpenChest = false;
                    pressE.enabled = false;
                    playerInventory.enabled = true;
                    chestInventory.enabled = true;
                    OpenChest = true;
                }
                else
                {
                    playerInventory.enabled = false;
                    chestInventory.enabled = false;
                }
            }
        }



        void OnTriggerEnter(Collider chest)
        {
            if (chest.CompareTag("Player"))
            {
                //Affiche touche E
                CanOpenChest = true;
            }
        }

        void OnTriggerExit(Collider chest)
        {
            if (chest.CompareTag("Player"))
            {
                //Ferme touche E
                CanOpenChest = false;
            }
        }
    }
}
