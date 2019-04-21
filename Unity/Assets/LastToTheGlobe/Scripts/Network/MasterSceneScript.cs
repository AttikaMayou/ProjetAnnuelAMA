using UnityEngine;
using UnityEngine.SceneManagement;

//Auteur : Attika
//Dirty script while we don't have a profile save management
//This script is suppose to log on the player when he launch the game

namespace LastToTheGlobe.Scripts.Network
{
    public class MasterSceneScript : MonoBehaviour
    {
        public void SwitchToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
