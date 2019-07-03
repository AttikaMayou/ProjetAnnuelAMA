using System;

namespace LastToTheGlobe.Scripts.httpRequests
{
    [Serializable]
    public class ParsedRequest
    {
        public string status;
        public int gameWon;
        public int playerKilled;
        public int coins;
        public int[] itemOwned;

    }
}
