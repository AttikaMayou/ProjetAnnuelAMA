using System.Collections.Generic;
using Assets.LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Singleton;
using Photon.Pun;
using UnityEngine;

//Auteur : Abdallah
//Modification : Attika

namespace Assets.LastToTheGlobe.Scripts.Management.OLD
{
    public class PlayerColliderDirectoryScript : MonoBehaviourSingleton<PlayerColliderDirectoryScript>
    {
        //TODO : Add methods to remove a CharacterExposer from the list and the directory dictionary
        
        [SerializeField]
        public List<CharacterExposerScript> characterExposers;
    
        private readonly Dictionary<Collider, CharacterExposerScript> _directory = new Dictionary<Collider, CharacterExposerScript>();
        private CharacterExposerScript _value;
        
        /// Get the player whom belongs to the collider
        public CharacterExposerScript GetExposer(Collider col)
        {
            Debug.Log("trying to find the player from this collider : " + col);
            if (!PhotonNetwork.IsMasterClient) return null;
            Debug.Log("I am the master");
            if (_directory.TryGetValue(col, out _value))
            {
                return _value;
            }
            Debug.LogError("The Collider has no CharacterExposerScript associated : " +
                           "something wrong happened at instantiation of the player whom it belongs to this gameObject : " +
                           col.gameObject.name);
            return null;
        }

        /// Add player in the list of CharactersExposers
        public void AddExposer(CharacterExposerScript player)
        {
            if (characterExposers == null)
            {
                characterExposers = new List<CharacterExposerScript>();
            }
            
            if (!characterExposers.Contains(player) && player)
            {
                characterExposers.Add(player);
            }
            
            AddPlayerInDirectory(player);
        }

        private void AddPlayerInDirectory(CharacterExposerScript player)
        {
            Debug.Log("add one player to directory");
            if (_directory.ContainsValue(player))
            {
                Debug.Log("directory already contains player");
                return;
            }
            _directory.Add(player.CharacterCollider, player);
            Debug.Log("Directory key : " + player.CharacterCollider + " and value : " + player);
        }
        
    }
}
