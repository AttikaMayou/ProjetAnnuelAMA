using System.Collections.Generic;
using UnityEngine;

//Auteur : Attika
//Modification : Abdallah

namespace LastToTheGlobe.Scripts.Inventory
{
    public class ObjectScript : UnityEngine.ScriptableObject
    {
        public bool isInInventory;
        public bool isConsume;
        public string objectName;
        public enum _typeOfItem
        {
            Consumable,
            Bonus,
            Skill
        };

        public _typeOfItem itemType = _typeOfItem.Consumable;

        /// <summary>
        /// Set an object in inventory or delete it from inventory
        /// </summary>
        /// <param name="state"></param>
        public void SetObjectInInventory(bool state)
        {
            //TODO : Refacto this function --> inventory will handle this kind of action so players cannot cheat
            isInInventory = state;
        }

        /// <summary>
        /// Return true if object is in inventory, false if not
        /// </summary>
        /// <returns></returns>
        public bool IsObjectInInventory()
        {
            return isInInventory;
        }
        
        /// <summary>
        /// Set the status of an object to consumed or not
        /// </summary>
        /// <param name="state"></param>
        public void SetConsumeStatus(bool state)
        {
            //TODO : Refacto this function --> inventory will handle this kind of action so players cannot cheat
            isConsume = state;
        }
        
        /// <summary>
        /// Return true if object is already consume, false if not
        /// </summary>
        /// <returns></returns>
        public bool IsObjectConsume()
        {
            return isConsume;
        }
    }
}
