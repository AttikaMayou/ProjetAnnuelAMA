﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.Camera;
using LastToTheGlobe.Scripts.Chest;
using LastToTheGlobe.Scripts.Environment;
using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Planet;
using LastToTheGlobe.Scripts.Management;
using LastToTheGlobe.Scripts.Network;
using LastToTheGlobe.Scripts.UI;
using LastToTheGlobe.Scripts.Weapon.Orb;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.Serialization;

//Auteur : Attika
//Modification : Margot

namespace LastToTheGlobe.Scripts.Avatar
{
    public class AvatarsController : MonoBehaviour
    {
        public bool debug = true;

        [FormerlySerializedAs("_startPosition")]
        [Header("Photon and Replication Parameters")] 
        [SerializeField] private Transform[] startPosition;
        [SerializeField] private CharacterExposerScript[] players;
        [SerializeField] private AIntentReceiver[] onlineIntentReceivers;
        private AIntentReceiver[] _activatedIntentReceivers;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private AvatarAnimation avatarAnimation;

        [Header("Environment Parameters")]
        private List<int> _spawnPos = new List<int>();
        [SerializeField] private CloudPlanet environmentController;
        private int _seed = 0;

        //[SerializeField] private GPInstanciation _lobbyAssets;

        [Header("Camera Parameters")] 
        public CameraControllerScript myCamera;
        [SerializeField] private float rotationSpeed = 5.0f;


        private List<ChestExposerScript> _chestExposers;

        [FormerlySerializedAs("lifeUI")] [Header("UI Parameters")] 
        //public ActivateObjects inventoryUI;
        public ActivateObjects lifeUi;
        [FormerlySerializedAs("victoryUI")] public ActivateObjects victoryUi;
        [FormerlySerializedAs("defeatUI")] public ActivateObjects defeatUi;
        
        [Header("Game Control Parameters And References")]
        [SerializeField] private StartMenuController startMenuController;
        [FormerlySerializedAs("_gameStarted")] [SerializeField] private bool gameStarted;
        private bool GameStarted
        {
            get => gameStarted;
            set
            {
                if (value && !gameStarted &&(!PhotonNetwork.IsConnected
                                             || PhotonNetwork.IsMasterClient))
                {
                    SubscribeCollisionEffect();
                }

                gameStarted = value;
            }
        }
        
        [SerializeField] private bool onLobby;
        [SerializeField] private bool gameLaunched;
        [SerializeField] private int nbMinPlayers = 2;
        [SerializeField] private float countdown = 10.0f;
        private float _countdownStartValue;
        private readonly Dictionary<CollisionEnterDispatcherScript, CharacterExposerScript>
            _dispatcherToCharacterExposer = new Dictionary<CollisionEnterDispatcherScript, CharacterExposerScript>();

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
            startMenuController.Disconnected += EndGame;
            startMenuController.MasterClientSwitched += EndGame;
            startMenuController.GameCanStart += LaunchGameRoom;

            avatarAnimation.character = players;

            OnlineIntentReceiver.Debug= debug;
            
            //Setting seed for chests
            

        }


        private void Update()
        {   
//            if (Input.GetKeyDown(KeyCode.Escape))
//            {
//                EndGame();
//                return;
//            }
            
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

            if (!GameStarted) return;

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

                if (!player.CharacterRootGameObject.activeSelf) continue;

                if (intent.Move)
                {
                    moveIntent += new Vector3(intent.strafe, 0.0f, intent.forward);
                }
                
                if (intent.Shoot)
                {
                    if(debug) Debug.LogFormat("[AvatarsController]  Player {0} Shoot intent", player);
                    
                    var orb = GetOrbsWithinPool();
                    orb.exposer.playerExposer = player;
                   // orb.exposer.Attractor = player.Attractor;
                    orb.Color.material.color = player.colorPreferences;
                    orb.loaded = false;
                    orb.gameObject.SetActive(true);
                    orb.InitializeOrPosition();
                    
                    intent.Shoot = false;
                    StartCoroutine(CooldownReset(GameVariablesScript.Instance.shootCooldown));
                    intent.canShoot = true;
                }

                if (intent.ShootLoaded)
                {
                    if(debug) Debug.Log("[AvatarsController] Loaded shoot intent");
                    
                    var orb = GetOrbsWithinPool();
                    orb.Color.material.color = player.colorPreferences;
                    orb.exposer.playerExposer = player;
                    orb.exposer.Attractor = player.Attractor;
                    orb.loaded = true;
                    orb.gameObject.SetActive(true);
                    orb.InitializeOrPosition();
                    
                    intent.ShootLoaded = false;
                    StartCoroutine(CooldownReset(GameVariablesScript.Instance.shootCooldown));
                    intent.canShoot = true;
                }

                if (intent.Bump)
                {
                    if (!player.Bumper)
                    {
                        Debug.LogFormat("[AvatarsController] Player {0} is trying to bump but there is no bumper around him", player);
                    }
                    else
                    {
                        player.Bumper.BumpPlayer(player.Bumper.exposer.Id,i,  GameVariablesScript.Instance.bumpersForce);
                        
                    }
                    StartCoroutine(CooldownReset(GameVariablesScript.Instance.bumpCooldown));
                    intent.Bump = false;
                }

                if (!intent.canDash)
                {
                    if (player.inventoryScript.isItemInInventory("Dash"))
                    {
                        intent.canDash = true;
                    }
                }

                rb.MovePosition(rb.position + intent.speed * Time.deltaTime * tr.TransformDirection(moveIntent));
                tr.Rotate(new Vector3(0, intent.rotationOnX, 0));
                //myCamera.transform.Rotate(new Vector3(-intent.rotationOnY, 0, 0));

                if (player.Attractor == null)
                {
                    //if(debug) Debug.LogFormat("[AvatarsController] {0} isn't actually attracted by anything",
                      //  player);
                    continue;
                }
                
                player.Attractor.AttractPlayer(player.Attractor.exposer.id,i, GameVariablesScript.Instance.planetsGravity);
            }

            if(!gameLaunched) return;
            /*if (ColliderDirectoryScript.Instance.activePlayers <= 3)
            {
                foreach (var player in players)
                {
                    if(!player.avatarLifeManager.alive) return;
                    player.VictoryUi.Activation();
                    player.CharacterRootGameObject.SetActive(false);
                }
                //TODO : active Victory UI for the victorious player
                //EndGame();
            }*/
        }

        #endregion

        #region Private Methods

        /// Set the intentReceivers tab
        private void ChooseAndSubscribeToIntentReceivers()
        {
            _activatedIntentReceivers = onlineIntentReceivers;
            ResetGame();
            
            //Animation
            avatarAnimation.intentReceivers = _activatedIntentReceivers;
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
                intent.Dash = false;
                intent.canDash = true;
                intent.Shoot = false;
                intent.canShoot = true;
                intent.Bump = false;
                intent.Interact = false;
                intent.forward = 0.0f;
                intent.strafe = 0.0f;
                intent.lockCursor = GameVariablesScript.Instance.lockCursor;
            }
        }

        private void DisableIntentReceivers()
        {
            if (_activatedIntentReceivers == null) return;
            
            var i = 0;
            for (; i < _activatedIntentReceivers.Length; i++)
            {
                _activatedIntentReceivers[i].enabled = false;
            }
        }

        // Called to activate the avatar root gameObject when a player join the game
        private void ActivateAvatar(int id)
        {
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("ActivateAvatarRpc", RpcTarget.AllBuffered, id);
            }
            else
            {
                ActivateAvatarRpc(id);
            }
        }

        // Called to set the right local target to camera
        private void SetupCamera(int id)
        {
            if(debug) Debug.Log("[AvatarsController] Camera setup initialized");
            if (myCamera.enabled) return;
            myCamera.enabled = true;
            myCamera.playerExposer = players[id];
            myCamera.InitializeCameraPosition();
            myCamera.startFollowing = true;
//            players[id].LifeUi = lifeUI;
//            players[id].VictoryUi = victoryUI;
//            players[id].DefeatUi = defeatUI;
            if(debug) Debug.Log("[AvatarsController] Camera is set for " + id);
        }

        // Each time a player join the lobby, we check if we're enough.
        // If yes, we load the GameRoom after a countdown
        private void LaunchGameRoom()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (_seed == 0)
            {
                _seed = 1;
                environmentController.GenerateMap();
                FindAllSpawnPoints();
                if(debug) Debug.Log("[AvatarsController] Seed generated.");
            }
 
            if (PhotonNetwork.IsConnected)
            {
                Debug.LogFormat("[AvatarsController] I am calling send seed to players and I aaaaam the Master ? {0}", PhotonNetwork.IsMasterClient);
                photonView.RPC("SendSeedToPlayers", RpcTarget.OthersBuffered, environmentController.GetIndices(), environmentController.GetVertices());
            }
            else
            {
                SendSeedToPlayers(environmentController.GetIndices(), environmentController.GetVertices());
                
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
            //if (!_gameStarted) return false;
            //_lobbyAssets.enabled = true;
            return false;
            /*var j = 0;
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

            return j >= nbMinPlayers;*/
        }

        // Wait the time indicated before teleport players to the spawn points
        private IEnumerator CountdownBeforeSwitchingScene(float time = 2.0f)
        {
            yield return new WaitForSeconds(time);
            
            if (_spawnPos.Count <= 1)
            {
                Debug.LogError("[AvatarsController] " +
                               "There is a problem with the map instantiation");
                yield break;
            }
            
            //Teleport players on planets
            for(var i = 0; i <= players.Length; i++)
            {
                if (!players[i].isActiveAndEnabled) break;
                var planet = ColliderDirectoryScript.Instance.GetPlanetExposer(_spawnPos[i]);
                if (planet == null) continue;
                players[i].CharacterRootGameObject.transform.position =planet.spawnPosition.position;
                players[i].Attractor = planet.attractorScript;
                yield return new WaitForSeconds(0.5f);
                if (!debug) continue;
                Debug.Log("[AvatarsController] Previous pos : " 
                          + players[i].CharacterRootGameObject.transform.position);
                Debug.Log("[AvatarsController] Final position : " 
                          + _spawnPos[i]);
            }
            
            gameLaunched = true;
        }

        private void FindAllSpawnPoints()
        {
            foreach (var planet in ColliderDirectoryScript.Instance.planetExposers)
            {
                if (!planet) continue;
                if (!planet.isSpawnPlanet) continue;
                if (_spawnPos.Contains(planet.id)) continue;
                _spawnPos.Add(planet.id);
            }
        }

        private void SubscribeCollisionEffect()
        {
            foreach (var player in players)
            {
                player.CollisionDispatcher.CollisionEvent += HandleCollision;
            }
        }

        private void HandleCollision(CollisionEnterDispatcherScript collisionDispatcher,
            Collider col)
        {
            var avatar = ColliderDirectoryScript.Instance.GetCharacterExposer(col);
            if (!_dispatcherToCharacterExposer.TryGetValue(collisionDispatcher, out var sourceAvatar)
                || !avatar)
            {
                return;
            }

            //TODO : add a logic to know if the shoot was loaded or not so remove hp according to it
            avatar.HitPointComponent.Hp -= GameVariablesScript.Instance.shootLoadedDamage;
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

        private void ResetGame()
        {
            var i = 0;
            for (; i < players.Length; i++)
            {
                var player = players[i];
                player.CharacterRb.velocity = Vector3.zero;
                player.CharacterRb.angularVelocity = Vector3.zero;
                player.CharacterTr.position = startPosition[i].position;
                player.CharacterTr.rotation = startPosition[i].rotation;
                player.CharacterRbPhotonView.enabled = _activatedIntentReceivers == onlineIntentReceivers;
            }
            
            EnableIntentReceivers();
            GameStarted = true;
        }

        private void EndGame()
        {
            GameStarted = false;
            _activatedIntentReceivers = null;

            var i = 0;
            for (; i < players.Length; i++)
            {
                players[i].CharacterRootGameObject.SetActive(false);
            }
            
            startMenuController.ShowMainMenu();

            DisableIntentReceivers();
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }
            
        }

        private IEnumerator CooldownReset(float time)
        {
            yield return new WaitForSeconds(time);
        }
        
        #endregion

        #region RPC Methods

        [PunRPC]
        private void ActivateAvatarRpc(int avatarId)
        {
            players[avatarId].CharacterRootGameObject.SetActive(true);
        }
        
        [PunRPC]
        private void DeactivateAvatarRpc(int avatarId)
        {
            players[avatarId].CharacterRootGameObject.SetActive(false);
        }

        [PunRPC]
        private void SendSeedToPlayers(int[] indices, Vector3[] vertices)
        {
            if (PhotonNetwork.IsMasterClient) return;
            environmentController.SetIndices(indices);
            environmentController.SetVertices(vertices);
            environmentController.GenerateMap();
        }


        #endregion
    }
}
