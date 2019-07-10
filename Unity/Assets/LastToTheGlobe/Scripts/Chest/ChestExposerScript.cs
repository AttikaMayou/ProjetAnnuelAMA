using LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

namespace LastToTheGlobe.Scripts.Chest
{
    public class ChestExposerScript : MonoBehaviour
    {
        //The id value of this chest. Updated at awakening
        public int Id;

        public ChestScript ChestScript;
        
        public Collider ChestCollider;
        public PhotonView ChestPhotonView;
        
        private void Awake()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            //ICI OU S'EST ARRETER LE TAFF TU DEVAIS VOIR POURQUOI LE CHEST SE METTAIS PAS DANS LE COLLIDER ETC ETC ETC ALLER SALUT !
            Debug.Log("Hi !");
            ColliderDirectoryScript.Instance.AddChestExposer(this, out Id);
        }

        private void OnDestroy()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            ColliderDirectoryScript.Instance.RemoveChestExposer(this);
        }
    }
}
