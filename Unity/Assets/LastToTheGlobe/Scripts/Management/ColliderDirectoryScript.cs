using System.Collections.Generic;
using Assets.LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Environment.Planets;
using LastToTheGlobe.Scripts.Singleton;
using LastToTheGlobe.Scripts.Weapon.Orb;
using Photon.Pun;
using UnityEngine;

//Auteur : Attika

namespace Assets.LastToTheGlobe.Scripts.Management
{
    public class ColliderDirectoryScript : MonoBehaviourSingleton<ColliderDirectoryScript>
    {
        public bool debug = true;
        public List<CharacterExposerScript> characterExposers;
        public List<PlanetExposerScript> planetExposers;
        public List<OrbManager> orbManagers;
        
        private Dictionary<Collider, CharacterExposerScript> _playersDirectory = new Dictionary<Collider, CharacterExposerScript>();
        private CharacterExposerScript _playerValue;
        
        private Dictionary<Collider, PlanetExposerScript> _planetsDirectory = new Dictionary<Collider, PlanetExposerScript>();
        private PlanetExposerScript _planetValue;

        private Dictionary<Collider, OrbManager> _orbsDirectory = new Dictionary<Collider, OrbManager>();
        private OrbManager _orbValue;
        
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
            if(debug) Debug.LogFormat("Directory key : {0} and value : {1}", player.characterCollider, player);
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
            if(debug) Debug.LogFormat("Directory key : {0} and value : {1}", planet.planetCollider, planet);
        }
        
        #endregion
        
        #region Orbs Methods

        public OrbManager GetOrbManager(Collider col)
        {
            if(debug) Debug.Log("trying to find orb from this collider : " + col);
            if (!PhotonNetwork.IsMasterClient) return null;
            return _orbsDirectory.TryGetValue(col, out _orbValue) ? _orbValue : null;
        }
        
        public void AddOrbManager(OrbManager orb)
        {
            if (orbManagers == null)
            {
                orbManagers = new List<OrbManager>();
            }

            if (!orbManagers.Contains(orb) && orb)
            {
                orbManagers.Add(orb);
            }
            
            AddOrbInDirectory(orb);
        }

        private void AddOrbInDirectory(OrbManager orb)
        {
            if(debug) Debug.Log("add one orb to directory");
            if (_orbsDirectory.ContainsValue(orb)) return;
            _orbsDirectory.Add(orb.orbCd, orb);
            if(debug) Debug.LogFormat("Directory key : {0} and value : {1}", orb.orbCd, orb);
        }
        
        #endregion

    }
}
