using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] public Collider selfCollider;
        [SerializeField] public ColliderDirectoryScript colliderDirectoryScript;
        [HideInInspector] public bool playerOpenChest = false;
        private List<string> _aroundChest = new List<string>();
        
        void Start()
        {
            openChest = false;
            canOpenChest = false;
        }

        void Update()
        {
            foreach (var var in _aroundChest)
            {
                Debug.Log(var);
            }
            if (_aroundChest.Contains("Player"))
            {
                Debug.Log("There is a player !");
            }

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

        private void OnTriggerEnter(Collider other)
        {
            
            Debug.Log(other.gameObject.tag);
            if (other.gameObject.CompareTag("Player"))
            {
                _aroundChest.Add(other.gameObject.tag);
                CharacterExposerScript characterExposerScript = colliderDirectoryScript.GetCharacterExposer(other);
                characterExposerScript.Interaction.SetActive(true);
                characterExposerScript.avatarsController._canOpenChest = true;
                //characterExposerScript.avatarsController.PlayerInventory.Activation();
                //characterExposerScript.avatarsController.Interaction.Activation();
                MeshRenderer meshRend = GetComponent<MeshRenderer>();
                meshRend.material.color = Color.green;
                //Affiche touche E
                canOpenChest = true;
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            
            if (other.gameObject.CompareTag("Player"))
            {
                CharacterExposerScript characterExposerScript = colliderDirectoryScript.GetCharacterExposer(other);
                characterExposerScript.Interaction.SetActive(false);
                characterExposerScript.avatarsController._canOpenChest = false;
                MeshRenderer meshRend = GetComponent<MeshRenderer>();
                meshRend.material.color = Color.magenta;
                canOpenChest = false;

                
            }
        }
    }
}
