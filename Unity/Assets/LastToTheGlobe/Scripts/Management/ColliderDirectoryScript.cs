using System.Collections.Generic;
using Assets.LastToTheGlobe.Scripts.Avatar;
using Assets.LastToTheGlobe.Scripts.Environment.Planets;
using Assets.LastToTheGlobe.Scripts.Weapon.Orb;
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
        public bool IsInitialized = false;
        
        public List<CharacterExposerScript> CharacterExposers;
        public List<PlanetExposerScript> PlanetExposers;
        public List<OrbManager> OrbManagers;
        
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
            if(debug) Debug.Log("Directory status : " + IsInitialized);
            if(debug) Debug.Log("trying to find player from this collider : " + col);
            if (!PhotonNetwork.IsMasterClient) return null;
            return _playersDirectory.TryGetValue(col, out _playerValue) ? _playerValue : null;
        }

        public void AddCharacterExposer(CharacterExposerScript player, out int id)
        {
            if (!IsInitialized) IsInitialized = true;
            if (CharacterExposers == null)
            {
                CharacterExposers = new List<CharacterExposerScript>();
            }

            if (!CharacterExposers.Contains(player) && player)
            {
                CharacterExposers.Add(player);
            }
            
            id = AddPlayerInDirectory(player);
        }

        public void RemoveCharacterExposer(CharacterExposerScript player)
        {
            if (CharacterExposers.Contains(player) && player)
            {
                CharacterExposers.Remove(player);
            }
        }

        private int AddPlayerInDirectory(CharacterExposerScript player)
        {
            var id = 0;
            if(debug) Debug.Log("add one player to directory");
            if (_playersDirectory.ContainsValue(player)) return id;
            _playersDirectory.Add(player.CharacterCollider, player);
            if(debug) Debug.LogFormat("Directory key : {0} and value : {1}", player.CharacterCollider, player);
            return id;
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
            if (PlanetExposers == null)
            {
                PlanetExposers = new List<PlanetExposerScript>();
            }

            if (!PlanetExposers.Contains(planet) && planet)
            {
                PlanetExposers.Add(planet);
            }

            AddPlanetInDirectory(planet);
        }

        private void AddPlanetInDirectory(PlanetExposerScript planet)
        {
            if(debug) Debug.Log("add one planet to directory");
            if (_planetsDirectory.ContainsValue(planet)) return;
            _planetsDirectory.Add(planet.PlanetCollider, planet);
            if(debug) Debug.LogFormat("Directory key : {0} and value : {1}", planet.PlanetCollider, planet);
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
            if (OrbManagers == null)
            {
                OrbManagers = new List<OrbManager>();
            }

            if (!OrbManagers.Contains(orb) && orb)
            {
                OrbManagers.Add(orb);
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
