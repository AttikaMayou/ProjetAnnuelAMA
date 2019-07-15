using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

namespace LastToTheGlobe.Scripts.Chest
{
    public class ChestExposerScript : MonoBehaviour
    {
        //The id value of this chest. Updated at awakening
        public int Id;
        public int seedChest;
        
        public ChestScript ChestScript;
        
        public Collider ChestCollider;
        public PhotonView ChestPhotonView;
        public ChestContentManagerScript chestContentManagerScript;
        
        private void Awake()
        {
            
            if (!PhotonNetwork.IsMasterClient) return;
            print(this + "____________" + Id);
            ColliderDirectoryScript.Instance.AddChestExposer(this, out Id);
        }


        private void OnDestroy()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.RemoveChestExposer(this);
        }
    }
}