using System.Collections;
using System.Collections.Generic;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Management;
using UnityEngine;
using UnityEngine.UI;

//Auteur : Margot 
//Modification : Abdallah

namespace LastToTheGlobe.Scripts.Inventory
{
    public class UIChest : MonoBehaviour
    {
        private bool openChest = false;
        private bool canOpenChest = false;
        [SerializeField] public ColliderDirectoryScript colliderDirectoryScript;
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                CharacterExposerScript characterExposerScript = colliderDirectoryScript.GetCharacterExposer(other);
                //characterExposerScript.characterRootGameObject.
                MeshRenderer meshRend = GetComponent<MeshRenderer>();
                meshRend.material.color = Color.green;
                pressE.gameObject.SetActive(true);
                playerInventory.gameObject.SetActive(true);
                chestInventory.gameObject.SetActive(true);
                //Affiche touche E
                canOpenChest = true;
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                MeshRenderer meshRend = GetComponent<MeshRenderer>();
                meshRend.material.color = Color.magenta;
                canOpenChest = false;
                pressE.gameObject.SetActive(false);
                chestInventory.gameObject.SetActive(false);
                
            }
        }
    }
}
