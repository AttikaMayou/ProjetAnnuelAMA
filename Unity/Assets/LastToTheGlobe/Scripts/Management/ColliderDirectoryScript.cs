using System.Collections;
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
        
        public List<CharacterExposerScript> CharacterExposers;
        [SerializeField] private int _activePlayers = 0;
        public List<PlanetExposerScript> PlanetExposers;
        [SerializeField] private int _activePlanets = 0;
        public List<OrbManager> OrbManagers;
        [SerializeField] private int _activeOrbs = 0;
        
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
            if(debug) Debug.Log("[ColliderDirectoryScript] Trying to find player from " +
                                "this collider : " + col);
            if (!PhotonNetwork.IsMasterClient) return null;
            return _playersDirectory.TryGetValue(col, out _playerValue) ? _playerValue : null;
        }

        public CharacterExposerScript GetCharacterExposer(int id)
        {
            if(debug) Debug.Log("[ColliderDirectoryScript] Trying to find player from " +
                                "this id: " + id);
            if (id < 0 || id >= CharacterExposers.Count) return null;
            return !PhotonNetwork.IsMasterClient ? null : CharacterExposers[id];
        }

        public int GetPlayerId(Collider col)
        {
            if (!col) return -1;
            if (CharacterExposers.Count == 0) StartCoroutine(Wait());
            var player = GetCharacterExposer(col);
            if (player) return player.Id;
            Debug.LogWarningFormat("[ColliderDirectoryScript] No CharacterExposer found with this collider {0}", 
                col.name);
            return -1;
        }

        public void AddCharacterExposer(CharacterExposerScript player, out int id)
        {
            if (CharacterExposers == null)
            {
                CharacterExposers = new List<CharacterExposerScript>();
            }

            if (!CharacterExposers.Contains(player) && player)
            {
                CharacterExposers.Add(player);
            }

            _activePlayers++;
            
            id = AddPlayerInDirectory(player);
            
            if (debug)
            {
                Debug.Log(id >= 0
                    ? "[ColliderDirectoryScript] Successful added player to directory"
                    : "[ColliderDirectoryScript] Failed to add player in directory");
            }
        }

        public void RemoveCharacterExposer(CharacterExposerScript player)
        {
            _activePlayers--;
            player.Id = -1;
            if (CharacterExposers.Contains(player) && player)
            {
                CharacterExposers.Remove(player);
            }
        }

        private int AddPlayerInDirectory(CharacterExposerScript player)
        {
            var id = -1;
            if(debug) Debug.Log("[ColliderDirectoryScript] Add one player to directory");
            if (_playersDirectory.ContainsValue(player)) return id;
            _playersDirectory.Add(player.CharacterCollider, player);
            id = _activePlayers - 1;
            if(debug) Debug.LogFormat("[ColliderDirectoryScript] Directory key : {0} and value : {1}", 
                player.CharacterCollider, player);
            return id;
        }
        
        #endregion
        
        #region Planets Methods

        public PlanetExposerScript GetPlanetExposer(Collider col)
        {
            if(debug) Debug.Log("[ColliderDirectoryScript] Trying to find planet from " +
                                "this collider : " + col);
            if (!PhotonNetwork.IsMasterClient) return null;
            return _planetsDirectory.TryGetValue(col, out _planetValue) ? _planetValue : null;
        }
        
        public PlanetExposerScript GetPlanetExposer(int id)
        {
            if(debug) Debug.Log("[ColliderDirectoryScript] Trying to find planet from " +
                                "this id: " + id);
            if (id < 0 || id >= PlanetExposers.Count) return null;
            return !PhotonNetwork.IsMasterClient ? null : PlanetExposers[id];
        }
        
        public int GetPlanetId(Collider col)
        {
            if (!col) return -1;
            var planet = GetPlanetExposer(col);
            if (planet) return planet.Id;
            Debug.LogWarningFormat("[ColliderDirectoryScript] No PlanetExposer found with this collider {0}",
                col.name);
            return -1;
        }

        public void AddPlanetExposer(PlanetExposerScript planet, out int id)
        {
            if (PlanetExposers == null)
            {
                PlanetExposers = new List<PlanetExposerScript>();
            }

            if (!PlanetExposers.Contains(planet) && planet)
            {
                PlanetExposers.Add(planet);
            }

            _activePlanets++;
            
            id = AddPlanetInDirectory(planet);
            
            if (debug)
            {
                Debug.Log(id >= 0
                    ? "[ColliderDirectoryScript] Successful added planet to directory"
                    : "[ColliderDirectoryScript] Failed to add planet in directory");
            }
        }

        public void RemovePlanetExposer(PlanetExposerScript planet)
        {
            _activePlanets--;
            planet.Id = -1;
            if (PlanetExposers.Contains(planet) && planet)
            {
                PlanetExposers.Remove(planet);
            }
        }

        private int AddPlanetInDirectory(PlanetExposerScript planet)
        {
            var id = -1;
            if(debug) Debug.Log("[ColliderDirectoryScript] Add one planet to directory");
            if (_planetsDirectory.ContainsValue(planet)) return id;
            _planetsDirectory.Add(planet.PlanetCollider, planet);
            id = _activePlanets -1;
            if(debug) Debug.LogFormat("[ColliderDirectoryScript] Directory key : {0} and value : {1}", 
                planet.PlanetCollider, planet);
            return id;
        }
        
        #endregion
        
        #region Orbs Methods

        public OrbManager GetOrbManager(Collider col)
        {
            if(debug) Debug.Log("[ColliderDirectoryScript] Trying to find orb from this collider : " + col);
            if (!PhotonNetwork.IsMasterClient) return null;
            return _orbsDirectory.TryGetValue(col, out _orbValue) ? _orbValue : null;
        }
        
        public OrbManager GetOrbManager(int id)
        {
            if(debug) Debug.Log("[ColliderDirectoryScript] Trying to find orb from this id: " + id);
            if (id < 0 || id >= OrbManagers.Count) return null;
            return !PhotonNetwork.IsMasterClient ? null : OrbManagers[id];
        }

        public int GetOrbId(Collider col)
        {
            if (!col) return -1;
            var orb = GetOrbManager(col);
            if (orb) return orb.Id;
            Debug.LogWarningFormat("[ColliderDirectoryScript] No OrbManager found with this collider {0}", 
                col.name);
            return -1;
        }
        
        public void AddOrbManager(OrbManager orb, out int id)
        {
            if (OrbManagers == null)
            {
                OrbManagers = new List<OrbManager>();
            }

            if (!OrbManagers.Contains(orb) && orb)
            {
                OrbManagers.Add(orb);
            }

            _activeOrbs++;
            
            id = AddOrbInDirectory(orb);
            
            if (debug)
            {
                Debug.Log(id >= 0
                    ? "[ColliderDirectoryScript] Successful added orb to directory"
                    : "[ColliderDirectoryScript] Failed to add orb in directory");
            }
        }

        public void RemoveOrbManager(OrbManager orb)
        {
            _activeOrbs--;
            orb.Id = -1;
            if (OrbManagers.Contains(orb) && orb)
            {
                OrbManagers.Remove(orb);
            }
        }

        private int AddOrbInDirectory(OrbManager orb)
        {
            var id = -1;
            if(debug) Debug.Log("[ColliderDirectoryScript] Add one orb to directory");
            if (_orbsDirectory.ContainsValue(orb)) return id;
            _orbsDirectory.Add(orb.OrbCd, orb);
            id = _activeOrbs - 1;
            if(debug) Debug.LogFormat("[ColliderDirectoryScript] Directory key : {0} and value : {1}", 
                orb.OrbCd, orb);
            return id;
        }
        
        #endregion

        private IEnumerator Wait()
        {
            if(debug) Debug.Log("[ColliderDirectoryScript] Wait called");
            yield return new WaitForSeconds(0.5f);
        }
        
    }
}
