using System.Collections;
using System.Collections.Generic;
using Assets.LastToTheGlobe.Scripts.Avatar;
using Assets.LastToTheGlobe.Scripts.Camera;
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
        private Vector3[] _spawnPos;
        [SerializeField] private CloudPlanet_STRUCT environmentController;
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
            startMenuController.GameCanStart += LaunchGameRoom;
            //startMenuController.GameCanStart += SetSeed;
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
                StopCoroutine(CountdownBeforeSwitchingScene(0.0f));
                StartCoroutine(CountdownBeforeSwitchingScene(2.0f));
            }

            if (_activatedIntentReceivers == null
                || players == null
                || players.Length != _activatedIntentReceivers.Length)
            {
                Debug.LogError("There is something wrong with avatars and intents setup !");
                return;
            }
            
            var i = 0;
            for (; i < _activatedIntentReceivers.Length; i++)
            {
                var moveIntent = Vector3.zero;

                var intent = _activatedIntentReceivers[i];
                var player = players[i];

                var rb = player.characterRb;
                var tr = player.characterTr;

                if (player == null) continue;

                if (intent.MoveBack || intent.MoveForward
                                    || intent.MoveRight || intent.MoveLeft)
                {
                    moveIntent += new Vector3(intent.strafe, 0.0f, intent.forward);
                }

                if (intent.Jump)
                {
                    if (player.attractor == null)
                    {
                        Debug.LogError("There is no attractor near us !");
                        return;
                    }
                    var jumpDir = player.attractor.dirForce;
                    rb.AddForce(jumpDir * 250);
                }

                if (intent.Shoot)
                {
                    if(debug) Debug.Log("Shoot intent");
                    if (_currentOrb == null)
                    {
                        _currentOrb = GetOrbsWithinPool();
                        _currentOrb.playerTransform = player.characterTr;
                        _currentOrb.attractor = player.attractor;
                        _currentOrb.charged = false;
                        _currentOrb.gameObject.SetActive(true);
                        _currentOrb.InitializeOrPosition();
                        intent.canShoot = true;
                        intent.Shoot = false;
                        _currentOrb = null;
                        //TODO : when the orb is reset --> canShoot = true
                    }
                }

                if (intent.ShootLoaded)
                {
                    if(debug) Debug.Log("Loaded shoot intent");
                    if (_currentOrb == null)
                    {
                        _currentOrb = GetOrbsWithinPool();
                        _currentOrb.playerTransform = player.characterTr;
                        _currentOrb.attractor = player.attractor;
                        _currentOrb.charged = true;
                        _currentOrb.gameObject.SetActive(true);
                        _currentOrb.InitializeOrPosition();
                        intent.canShoot = true;
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
                rb.MovePosition(rb.position + tr.TransformDirection(moveIntent) * intent.speed * Time.deltaTime);
                tr.Rotate(new Vector3(0, intent.rotationOnX, 0));
                player.cameraRotatorX.transform.Rotate(new Vector3(-intent.rotationOnY, 0, 0), Space.Self);

                if (player.attractor == null)
                {
                    //Debug.LogError("There is no attractor near us !");
                    return;
                }
                //TODO : make this master client server like 
                player.attractor.Attractor(i, -2600.0f);
                /*if (intent.canJump && player.attractor)
                {
                }
                else if(!intent.canJump && player.attractor)
                {
                }*/

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
                Debug.LogError("there is no intent receivers");
                return;
            }

            foreach (var intent in _activatedIntentReceivers)
            {
                intent.enabled = true;
                intent.MoveBack = false;
                intent.MoveForward = false;
                intent.MoveLeft = false;
                intent.MoveRight = false;
                intent.Run = false;
                intent.Jump = false;
                intent.canJump = true;
                intent.Dash = false;
                intent.canDash = true;
                intent.Shoot = false;
                intent.canShoot = true;
                intent.Bump = false;
                intent.Interact = false;
                intent.forward = 0.0f;
                intent.strafe = 0.0f;
            }
        }

        /// Called to activate the avatar root gameObject when a player join the game
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

        /// Called to set the right local target to camera
        private void SetupCamera(int id)
        {
            //if (photonView.IsMine != players[id].characterPhotonView) return;
            if (myCamera.enabled) return;
            myCamera.enabled = true;
            myCamera.playerExposer = players[id];
            myCamera.InitializeCameraPosition();
            myCamera.startFollowing = true;
            players[id].lifeUI = lifeUI;
            players[id].victoryUI = victoryUI;
            players[id].defeatUI = defeatUI;
            if(debug) Debug.Log("Camera is set for " + id);
        }

        /// Each time a player join the lobby, we check if we're enough. If yes, we load the GameRoom after a countdown
        private void LaunchGameRoom()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (PhotonNetwork.IsMasterClient && _seed == 0)
            {
                // _seed = environmentController.GetSeed();
                _seed = 10;
                environmentController.SetSeed(_seed);
                FindAllSpawnPoint();
                if(debug) Debug.Log("seed is " + _seed);
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

        /// <summary>
        /// Check if there is enough players to start the game and leave Lobby
        /// </summary>
        /// <returns></returns>
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

        /// Wait the time indicated before teleport players to the spawn points
        private IEnumerator CountdownBeforeSwitchingScene(float time)
        {
            yield return new WaitForSeconds(time);
            
            if (_spawnPointInPlanet.Length <= 1)
            {
                Debug.LogError("There is a problem with the map instantiation");
                yield break;
            }
            
            //Teleport players on planets
            for(var i = 0; i<= players.Length; i++)
            {
                if (!players[i].isActiveAndEnabled) break;
                //TODO : deactivate rb and set isKinematic = false 
                players[i].characterRootGameObject.transform.position = _spawnPos[i + 1];
                if (debug)
                {
                    Debug.Log("Previous pos : " + players[i].characterRootGameObject.transform.position);
                    Debug.Log("Final position : " +_spawnPointInPlanet[i].transform.position);
                    Debug.Log("Local position :  " + _spawnPointInPlanet[i].transform.localPosition);
                }
                yield return new WaitForSeconds(0.5f);
            }

            gameLaunched = true;
        }

        private void FindAllSpawnPoint()
        {
            _spawnPointInPlanet = GameObject.FindGameObjectsWithTag("SpawnPoint");
            //TODO : do not use tags to find all spawn points
            if (debug)
            {
                var i = 0;
                foreach (var point in _spawnPointInPlanet)
                {
                    Debug.Log("Spawn Point : " + i + " is " + point);
                    i++;
                }
            }
            _spawnPos = new Vector3[_spawnPointInPlanet.Length + 1];
            for (var i = 0; i < _spawnPointInPlanet.Length; i++)
            {
                _spawnPos[i] = _spawnPointInPlanet[i].transform.position;
            }
        }

        private OrbManager GetOrbsWithinPool()
        {
            foreach (var orb in orbsPool)
            {
                if (orb.gameObject.activeSelf)
                {
                    if(debug) Debug.Log("orb is enabled");
                    continue;
                }
                else
                {
                    if (debug) Debug.Log("Orb selected is " + orb);
                    return orb;
                }
            }
            if(debug) Debug.Log("There is no orb available");
            return null;
        }

        #endregion

        #region RPC Methods

        [PunRPC]
        private void ActivateAvatarRPC(int avatarId)
        {
            players[avatarId].characterRootGameObject.SetActive(true);
        }

        [PunRPC]
        private void DeactivateAvatarRPC(int avatarId)
        {
            players[avatarId].characterRootGameObject.SetActive(false);
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
