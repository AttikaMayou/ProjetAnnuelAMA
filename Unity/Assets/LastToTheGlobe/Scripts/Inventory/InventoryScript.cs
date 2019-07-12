using System.Collections.Generic;
using System.Linq;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

//Auteur : Attika
//Modification : Abdallah

namespace LastToTheGlobe.Scripts.Inventory
{
    public class InventoryScript : MonoBehaviour
    {
        [SerializeField] private int nbSlots = 4;
        public bool isFull;
        public List<ObjectScript> objectsInInventory = new List<ObjectScript>();
        public List<string> objectsName = new List<string>();

        private void Awake()
        {
            InitializeInventory();
        }
        
        private void InitializeInventory()
        {
            //Add here if you want to put something in inventory at beginning of game 
            //like this : objectsInInventory.Add(objectToAdd);
        }
        
        /// <summary>
        /// Add an object in itself inventory
        /// </summary>
        /// <param name="obj"></param>
        public void AddObjectInInventory(ObjectScript obj)
        {
            
            
            if (!objectsInInventory.Contains(obj) && !IsInventoryFull())
            {
                Debug.LogFormat("Objet ajouté à l'inventaire : {0}", obj.objectName);
                objectsName.Append(obj.objectName);
                objectsInInventory.Add(obj);
                obj.SetObjectInInventory(true);
                SetInventoryStatus();
            }
        }

        private void DeleteObjectFromInventory(ObjectScript obj)
        {
            if (!objectsInInventory.Contains(obj)) return;
            objectsInInventory.Remove(obj);
            SetInventoryStatus();
        }
        
        /// <summary>
        /// Remove all objects from itself inventory
        /// </summary>
        private void EraseInventoryContent()
        {
            if (!isFull || objectsInInventory.Count <= 0) return;
            foreach (var obj in objectsInInventory)
            {
                obj.SetObjectInInventory(false);
                //TODO : add action in ObjectScript --> set object free
            }
            objectsInInventory.Clear();
            SetInventoryStatus();
        }
        
        /// <summary>
        /// Set the inventory status to full or not
        /// </summary>
        /// <returns></returns>
        private void SetInventoryStatus()
        {
            isFull = objectsInInventory.Count >= nbSlots;
        }
        
        /// <summary>
        /// Return true if inventory is full, false if not
        /// </summary>
        /// <returns></returns>
        public bool IsInventoryFull()
        {
            return isFull;
        }
    }
}
