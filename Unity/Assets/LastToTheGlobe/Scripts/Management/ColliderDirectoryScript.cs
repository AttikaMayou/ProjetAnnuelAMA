using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Singleton;
using UnityEngine;
using System.Collections.Generic;
using LastToTheGlobe.Scripts.Environment.Planets;
using Photon.Pun;
using UnityEngine.Serialization;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Management
{
    public class ColliderDirectoryScript : MonoBehaviourSingleton<ColliderDirectoryScript>
    {
        public bool debug = true;
        public List<CharacterExposerScript> characterExposers;
        public List<PlanetExposerScript> planetExposers;
        
        private Dictionary<Collider, CharacterExposerScript> _playersDirectory = new Dictionary<Collider, CharacterExposerScript>();
        private CharacterExposerScript _playerValue;
        
        private Dictionary<Collider, PlanetExposerScript> _planetsDirectory = new Dictionary<Collider, PlanetExposerScript>();
        private PlanetExposerScript _planetValue;

        #region Players Methods
        //Get the player whom belongs to the collider
        public CharacterExposerScript GetCharacterExposer(Collider col)
        {
            if(debug) Debug.Log("trying to find player from this collider : " + col);
            if (!PhotonNetwork.IsMasterClient) return null;
            return _playersDirectory.TryGetValue(col, out _playerValue) ? _playerValue : null;
        }

        public void AddCharacterExposer(CharacterExposerScript player)
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
            if(debug) Debug.Log("add one player to directory");
            if (_playersDirectory.ContainsValue(player)) return;
            _playersDirectory.Add(player.characterCollider, player);
            if(debug) Debug.Log("Directory key : " + player.characterCollider + " and value : " + player);
        }
        #endregion
        
        #region Planets Methods

        public PlanetExposerScript GetPlanetExposerScript(Collider col)
        {
            if(debug) Debug.Log("trying to find planet from this collider : " + col);
            if (!PhotonNetwork.IsMasterClient) return null;
            return _planetsDirectory.TryGetValue(col, out _planetValue) ? _planetValue : null;
        }

        public void AddPlanetExposer(PlanetExposerScript planet)
        {
            if (planetExposers == null)
            {
                planetExposers = new List<PlanetExposerScript>();
            }

            if (!planetExposers.Contains(planet) && planet)
            {
                planetExposers.Add(planet);
            }

            AddPlanetInDirectory(planet);
        }

        private void AddPlanetInDirectory(PlanetExposerScript planet)
        {
            if(debug) Debug.Log("add one planet to directory");
            if (_planetsDirectory.ContainsValue(planet)) return;
            _planetsDirectory.Add(planet.planetCollider, planet);
            if(debug) Debug.Log("Directory key : " + planet.planetCollider + " and value : " + planet);
        }
        
        #endregion

    }
}
