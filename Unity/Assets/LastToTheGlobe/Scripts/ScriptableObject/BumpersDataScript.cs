using System.Collections.Generic;
using UnityEngine;

namespace LastToTheGlobe.Scripts.ScriptableObject
{
    public class BumpersDataScript : MonoBehaviour
    {
        private List<BumpersData> _bumpersData = new List<BumpersData>();

        public List<BumpersData> publicBumpersData => this._bumpersData;

        public void ResetData()
        {
            this._bumpersData.Clear();
        }

        public void AddBumperData(int bumperId, Vector3 bumperPos)
        {
            this._bumpersData.Add(new BumpersData()
            {
                BumperId = bumperId,
                BumperPos = bumperPos
            });
        }
    }

    public class BumpersData
    {
        public int BumperId;
        public Vector3 BumperPos;
        //public Vector3 KillPos;
    }
}
