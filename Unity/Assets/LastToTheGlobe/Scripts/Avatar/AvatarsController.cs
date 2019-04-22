using System.Collections;
using System.Collections.Generic;
using LastToTheGlobe.Scripts.Camera;
using LastToTheGlobe.Scripts.Dev.LevelManager;
using LastToTheGlobe.Scripts.Environment.ProceduralGenerationMap.Voronoi;
using LastToTheGlobe.Scripts.Network;
using LastToTheGlobe.Scripts.Weapon.Orb;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

//Auteur : Attika
//Modification : Margot

namespace LastToTheGlobe.Scripts.Avatar
{
    public class AvatarsController : MonoBehaviour
    {
        public bool debug = true;
        
        [Header("Photon and Replication Parameters")] 
        [SerializeField] private CharacterExposerScript[] players;
        [SerializeField] private AIntentReceiver[] onlineIntentReceivers;
        [SerializeField] private AIntentReceiver[] _activatedIntentReceivers;
        [SerializeField] private PhotonView photonView;
        
        [Header("Environment Parameters")]
        //spawn point tab
        private GameObject[] _spawnPointInPlanet;
        [SerializeField] private Vector3[] spawnPos;
        [SerializeField] private CloudPlanet environmentController;
        private int _seed = 0;

        [Header("Camera Parameters")] 
        public CameraControllerScript myCamera;
        [SerializeField] private float rotationSpeed = 5.0f;
        
        [Header("Game Control Parameters And References")]
        [SerializeField] private StartMenuController startMenuController;
        [SerializeField] private bool gameStarted;
        [SerializeField] private bool onLobby;
        [SerializeField] private bool gameLaunched;
        [SerializeField] private int nbMinPlayers = 2;
        [SerializeField] private float countdown = 10.0f;
        private float _countdownStartValue;
        
        [SerializeField] private List<OrbManager> orbsPool = new List<OrbManager>();
        
        
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
                    var jumpDir = player.attractor.dirForce;
                    rb.AddForce(jumpDir * 250);
                }

                if (intent.Shoot)
                {
                    var orb = GetOrbsWithinPool();
                    if (orb == null)
                    {
                        Debug.LogError("Orbs pools return null reference");
                        return;
                    }
                    orb.playerTransform = player.characterTr;
                    orb.gameObject.SetActive(true);
                    intent.canShoot = true;
                }

                if (intent.ShootLoaded)
                {
                    var orb = GetOrbsWithinPool();
                    if (orb == null)
                    {
                        Debug.LogError("Orbs pools return null reference");
                        return;
                    }
                    orb.playerTransform = player.characterTr;
                    orb.charged = true;
                    orb.gameObject.SetActive(true);
                    intent.canShoot = true;
                }

                if (intent.Bump)
                {
                    
                }

                if (intent.Interact)
                {
                    
                }
                
                rb.MovePosition(rb.position + tr.TransformDirection(moveIntent) * intent.speed * Time.deltaTime);
            }
        }

        #endregion

        
        #region Private Methods

        /// <summary>
        /// Set the intentReceivers tab
        /// </summary>
        private void ChooseAndSubscribeToIntentReceivers()
        {
            _activatedIntentReceivers = onlineIntentReceivers;
            EnableIntentReceivers();
            gameStarted = true;
        }

        /// <summary>
        /// Activate the intentReceivers and set defaults values
        /// </summary>
        private void EnableIntentReceivers()
        {
            if (_activatedIntentReceivers == null)
            {
                Debug.Log("there is no intent receivers");
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
                intent.Dash = false;
                intent.Shoot = false;
                intent.Bump = false;
                intent.Interact = false;
                intent.forward = 0.0f;
                intent.strafe = 0.0f;
            }
        }

        /// <summary>
        /// Called to activate the avatar root gameObject when a player join the game
        /// </summary>
        /// <param name="id"></param>
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

        /// <summary>
        /// Called to set the right local target to camera
        /// </summary>
        /// <param name="id"></param>
        private void SetupCamera(int id)
        {
            //if (photonView.IsMine != players[id].characterPhotonView) return;
            if (myCamera.enabled) return;
            myCamera.enabled = true;
            myCamera.playerExposer = players[id];
            myCamera.InitializeCameraPosition();
            myCamera.startFollowing = true;
        }

        /// <summary>
        /// Each time a player join the lobby, we check if we're enough. If yes, we load the GameRoom after a countdown
        /// </summary>
        private void LaunchGameRoom()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            if (PhotonNetwork.IsMasterClient && _seed == 0)
            {
                _seed = environmentController.GetSeed();
                environmentController.SetSeed(_seed);
                FindAllSpawnPoint();
                if(debug) Debug.Log("seed is " + _seed);
            }
            photonView.RPC("SendSeedToPlayers", RpcTarget.Others, _seed);
            if(!CheckIfEnoughPlayers() || gameLaunched) return;
            onLobby = true;
            startMenuController.ShowLobbyCountdown();
            StartCoroutine(CountdownBeforeSwitchingScene(_countdownStartValue));
        }

        /// <summary>
        /// Check if there is enough players to start the game and leave Lobby
        /// </summary>
        /// <returns></returns>
        private bool CheckIfEnoughPlayers()
        {
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

        /// <summary>
        /// Wait the time indicated before teleport players to the spawn points
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
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
                players[i].characterRootGameObject.transform.position = spawnPos[i + 1];
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
            if (debug)
            {
                var i = 0;
                foreach (var point in _spawnPointInPlanet)
                {
                    Debug.Log("Spawn Point : " + i + " is " + point);
                    i++;
                }
            }
            spawnPos = new Vector3[_spawnPointInPlanet.Length + 1];
            for (var i = 0; i < _spawnPointInPlanet.Length; i++)
            {
                spawnPos[i] = _spawnPointInPlanet[i].transform.position;
            }
        }

        private OrbManager GetOrbsWithinPool()
        {
            foreach (var orb in orbsPool)
            {
                if (orb.enabled)
                {
                    continue;
                }
                else
                {
                    if (debug) Debug.Log("Orb selected is " + orb);
                    return orb;
                }
            }
            if(debug) Debug.Log("There is no orb available");
            return new OrbManager();
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
            environmentController.SetSeed(_seed);
        }

        #endregion
    }
}
