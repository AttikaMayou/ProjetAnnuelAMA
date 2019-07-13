using System.Collections;
using System.Collections.Generic;
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
using UnityEngine;

//Auteur : Attika
//Modification : Margot

namespace LastToTheGlobe.Scripts.Avatar
{
    public class AvatarsController : MonoBehaviour
    {
        public bool debug = false;

        [Header("Photon and Replication Parameters")] 
        [SerializeField] private Transform[] _startPosition;
        [SerializeField] private CharacterExposerScript[] players;
        [SerializeField] private AIntentReceiver[] onlineIntentReceivers;
        private AIntentReceiver[] _activatedIntentReceivers;
        [SerializeField] private PhotonView photonView;
        [SerializeField] private AvatarAnimation avatarAnimation;

        [Header("Environment Parameters")]
        //spawn point tab
        private List<Transform> _spawnPoints = new List<Transform>();
        private Vector3[] _spawnPos;
        [SerializeField] private CloudPlanet environmentController;
        private int _seed = 0;

        private int _chestSeed;
        //[SerializeField] private GPInstanciation _lobbyAssets;

        [Header("Camera Parameters")] 
        public CameraControllerScript myCamera;
        [SerializeField] private float rotationSpeed = 5.0f;


        private List<ChestExposerScript> chestExposers;

        [Header("UI Parameters")] 
        //public ActivateObjects inventoryUI;
        public ActivateObjects lifeUI;
        public ActivateObjects victoryUI;
        public ActivateObjects defeatUI;
        
        [Header("Game Control Parameters And References")]
        [SerializeField] private StartMenuController startMenuController;
        [SerializeField] private bool _gameStarted;
        private bool GameStarted
        {
            get => _gameStarted;
            set
            {
                if (value && !_gameStarted &&(!PhotonNetwork.IsConnected
                                             || PhotonNetwork.IsMasterClient))
                {
                    SubscribeCollisionEffect();
                }

                _gameStarted = value;
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
            _gameStarted = false;
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
            
            _chestSeed = Random.Range(0,255);

        }


        private void FixedUpdate()
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
                    if(debug) Debug.Log("[AvatarsController] Shoot intent");
                    
                    var orb = GetOrbsWithinPool();
                    orb.exposer.playerExposer = player;
                    orb.Attractor = player.Attractor;
                    orb.loaded = false;
                    orb.gameObject.SetActive(true);
                    orb.InitializeOrPosition();
                    
                    intent.Shoot = false;
                    intent.canShoot = true;
                }

                if (intent.ShootLoaded)
                {
                    if(debug) Debug.Log("[AvatarsController] Loaded shoot intent");
                    
                    var orb = GetOrbsWithinPool();
                    orb.exposer.playerExposer = player;
                    orb.Attractor = player.Attractor;
                    orb.loaded = true;
                    orb.gameObject.SetActive(true);
                    orb.InitializeOrPosition();
                    
                    intent.ShootLoaded = false;
                    intent.canShoot = true;
                }
                
                /*if (intent.Shoot)
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
                    }
                }*/

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
                player.CameraRotatorX.transform.Rotate(new Vector3(-intent.rotationOnY, 
                    0, 0), Space.Self);
                
                //Get back to initial values to prevent from network lags and stuff like this
                intent.rotationOnX = 0.0f;
                intent.rotationOnY = 0.0f;
                intent.strafe = 0.0f;
                intent.forward = 0.0f;

                if (player.Attractor == null)
                {
                    if(debug) Debug.LogFormat("[AvatarsController] {0} isn't actually attracted by anything",
                        player);
                    continue;
                }
                
                player.Attractor.AttractPlayer(player.Attractor.Exposer.id,i, GameVariablesScript.Instance.planetsGravity);
                
                /*if (intent.canJump && player.attractor)
                {
                }
                else if(!intent.canJump && player.attractor)
                {
                }

                if (intent.Jump)
                {
                    var jumpDir = player.Attractor.DirForce;
                    rb.AddForce(jumpDir * 250);
                }*/
            }

//            if (ColliderDirectoryScript.Instance.ActivePlayers == 1)
//            {
//                //TODO : active Victory UI for the victorious player
//                EndGame();
//            }
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
//                intent.Jump = false;
//                intent.CanJump = true;
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
                // _seed = environmentController.GetSeed();
                _seed = 1;
                environmentController.GenerateMap();
                //TODO : make sure all the planets are being well instantiated before
                //calling 'FindAllSpawnPoint' 
                FindAllSpawnPoints();
                if(debug) Debug.Log("[AvatarsController] Seed generated.");
            }
 
            if (PhotonNetwork.IsConnected)
            {
                Debug.LogFormat("[AvatarsController] I am calling send seed to players and I aaaaam the Master ? {0}", PhotonNetwork.IsMasterClient);
                photonView.RPC("SendSeedToPlayers", RpcTarget.OthersBuffered, environmentController.GetIndices(), environmentController.GetVertices());
                SendChestSeedToPlayers(_chestSeed);
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
            
            if (_spawnPos.Length <= 1)
            {
                Debug.LogError("[AvatarsController] " +
                               "There is a problem with the map instantiation");
                yield break;
            }
            
            //Teleport players on planets
            for(var i = 0; i <= players.Length; i++)
            {
                if (!players[i].isActiveAndEnabled) break;
                players[i].DeactivateRb();
                players[i].CharacterRootGameObject.transform.position = _spawnPos[i + 1];
                yield return new WaitForSeconds(0.5f);
                players[i].ActivateRb();
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
                if (_spawnPoints.Contains(planet.spawnPosition)) continue;
                _spawnPoints.Add(planet.spawnPosition);
                //Deactivate the collider so the attraction will not work there
                planet.DeactivateCollider();
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
                player.CharacterTr.position = _startPosition[i].position;
                player.CharacterTr.rotation = _startPosition[i].rotation;
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
        private void SendSeedToPlayers(int[] indices, Vector3[] vertices)
        {
            if (PhotonNetwork.IsMasterClient) return;
            environmentController.SetIndices(indices);
            environmentController.SetVertices(vertices);
            environmentController.GenerateMap();
        }

        [PunRPC]
        private void SendChestSeedToPlayers(int seed)
        {
            if (debug) print("Hi from Avatar controller seed = "+seed);
            foreach (var player in players)
            {
                
                player.seedChest = seed;
            }
        }

        #endregion
    }
}
