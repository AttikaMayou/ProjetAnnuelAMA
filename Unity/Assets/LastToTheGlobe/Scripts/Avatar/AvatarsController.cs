﻿using System.Collections;
using System.Collections.Generic;
using Assets.LastToTheGlobe.Scripts.Avatar;
using Assets.LastToTheGlobe.Scripts.Camera;
using Assets.LastToTheGlobe.Scripts.Management;
using Assets.LastToTheGlobe.Scripts.Weapon.Orb;
using LastToTheGlobe.Scripts.Camera;
using LastToTheGlobe.Scripts.Dev.LevelManager;
using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi.DEV;
using LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.Network;
using LastToTheGlobe.Scripts.UI;
using LastToTheGlobe.Scripts.Weapon.Orb;
using Photon.Pun;
using UnityEngine;

//Auteur : Attika
//Modification : Margot

namespace Assets.LastToTheGlobe.Scripts.Avatar
{
    public class AvatarsController : MonoBehaviour
    {
        public bool debug = true;
        
        [Header("Photon and Replication Parameters")] 
        [SerializeField] private CharacterExposerScript[] players;
        [SerializeField] private AIntentReceiver[] onlineIntentReceivers;
        private AIntentReceiver[] _activatedIntentReceivers;
        [SerializeField] private PhotonView photonView;
        
        [Header("Environment Parameters")]
        //spawn point tab
        private GameObject[] _spawnPointInPlanet;
        private List<Transform> _spawnPoints;
        private Vector3[] _spawnPos;
        [SerializeField] private CloudPlanet_PUN environmentController;
        private int _seed = 0;

        [Header("Camera Parameters")] 
        public CameraControllerScript myCamera;
        [SerializeField] private float rotationSpeed = 5.0f;

        [Header("UI Parameters")] 
        //public ActivateObjects inventoryUI;
        public ActivateObjects lifeUI;
        public ActivateObjects victoryUI;
        public ActivateObjects defeatUI;
        
        [Header("Game Control Parameters And References")]
        [SerializeField] private StartMenuController startMenuController;
        [SerializeField] private bool gameStarted;
        [SerializeField] private bool onLobby;
        [SerializeField] private bool gameLaunched;
        [SerializeField] private int nbMinPlayers = 2;
        [SerializeField] private float countdown = 10.0f;
        private float _countdownStartValue;
        
        [SerializeField] private List<OrbManager> orbsPool = new List<OrbManager>();
        private OrbManager _currentOrb;
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            gameStarted = false;
            onLobby = false;
            gameLaunched = false;

            myCamera.enabled = false;

            _countdownStartValue = countdown;
            
            startMenuController.OnlinePlayReady += ChooseAndSubscribeToIntentReceivers;
            startMenuController.PlayerJoined += ActivateAvatar;
            startMenuController.SetCamera += SetupCamera;
            //TODO : make sure attraction works before uncomment the following line
            //startMenuController.GameCanStart += LaunchGameRoom;
        }

        private void FixedUpdate()
        {
            if (onLobby && !gameLaunched)
            {
                countdown -= Time.deltaTime;
                startMenuController.UpdateCountdownValue(countdown);
                if (countdown <= 0.0f)
                {
                    countdown = 0.0f;
                    onLobby = false;
                }
            }
            
            if (!PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected)
            {
                return;
            }

            if (!gameStarted) return;

            if (Input.GetKeyDown(KeyCode.L))
            {
                StopCoroutine(CountdownBeforeSwitchingScene(.0f));
                StartCoroutine(CountdownBeforeSwitchingScene(.5f));
            }

            if (_activatedIntentReceivers == null
                || players == null
                || players.Length != _activatedIntentReceivers.Length)
            {
                Debug.LogError("[AvatarsController] " +
                               "There is something wrong with avatars and intents setup !");
                return;
            }
            
            var i = 0;
            for (; i < _activatedIntentReceivers.Length; i++)
            {
                var moveIntent = Vector3.zero;

                var intent = _activatedIntentReceivers[i];
                var player = players[i];

                var rb = player.CharacterRb;
                var tr = player.CharacterTr;

                if (player == null) continue;

                if (intent.Move)
                {
                    moveIntent += new Vector3(intent.Strafe, 0.0f, intent.Forward);
                }

                if (intent.Shoot)
                {
                    if(debug) Debug.Log("[AvatarsController] Shoot intent");
                    if (_currentOrb == null)
                    {
                        _currentOrb = GetOrbsWithinPool();
                        _currentOrb.playerTransform = player.CharacterTr;
                        _currentOrb.Attractor = player.Attractor;
                        _currentOrb.charged = false;
                        _currentOrb.gameObject.SetActive(true);
                        _currentOrb.InitializeOrPosition();
                        intent.CanShoot = true;
                        intent.Shoot = false;
                        _currentOrb = null;
                        //TODO : when the orb is reset --> canShoot = true
                    }
                }

                if (intent.ShootLoaded)
                {
                    if(debug) Debug.Log("[AvatarsController] Loaded shoot intent");
                    if (_currentOrb == null)
                    {
                        _currentOrb = GetOrbsWithinPool();
                        _currentOrb.playerTransform = player.CharacterTr;
                        _currentOrb.Attractor = player.Attractor;
                        _currentOrb.charged = true;
                        _currentOrb.gameObject.SetActive(true);
                        _currentOrb.InitializeOrPosition();
                        intent.CanShoot = true;
                        intent.Shoot = false;
                        _currentOrb = null;
                        //TODO : when the orb is reset --> canShoot = true
                    }
                }

                if (intent.Bump)
                {
                    
                }

                if (intent.Interact)
                {
                    
                }
                
                rb.MovePosition(rb.position + tr.TransformDirection(moveIntent) * 
                                intent.Speed * Time.deltaTime);
                tr.Rotate(new Vector3(0, intent.RotationOnX, 0));
                player.CameraRotatorX.transform.Rotate(new Vector3(-intent.RotationOnY, 
                    0, 0), Space.Self);

                if (player.Attractor == null)
                {
                    //Debug.LogError("There is no attractor near us !");
                    return;
                }
                
                //TODO : make this master client server like 
                player.Attractor.Attractor(i, -2600.0f);
                /*if (intent.canJump && player.attractor)
                {
                }
                else if(!intent.canJump && player.attractor)
                {
                }*/

                if (intent.Jump)
                {
                    var jumpDir = player.Attractor.DirForce;
                    rb.AddForce(jumpDir * 250);
                }
            }
        }

        #endregion

        #region Private Methods

        /// Set the intentReceivers tab
        private void ChooseAndSubscribeToIntentReceivers()
        {
            _activatedIntentReceivers = onlineIntentReceivers;
            EnableIntentReceivers();
            gameStarted = true;
        }

        /// Activate the intentReceivers and set defaults values
        private void EnableIntentReceivers()
        {
            if (_activatedIntentReceivers == null)
            {
                Debug.LogError("[AvatarsController] There is no intent receivers");
                return;
            }

            foreach (var intent in _activatedIntentReceivers)
            {
                intent.enabled = true;
                intent.Move = false;
                intent.Run = false;
                intent.Jump = false;
                intent.CanJump = true;
                intent.Dash = false;
                intent.CanDash = true;
                intent.Shoot = false;
                intent.CanShoot = true;
                intent.Bump = false;
                intent.Interact = false;
                intent.Forward = 0.0f;
                intent.Strafe = 0.0f;
            }
        }

        // Called to activate the avatar root gameObject when a player join the game
        private void ActivateAvatar(int id)
        {
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("ActivateAvatarRPC", RpcTarget.AllBuffered, id);
            }
            else
            {
                ActivateAvatarRPC(id);
            }
        }

        // Called to set the right local target to camera
        private void SetupCamera(int id)
        {
            if(debug) Debug.Log("[AvatarsController] Camera setup initialized");
            //if (photonView.IsMine != players[id].characterPhotonView) return;
            if (myCamera.enabled) return;
            myCamera.enabled = true;
            myCamera.PlayerExposer = players[id];
            myCamera.InitializeCameraPosition();
            myCamera.StartFollowing = true;
            players[id].LifeUi = lifeUI;
            players[id].VictoryUi = victoryUI;
            players[id].DefeatUi = defeatUI;
            if(debug) Debug.Log("[AvatarsController] Camera is set for " + id);
        }

        // Each time a player join the lobby, we check if we're enough.
        // If yes, we load the GameRoom after a countdown
        private void LaunchGameRoom()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (_seed == 0)
            {
                // _seed = environmentController.GetSeed();
                _seed = 10;
                environmentController.SetSeed(_seed);
                //TODO : make sure all the planets are being well instantiated before
                //calling 'FindAllSpawnPoint' 
                FindAllSpawnPoint();
                if(debug) Debug.Log("[AvatarsController] Seed is " + _seed);
            }
 
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("SendSeedToPlayers", RpcTarget.Others, _seed);
            }
            else
            {
                SendSeedToPlayers(_seed);
            }
            
            if(!CheckIfEnoughPlayers() || gameLaunched) return;
            onLobby = true;
            startMenuController.ShowLobbyCountdown();
            //TODO : call this for all players
            StartCoroutine(CountdownBeforeSwitchingScene(_countdownStartValue));
        }

        // Check if there is enough players to start the game and leave Lobby
        private bool CheckIfEnoughPlayers()
        {
            //TODO : refacto this function with Photon functions
            if (!gameStarted) return false;

            var j = 0;
            var i = 0;
            for (; i < players.Length; i++)
            {
                if (players[i].isActiveAndEnabled)
                {
                    j++;
                }
                else
                {
                    break;
                }
            }

            return j >= nbMinPlayers;
        }

        // Wait the time indicated before teleport players to the spawn points
        private IEnumerator CountdownBeforeSwitchingScene(float time = 2.0f)
        {
            yield return new WaitForSeconds(time);
            
            if (_spawnPointInPlanet.Length <= 1)
            {
                Debug.LogError("[AvatarsController] " +
                               "There is a problem with the map instantiation");
                yield break;
            }
            
            //Teleport players on planets
            for(var i = 0; i<= players.Length; i++)
            {
                if (!players[i].isActiveAndEnabled) break;
                //TODO : deactivate rb and set isKinematic = false 
                players[i].CharacterRootGameObject.transform.position = _spawnPos[i + 1];
                if (debug)
                {
                    Debug.Log("[AvatarsController] Previous pos : " 
                              + players[i].CharacterRootGameObject.transform.position);
                    Debug.Log("[AvatarsController] Final position : " 
                              + _spawnPointInPlanet[i].transform.position);
                    Debug.Log("[AvatarsController] Local position :  " 
                              + _spawnPointInPlanet[i].transform.localPosition);
                }
                yield return new WaitForSeconds(0.5f);
            }

            gameLaunched = true;
        }

        private void FindAllSpawnPoint()
        {
            foreach (var planet in ColliderDirectoryScript.Instance.PlanetExposers)
            {
                if (!planet.IsSpawnPlanet) continue;
                _spawnPoints.Add(planet.SpawnPosition);
            }
            
            if (debug)
            {
                var i = 0;
                foreach (var point in _spawnPoints)
                {
                    Debug.Log("Spawn Point : " + i + " is " + point.gameObject);
                    i++;
                }
            }
            
            _spawnPos = new Vector3[_spawnPoints.Count];
            for (var i = 0; i < _spawnPoints.Count; i++)
            {
                _spawnPos[i] = _spawnPoints[i].position;
            }
        }

        private OrbManager GetOrbsWithinPool()
        {
            foreach (var orb in orbsPool)
            {
                if (orb.gameObject.activeSelf)
                {
                    if(debug) Debug.Log("[AvatarsController] Orb is enabled");
                    continue;
                }
                else
                {
                    if (debug) Debug.Log("[AvatarsController] Orb selected is " + orb);
                    return orb;
                }
            }
            if(debug) Debug.Log("[AvatarsController] There is no orb available");
            return null;
        }

        #endregion

        #region RPC Methods

        [PunRPC]
        private void ActivateAvatarRPC(int avatarId)
        {
            players[avatarId].CharacterRootGameObject.SetActive(true);
        }

        [PunRPC]
        private void DeactivateAvatarRPC(int avatarId)
        {
            players[avatarId].CharacterRootGameObject.SetActive(false);
        }

        [PunRPC]
        private void SendSeedToPlayers(int seed)
        {
            _seed = seed;
            if (debug) Debug.Log("My seed is : " + seed);
            //environmentController.SetSeed(_seed);
        }

        #endregion
    }
}
