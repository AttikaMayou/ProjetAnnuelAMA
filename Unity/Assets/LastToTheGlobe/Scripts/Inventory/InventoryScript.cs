using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using UnityEngine;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Inventory
{
    public class InventoryScript : MonoBehaviour
    {
        public int nbSlots = 5;
        public bool isFull;
        public List<ObjectScript> objectsInInventory;


        /// <summary>
        /// Add an object in itself inventory
        /// </summary>
        /// <param name="obj"></param>
        public void AddObjectInInventory(ObjectScript obj)
        {
            if(objectsInInventory.Contains(obj)) return;
            objectsInInventory.Add(obj);
            obj.SetObjectInInventory(true);
            //TODO : add action in ObjectScript --> set object in inventory
        }
        
        /// <summary>
        /// Remove all objects from itself inventory
        /// </summary>
        public void EraseInventoryContent()
        {
            if (!isFull || objectsInInventory.Count <= 0) return;
            foreach (var obj in objectsInInventory)
            {
                obj.SetObjectInInventory(false);
                //TODO : add action in ObjectScript --> set object free
            }
            objectsInInventory.Clear();
        }
        
        /// <summary>
        /// Set the inventory status to full or not
        /// </summary>
        /// <returns></returns>
        public void SetInventoryStatus()
        {
            isFull = objectsInInventory.Count > 0;
           
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
