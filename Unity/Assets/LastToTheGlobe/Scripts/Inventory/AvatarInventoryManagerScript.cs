using LastToTheGlobe.Scripts.Avatar;
using Photon.Pun;
using UnityEngine;

//Auteur : Abdallah

namespace LastToTheGlobe.Scripts.Inventory
{
    public class AvatarInventoryManagerScript : MonoBehaviour
    {
        public CharacterExposerScript selfExposer;

        [SerializeField] private PhotonView inventoryPhotonView;

        private ObjectScript _objectToAdd;


        private void Update()
        {
            //Check if an item have been added
            foreach (var item in selfExposer.InventoryExposer.playerSlot)
            {
                if (item.transform.childCount > 2 && !item.transform.GetChild(2).CompareTag("Untagged"))
                {
                    item.transform.GetChild(2).SetAsFirstSibling();
                    //Send RPC to add item
                
                    inventoryPhotonView.RPC("AddItemToInventory", RpcTarget.MasterClient, item.transform.GetChild(0).tag, selfExposer.Id);
                
                }
            }
        }
    }
}
