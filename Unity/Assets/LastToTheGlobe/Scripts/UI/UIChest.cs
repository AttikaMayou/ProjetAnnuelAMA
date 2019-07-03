using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Auteur : Margot 
//Modification : Abdallah

namespace LastToTheGlobe.Scripts.Inventory
{
    public class UIChest : MonoBehaviour
    {
        /*private bool openChest = false;
        private bool canOpenChest = false;
        [SerializeField] public Canvas pressE;
        [SerializeField] public Canvas playerInventory;
        [SerializeField] public Canvas chestInventory;
        [HideInInspector] public bool playerOpenChest = false;
        
        void Start()
        {
            pressE.gameObject.SetActive(false);
            playerInventory.gameObject.SetActive(false);
            chestInventory.gameObject.SetActive(false);
            openChest = false;
            canOpenChest = false;
        }

        void Update()
        {

            if (playerOpenChest)
            {
                if(canOpenChest)
                {
                    print("Hello");
                    
                    canOpenChest = false;
                    openChest = true;
                }
                else if(!canOpenChest)
                {
                    canOpenChest = true;
                    openChest = false;
                }
            }
        }

        public void OnResponseCloseToChest()
        {
            
        }
        
        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.SendMessage("CloseToChest", this);
                //MeshRenderer meshRend = GetComponent<MeshRenderer>();
                //meshRend.material.color = Color.green;
                //Affiche touche E
                canOpenChest = true;
                
            }
                
        }
        
        void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.SendMessage("AwayFromChest", this);
                //MeshRenderer meshRend = GetComponent<MeshRenderer>();
                //meshRend.material.color = Color.magenta;
                canOpenChest = false;
                pressE.gameObject.SetActive(false);
                
            }
            
        }*/
    }
}
