using System.Collections;
using System.Collections.Generic;
using Assets.LastToTheGlobe.Scripts.Avatar;
using Assets.LastToTheGlobe.Scripts.Environment.Planets;
using Assets.LastToTheGlobe.Scripts.Weapon.Orb;
using LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.Singleton;
using Photon.Pun;
using UnityEngine;

//Auteur : Attika

namespace LastToTheGlobe.Scripts.Management
{
    public class ColliderDirectoryScript : MonoBehaviourSingleton<ColliderDirectoryScript>
    {
        public bool debug = false;

        [Header("Photon View to assign")] 
        public PhotonView BumpersPhotonView;
        public PhotonView PlanetsPhotonView;
        public PhotonView OrbsPhotonView;
        
        public List<CharacterExposerScript> CharacterExposers;
        public int ActivePlayers = 0;
        public List<PlanetExposerScript> PlanetExposers;
        [SerializeField] private int _activePlanets = 0;
        public List<OrbExposerScript> OrbExposers;
        [SerializeField] private int _activeOrbs = 0;
        public List<BumperExposerScript> BumperExposers;
        [SerializeField] private int _activeBumpers = 0;
        
        private Dictionary<Collider, CharacterExposerScript> _playersDirectory = new Dictionary<Collider, CharacterExposerScript>();
        private CharacterExposerScript _playerValue;
        
        private Dictionary<Collider, PlanetExposerScript> _planetsDirectory = new Dictionary<Collider, PlanetExposerScript>();
        private PlanetExposerScript _planetValue;

        private Dictionary<Collider, OrbExposerScript> _orbsDirectory = new Dictionary<Collider, OrbExposerScript>();
        private OrbExposerScript _orbValue;
        
        private Dictionary<Collider, BumperExposerScript> _bumpersDirectory = new Dictionary<Collider, BumperExposerScript>();
        private BumperExposerScript _bumperValue;
        
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

            ActivePlayers++;
            
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
            ActivePlayers--;
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
            id = ActivePlayers - 1;
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
            Debug.LogErrorFormat("[ColliderDirectoryScript] No PlanetExposer found with this collider {0}",
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
            planet.PlanetsPhotonView = PlanetsPhotonView;
            
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
            id = _activePlanets - 1;
            if(debug) Debug.LogFormat("[ColliderDirectoryScript] Directory key : {0} and value : {1}", 
                planet.PlanetCollider, planet);
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
            if (id < 0 || id >= OrbExposers.Count) return null;
            return !PhotonNetwork.IsMasterClient ? null : OrbExposers[id];
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
            if (OrbExposers == null)
            {
                OrbExposers = new List<OrbExposerScript>();
            }

            if (!OrbExposers.Contains(orb) && orb)
            {
                OrbExposers.Add(orb);
            }

            _activeOrbs++;
            
            id = AddOrbInDirectory(orb);
            orb.OrbsPhotonView = OrbsPhotonView;
            
            if (debug)
            {
                Debug.Log(id >= 0
                    ? "[ColliderDirectoryScript] Successful added orb to directory"
                    : "[ColliderDirectoryScript] Failed to add orb in directory");
            }
        }

        public void RemoveOrbExposer(OrbExposerScript orb)
        {
            _activeOrbs--;
            orb.Id = -1;
            if (OrbExposers.Contains(orb) && orb)
            {
                OrbExposers.Remove(orb);
            }
        }

        private int AddOrbInDirectory(OrbExposerScript orb)
        {
            var id = -1;
            if(debug) Debug.Log("[ColliderDirectoryScript] Add one orb to directory");
            if (_orbsDirectory.ContainsValue(orb)) return id;
            _orbsDirectory.Add(orb.OrbCollider, orb);
            id = _activeOrbs - 1;
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
            if (id < 0 || id >= BumperExposers.Count) return null;
            return !PhotonNetwork.IsMasterClient ? null : BumperExposers[id];
        }

        public int GetBumperId(Collider col)
        {
            if (!col) return -1;
            if (BumperExposers.Count == 0) StartCoroutine(Wait());
            var bumper = GetBumperExposer(col);
            if (bumper) return bumper.Id;
            Debug.LogWarningFormat("[ColliderDirectoryScript] No BumperExposer found with this collider {0}", 
                col.name);
            return -1;
        }

        public void AddBumperExposer(BumperExposerScript bumper, out int id)
        {
            if (BumperExposers == null)
            {
                BumperExposers = new List<BumperExposerScript>();
            }

            if (!BumperExposers.Contains(bumper) && bumper)
            {
                BumperExposers.Add(bumper);
            }

            _activeBumpers++;

            id = AddBumperInDirectory(bumper);
            bumper.BumpersPhotonView = BumpersPhotonView;

            if (debug)
            {
                Debug.Log(id >= 0
                    ? "[ColliderDirectoryScript] Successful added bumper to directory"
                    : "[ColliderDirectoryScript] Failed to add bumper in directory");
            }
        }

        public void RemoveBumperExposer(BumperExposerScript bumper)
        {
            _activeBumpers--;
            bumper.Id = -1;
            if (BumperExposers.Contains(bumper) && bumper)
            {
                BumperExposers.Remove(bumper);
            }
        }
        
        private int AddBumperInDirectory(BumperExposerScript bumper)
        {
            var id = -1;
            if(debug) Debug.Log("[ColliderDirectoryScript] Add one bumper to directory");
            if (_bumpersDirectory.ContainsValue(bumper)) return id;
            _bumpersDirectory.Add(bumper.BumperCollider, bumper);
            id = ActivePlayers - 1;
            if(debug) Debug.LogFormat("[ColliderDirectoryScript] Directory key : {0} and value : {1}", 
                bumper.BumperCollider, bumper);
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
