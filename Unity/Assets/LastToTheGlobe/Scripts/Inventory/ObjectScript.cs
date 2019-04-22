using UnityEngine;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Inventory
{
    public class ObjectScript : ScriptableObject
    {
        public bool isInInventory;
        public bool isConsume;
        public int nbOfConsumption = 0;
        public string objectName;
        /*
         Consommable = 0
         Bonus = 1
         Skills = 2
         */
        public int typeOfObject = 2;

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
