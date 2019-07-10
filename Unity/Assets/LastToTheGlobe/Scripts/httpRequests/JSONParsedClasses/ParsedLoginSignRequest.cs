using System;

//Auteur : Abdallah


/*
  Classe sur laquelle est Parsé la réponse de la requete Login ou Signin 
*/

namespace LastToTheGlobe.Scripts.httpRequests
{
    [Serializable]
    public class ParsedLoginSignRequest
    {
        public string status;
        public int gameWon;
        public int playerKilled;
        public int coins;
        public int[] itemOwned;

    }
}
