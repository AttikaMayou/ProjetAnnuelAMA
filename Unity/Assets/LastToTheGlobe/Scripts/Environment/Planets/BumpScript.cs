using Assets.LastToTheGlobe.Scripts.Management;
using Photon.Pun;
using UnityEngine;

namespace Assets.LastToTheGlobe.Scripts.Environment.Planets
{
    public class BumpScript : MonoBehaviour
    {
        public static bool debug = true;

        public BumperExposerScript Exposer;

        [SerializeField] private PhotonView photonView;

//        public void BumpPlayer(int playerId)
//        {
//            
//        }
        
        #region Collision Methods

        private void OnTriggerEnter(Collider other)
        {
            if(debug) Debug.LogFormat("[BumpScript] {0} get triggered by something : {1}",
                this.gameObject.name, other.gameObject.name);
            
            //Only the Master Client interact with collider and stuff like this
            if (!PhotonNetwork.IsMasterClient) return;

            var playerId = ColliderDirectoryScript.Instance.GetPlayerId(other);

            //if playerId is different from -1, that means this is a player which hit the bumper
            if (playerId != -1)
            {
                //Send to MasterClient a message to warn him with its own ID and playerId
                photonView.RPC("BumpPlayerRPC", RpcTarget.MasterClient,
                    Exposer.Id, playerId);
            }
        }

        #endregion
        
        #region RPC Callbacks

        [PunRPC]
        void BumpPlayerRPC(int bumperId, int playerId)
        {
            
        }
        
        #endregion
    }
}
