using LastToTheGlobe.Scripts.Avatar;
using Photon.Pun;
using UnityEngine;

//Auteur : Margot
//Modification : Attika

namespace LastToTheGlobe.Scripts.Dev
{
    public class AvatarsController : MonoBehaviour
    {
        [Header("Photon and Replication Parameters")]
        //[SerializeField] private CharacterExposer[] players;
        [SerializeField] private AIntentReceiver[] onlineIntentReceivers;
        //[SerializeField] private AIntentReceiver[] offlineIntentReceivers;
        [SerializeField] private SceneMenuController startGameController;
        [SerializeField] private PhotonView photonView;
        
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            //startGameController.OnlinePlayReady +=
        }
        
        #endregion
    }
}
