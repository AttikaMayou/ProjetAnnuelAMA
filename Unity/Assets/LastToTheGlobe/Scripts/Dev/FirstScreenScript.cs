using UnityEngine;
using UnityEngine.SceneManagement;

//Auteur : Attika
//Dirty script while we dont have a profile save management
//This script is suppose to log on the player when he launch the game

namespace LastToTheGlobe.Scripts.Dev
{
    public class FirstScreenScript : MonoBehaviour
    {
        public void SwitchToMainMenu()
        {
            SceneManager.LoadScene("GameRoom");
        }
    }
}
