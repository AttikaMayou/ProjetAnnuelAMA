using UnityEngine;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Inventory
{
    public class InventoryController : InventoryScript
    {
        [SerializeField] private KeyCode interactionKey;
        private ObjectScript curr;
        
        
        private void Update()
        {
            if (Input.GetKey(interactionKey))
            {
                //Add here the method that get the object to add
                if (curr)
                {
                    AddObjectInInventory(curr);
                }
            }
        }


        private void RaycastToObj()
        {
            
        }
    }
}
