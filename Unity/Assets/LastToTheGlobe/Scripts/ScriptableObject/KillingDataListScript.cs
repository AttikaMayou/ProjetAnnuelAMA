using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KillingDataListScript", menuName = "ScriptableData", order = 1)]
public class KillingDataListScript : ScriptableObject
{
    [SerializeField]
    private List<KillData> killingDataList;

    public List<KillData> publickillingDataList => this.killingDataList;

    public void ResetData()
    {
        this.killingDataList.Clear();
    }

    //fonction de data à collecter
    public void AddKillPosEntry(Vector3 pos)
    {
        this.killingDataList.Add(new KillData() { killPosition = pos });
    }
    public void AddKillPosEntry(Vector3 pos, float timeStamp)
    {
        this.killingDataList.Add(new KillData() { killPosition = pos, killTime = timeStamp });
    }

    public void AddPlayerKill(KillData data)
    {
        this.killingDataList.Add(data);
    }
}

//class de datas que l'on veut collecter
    [Serializable]
    public class KillData
    {
        public Vector3 killPosition;
        public float killTime;
        public PlanetType killOnTypePlanet;
    }
