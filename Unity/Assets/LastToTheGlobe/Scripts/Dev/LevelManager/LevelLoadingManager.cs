using System;
using System.Collections;
using LastToTheGlobe.Scripts.Singleton;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

//Auteur : Attika

// Make sure that scene in the Builds Settings are in this exact order and named like following
public enum LastToTheGlobeScene
{
    NONE,
    MainMenu,
    Lobby,
    GameRoom
}

namespace LastToTheGlobe.Scripts.Dev.LevelManager
{
    public class LevelLoadingManager : MonoBehaviourSingleton<LevelLoadingManager>
    {
        private static LastToTheGlobeScene _currScene = LastToTheGlobeScene.NONE;

        private static bool _isProcessing;

        /// <summary>
        /// Load a given scene
        /// </summary>
        /// <param name="scene"></param>
        private static IEnumerator CustomLoadingScene(LastToTheGlobeScene scene)
        {
            _isProcessing = true;
            
            if (scene == LastToTheGlobeScene.GameRoom)
            {
                if (!PhotonNetwork.IsMasterClient) yield return null;
                
                //Every clients will load the GameRoom as the same time
                PhotonNetwork.AutomaticallySyncScene = true;
            }
            
            //Load the levels asynchronously among clients
            PhotonNetwork.LoadLevel((int)scene);

            while (PhotonNetwork.LevelLoadingProgress < 1)
            {
                yield return new WaitForEndOfFrame();
            }

            _isProcessing = false;
            
            _currScene = scene;
        }

        /// <summary>
        /// Called to check if scene exists in the build
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        private static bool IsSceneValid(LastToTheGlobeScene scene)
        {
            foreach (var sc in Enum.GetValues(typeof(LastToTheGlobeScene)))
            {
                if ((int) scene == (int) sc)
                {
                    return true;
                }   
            }
            return false;
        }

        public LastToTheGlobeScene GetCurrentScene()
        {
            return _currScene;
        }

        /// <summary>
        /// Switch the player to the scene given in parameters
        /// </summary>
        /// <param name="scene"></param>
        public void SwitchToScene(LastToTheGlobeScene scene)
        {
            if (!IsSceneValid(scene))
            {
                Debug.LogError("Argument invalid, the parameter is not in the enum range");
                return;
            }

            if (scene == LastToTheGlobeScene.NONE)
            {
                Debug.LogError("Invalid argument NONE scene");
                return;
            }

            if (_currScene == scene)
            {
                Debug.LogWarning("Trying to switch to next scene with the current scene as argument");
                return;
            }

            if (!_isProcessing)
            {
                StartCoroutine(CustomLoadingScene(scene));
            }
        }
        
        /// <summary>
        /// Get the player back to the main menu
        /// </summary>
        public void BackToMainMenu()
        {
            SwitchToScene(LastToTheGlobeScene.MainMenu);
            //TODO : add logic for quiting the room ? 
        }
    }
}
