using System.Collections.Generic;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Dev;
using LastToTheGlobe.Scripts.Singleton;
using Photon.Pun;
using UnityEngine;

//Auteur : Abdallah
//Modification : Attika

namespace LastToTheGlobe.Scripts.Management
{
    public class PlayerColliderDirectoryScript : MonoBehaviourSingleton<PlayerColliderDirectoryScript>
    {
        //TODO : Add methods to remove a CharacterExposer from the list and the directory dictionary
        
        [SerializeField]
        public List<CharacterExposerScript> characterExposers;
    
        private readonly Dictionary<Collider, CharacterExposerScript> _directory = new Dictionary<Collider, CharacterExposerScript>();
        private CharacterExposerScript _value;
        
        /// <summary>
        /// Get the player whom belongs to the collider
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public CharacterExposerScript GetExposer(Collider col)
        {
            if (!PhotonNetwork.IsMasterClient) return null;
            if (_directory.TryGetValue(col, out _value))
            {
                return _value;
            }
            Debug.LogError("The Collider has no CharacterExposer associated : " +
                           "something wrong happened at instantiation of the player whom it belongs to this gameObject : " +
                           col.gameObject.name);
            return null;
        }

        /// <summary>
        /// Add player in the list of CharactersExposers
        /// </summary>
        /// <param name="player"></param>
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

        public void SyncData(CharacterExposerScript exposer)
        {
            AddExposer(exposer);
        }

        private void AddPlayerInDirectory(CharacterExposerScript player)
        {
            Debug.Log("add one player to directory");
            if (_directory.ContainsValue(player)) return;
            _directory.Add(player.characterCollider, player);
        }
        
    }
}
