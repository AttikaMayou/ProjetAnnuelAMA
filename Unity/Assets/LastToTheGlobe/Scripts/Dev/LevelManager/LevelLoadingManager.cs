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
    Lobby
}

namespace LastToTheGlobe.Scripts.Dev.LevelManager
{
    public class LevelLoadingManager : MonoBehaviourSingleton<LevelLoadingManager>
    {
        private LastToTheGlobeScene _currScene = LastToTheGlobeScene.NONE;

        private bool _isProcessing;
        private bool _levelLoaded;
  
        /// <summary>
        /// Handle RPC loss by deactivating the queue when loading a new scene
        /// </summary>
        /// <returns></returns>
        private IEnumerator SwitchToScene(LastToTheGlobeScene scene)
        {
            // To prevent multiple switch
            _isProcessing = true;
            
            // To prevent loss of RPCs, disable temporary the queue that contains it
            PhotonNetwork.IsMessageQueueRunning = false;
            CustomLoadingScene(scene);
            while (!_levelLoaded)
            {
                yield return null;
            }
            PhotonNetwork.IsMessageQueueRunning = true;
        }

        /// <summary>
        /// Called to load a scene properly and get the status
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneAsync(LastToTheGlobeScene scene)
        {
            if (!PhotonNetwork.IsMasterClient) yield break;

            if (!PhotonNetwork.InRoom) yield break;
            
            //TODO : mix this call with the PhotonNetwork function to load the level
            //AsyncOperation sceneLoading = PhotonNetwork.LoadLevelAsync(scene.ToString(), LoadSceneMode.Additive);
            AsyncOperation sceneLoading = SceneManager.LoadSceneAsync(scene.ToString());
           
            sceneLoading.allowSceneActivation = false;

            while (!sceneLoading.isDone)
            {
                yield return new WaitForEndOfFrame();
                if (sceneLoading.progress >= 0.9f)
                {
                    sceneLoading.allowSceneActivation = true;
                }

                yield return null;
            }

            _currScene = scene;

            PhotonNetwork.IsMessageQueueRunning = true;

            _isProcessing = false;
        }
        
        /// <summary>
        /// Called to unload a scene properly and get the status
        /// </summary>
        /// <param name="scene"></param>
        /// <returns></returns>
        private IEnumerator UnloadSceneAsync(LastToTheGlobeScene scene)
        {
            if (!PhotonNetwork.IsMasterClient) yield break;
            
            _isProcessing = true;

            if (_currScene != LastToTheGlobeScene.NONE)
            {
                PhotonNetwork.IsMessageQueueRunning = false;
                string oldScene = _currScene.ToString();

                AsyncOperation sceneUnloading = SceneManager.UnloadSceneAsync(_currScene.ToString());
                while (!sceneUnloading.isDone)
                {
                    //TODO : add debug or UI for process bar if needed
                    yield return new WaitForEndOfFrame();
                }

                yield return StartCoroutine(LoadSceneAsync(scene));
            }
            else
            {
                //In case there is no scene to unload
                yield return StartCoroutine(LoadSceneAsync(scene));
            }
        }

        /// <summary>
        /// Load a given scene 
        /// </summary>
        /// <param name="sceneName"></param>
        private static void CustomLoadingScene(LastToTheGlobeScene sceneName)
        {
            PhotonNetwork.LoadLevel((int)sceneName);
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
        public void SwitchSceneTo(LastToTheGlobeScene scene)
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
                StartCoroutine(UnloadSceneAsync(scene));
            }
        }
        
        /// <summary>
        /// Get the player back to the main menu
        /// </summary>
        public void BackToMainMenu()
        {
            SwitchSceneTo(LastToTheGlobeScene.MainMenu);
        }
    }
}
