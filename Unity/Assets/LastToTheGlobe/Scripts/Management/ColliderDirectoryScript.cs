using System.Collections;
using System.Collections.Generic;
using Assets.LastToTheGlobe.Scripts.Avatar;
using Assets.LastToTheGlobe.Scripts.Weapon.Orb;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Chest;
using LastToTheGlobe.Scripts.Environment.Planets;
using LastToTheGlobe.Scripts.Singleton;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Management
{
    public class ColliderDirectoryScript : MonoBehaviourSingleton<ColliderDirectoryScript>
    {
        public bool debug = false;

        [Header("Photon View to assign")] 
        public PhotonView bumpersPhotonView;
        public PhotonView planetsPhotonView;
        public PhotonView orbsPhotonView;
        public PhotonView chestPhotonView;
        
        public List<CharacterExposerScript> characterExposers;
        public int activePlayers = 0;
        public List<PlanetExposerScript> planetExposers;
        [SerializeField] private int activePlanets = 0;
        public List<OrbExposerScript> orbExposers;
        [SerializeField] private int activeOrbs = 0;
        public List<BumperExposerScript> bumperExposers;
        [SerializeField] private int activeBumpers = 0;
        public List<ChestExposerScript> chestExposers;
        public int activeChests = 0;
        
        private Dictionary<Collider, CharacterExposerScript> _playersDirectory = new Dictionary<Collider, CharacterExposerScript>();
        private CharacterExposerScript _playerValue;
        
        private Dictionary<Collider, PlanetExposerScript> _planetsDirectory = new Dictionary<Collider, PlanetExposerScript>();
        private PlanetExposerScript _planetValue;

        private Dictionary<Collider, OrbExposerScript> _orbsDirectory = new Dictionary<Collider, OrbExposerScript>();
        private OrbExposerScript _orbValue;
        
        private Dictionary<Collider, BumperExposerScript> _bumpersDirectory = new Dictionary<Collider, BumperExposerScript>();
        private BumperExposerScript _bumperValue;
        
        private Dictionary<Collider, ChestExposerScript> _chestsDirectory = new Dictionary<Collider, ChestExposerScript>();
        private ChestExposerScript _chestValue;
        
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
            if (id < 0 || id >= characterExposers.Count) return null;
            return !PhotonNetwork.IsMasterClient ? null : characterExposers[id];
        }

        public int GetPlayerId(Collider col)
        {
            if (!col) return -1;
            if (characterExposers.Count == 0) StartCoroutine(Wait());
            var player = GetCharacterExposer(col);
            if (player) return player.Id;
            Debug.LogWarningFormat("[ColliderDirectoryScript] No CharacterExposer found with this collider {0}", 
                col.name);
            return -1;
        }

        public void AddCharacterExposer(CharacterExposerScript player, out int id)
        {
            if (characterExposers == null)
            {
                characterExposers = new List<CharacterExposerScript>();
            }

            if (!characterExposers.Contains(player) && player)
            {
                characterExposers.Add(player);
            }

            activePlayers++;
            
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
            activePlayers--;
            player.Id = -1;
            if (characterExposers.Contains(player) && player)
            {
                characterExposers.Remove(player);
            }
        }

        private int AddPlayerInDirectory(CharacterExposerScript player)
        {
            var id = -1;
            if(debug) Debug.Log("[ColliderDirectoryScript] Add one player to directory");
            if (_playersDirectory.ContainsValue(player)) return id;
            _playersDirectory.Add(player.CharacterCollider, player);
            id = activePlayers - 1;
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
            if (id < 0 || id >= planetExposers.Count) return null;
            return !PhotonNetwork.IsMasterClient ? null : planetExposers[id];
        }
        
        public int GetPlanetId(Collider col)
        {
            if (!col) return -1;
            var planet = GetPlanetExposer(col);
            if (planet) return planet.id;
            Debug.LogErrorFormat("[ColliderDirectoryScript] No PlanetExposer found with this collider {0}",
                col.name);
            return -1;
        }

        public void AddPlanetExposer(PlanetExposerScript planet, out int id)
        {
            if (planetExposers == null)
            {
                planetExposers = new List<PlanetExposerScript>();
            }

            if (!planetExposers.Contains(planet) && planet)
            {
                planetExposers.Add(planet);
            }

            activePlanets++;
            
            id = AddPlanetInDirectory(planet);
            planet.planetsPhotonView = planetsPhotonView;
            
            if (debug)
            {
                Debug.Log(id >= 0
                    ? "[ColliderDirectoryScript] Successful added planet to directory"
                    : "[ColliderDirectoryScript] Failed to add planet in directory");
            }
        }

        public void RemovePlanetExposer(PlanetExposerScript planet)
        {
            activePlanets--;
            planet.id = -1;
            if (planetExposers.Contains(planet) && planet)
            {
                planetExposers.Remove(planet);
            }
        }

        private int AddPlanetInDirectory(PlanetExposerScript planet)
        {
            var id = -1;
            if(debug) Debug.Log("[ColliderDirectoryScript] Add one planet to directory");
            if (_planetsDirectory.ContainsValue(planet)) return id;
            _planetsDirectory.Add(planet.planetCollider, planet);
            id = activePlanets - 1;
            if(debug) Debug.LogFormat("[ColliderDirectoryScript] Directory key : {0} and value : {1}", 
                planet.planetCollider, planet);
            return id;
        }
        
        #endregion
        
        #region Orbs Methods

        public OrbExposerScript GetOrbExposer(Collider col)
        {
            if(debug) Debug.Log("[ColliderDirectoryScript] Trying to find orb from this collider : " + col);
            if (!PhotonNetwork.IsMasterClient) return null;
            return _orbsDirectory.TryGetValue(col, out _orbValue) ? _orbValue : null;
        }
        
        public OrbExposerScript GetOrbExposer(int id)
        {
            if(debug) Debug.Log("[ColliderDirectoryScript] Trying to find orb from this id: " + id);
            if (id < 0 || id >= orbExposers.Count) return null;
            return !PhotonNetwork.IsMasterClient ? null : orbExposers[id];
        }

        public int GetOrbId(Collider col)
        {
            if (!col) return -1;
            var orb = GetOrbExposer(col);
            if (orb) return orb.Id;
            Debug.LogWarningFormat("[ColliderDirectoryScript] No OrbExposer found with this collider {0}", 
                col.name);
            return -1;
        }
        
        public void AddOrbExposer(OrbExposerScript orb, out int id)
        {
            if (orbExposers == null)
            {
                orbExposers = new List<OrbExposerScript>();
            }

            if (!orbExposers.Contains(orb) && orb)
            {
                orbExposers.Add(orb);
            }

            activeOrbs++;
            
            id = AddOrbInDirectory(orb);
            orb.OrbsPhotonView = orbsPhotonView;
            
            if (debug)
            {
                Debug.Log(id >= 0
                    ? "[ColliderDirectoryScript] Successful added orb to directory"
                    : "[ColliderDirectoryScript] Failed to add orb in directory");
            }
        }

        public void RemoveOrbExposer(OrbExposerScript orb)
        {
            activeOrbs--;
            orb.Id = -1;
            if (orbExposers.Contains(orb) && orb)
            {
                orbExposers.Remove(orb);
            }
        }

        private int AddOrbInDirectory(OrbExposerScript orb)
        {
            var id = -1;
            if(debug) Debug.Log("[ColliderDirectoryScript] Add one orb to directory");
            if (_orbsDirectory.ContainsValue(orb)) return id;
            _orbsDirectory.Add(orb.OrbCollider, orb);
            id = activeOrbs - 1;
            if(debug) Debug.LogWarningFormat("[ColliderDirectoryScript] Directory key : {0} and value : {1}", 
                orb.OrbCollider, orb);
            return id;
        }
        
        #endregion

        #region Bumpers Methods

        public BumperExposerScript GetBumperExposer(Collider col)
        {
            if(debug) Debug.Log("[ColliderDirectoryScript] Trying to find bumper from " +
                                "this collider : " + col);
            if (!PhotonNetwork.IsMasterClient) return null;
            return _bumpersDirectory.TryGetValue(col, out _bumperValue) ? _bumperValue : null;
        }

        public BumperExposerScript GetBumperExposer(int id)
        {
            if(debug) Debug.Log("[ColliderDirectoryScript] Trying to find bumper from " +
                                "this id: " + id);
            if (id < 0 || id >= bumperExposers.Count) return null;
            return !PhotonNetwork.IsMasterClient ? null : bumperExposers[id];
        }

        public int GetBumperId(Collider col)
        {
            if (!col) return -1;
            if (bumperExposers.Count == 0) StartCoroutine(Wait());
            var bumper = GetBumperExposer(col);
            if (bumper) return bumper.Id;
            Debug.LogWarningFormat("[ColliderDirectoryScript] No BumperExposer found with this collider {0}", 
                col.name);
            return -1;
        }

        public void AddBumperExposer(BumperExposerScript bumper, out int id)
        {
            if (bumperExposers == null)
            {
                bumperExposers = new List<BumperExposerScript>();
            }

            if (!bumperExposers.Contains(bumper) && bumper)
            {
                bumperExposers.Add(bumper);
            }

            activeBumpers++;

            id = AddBumperInDirectory(bumper);
            bumper.BumpersPhotonView = bumpersPhotonView;

            if (debug)
            {
                Debug.Log(id >= 0
                    ? "[ColliderDirectoryScript] Successful added bumper to directory"
                    : "[ColliderDirectoryScript] Failed to add bumper in directory");
            }
        }

        public void RemoveBumperExposer(BumperExposerScript bumper)
        {
            activeBumpers--;
            bumper.Id = -1;
            if (bumperExposers.Contains(bumper) && bumper)
            {
                bumperExposers.Remove(bumper);
            }
        }
        
        private int AddBumperInDirectory(BumperExposerScript bumper)
        {
            var id = -1;
            if(debug) Debug.Log("[ColliderDirectoryScript] Add one bumper to directory");
            if (_bumpersDirectory.ContainsValue(bumper)) return id;
            _bumpersDirectory.Add(bumper.BumperCollider, bumper);
            id = activePlayers - 1;
            if(debug) Debug.LogFormat("[ColliderDirectoryScript] Directory key : {0} and value : {1}", 
                bumper.BumperCollider, bumper);
            return id;
        }
        
        #endregion
        
        #region Chest Method
        
        public ChestExposerScript GetChestExposer(Collider col)
        {
            if(debug) Debug.Log("[ColliderDirectoryScript] Trying to find chest from this collider : " + col);
            if (!PhotonNetwork.IsMasterClient) return null;
            return _chestsDirectory.TryGetValue(col, out _chestValue) ? _chestValue : null;
        }
        
        public ChestExposerScript GetChestExposer(int id)
        {
            if(debug) Debug.Log("[ColliderDirectoryScript] Trying to find bumper from this id: " + id);
            if (id < 0 || id >= chestExposers.Count) return null;
            return !PhotonNetwork.IsMasterClient ? null : chestExposers[id];
        }
        
        public int GetChestId(Collider col)
        {
            if (!col) return -1;
            if (chestExposers.Count == 0) StartCoroutine(Wait());
            var chest = GetChestExposer(col);
            if (chest) return chest.Id;
            Debug.LogWarningFormat("[ColliderDirectoryScript] No ChestExposer found with this collider {0}", col.name);
            return -1;
        }
        
        #endregion
        
        private IEnumerator Wait()
        {
            if(debug) Debug.Log("[ColliderDirectoryScript] Wait called");
            yield return new WaitForSeconds(0.5f);
        }
        
    }
}
