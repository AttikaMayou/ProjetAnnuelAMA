using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerOnPlanet", menuName = "PlayerOnPlanet", order = 2)]
public class PlayerInTime : ScriptableObject
{
    [SerializeField]
    private List<PlanetTimeData> PlanetDataList = new List<PlanetTimeData>();

    public List<PlanetTimeData> publicPlanetDataList => this.PlanetDataList;

    public void ResetData()
    {
        this.PlanetDataList.Clear();
    }

    //fonction de data à collecter
    public void AddidPlanet(int IdPlanet, int numberOfChest, int numberOfPlayer, float EnterTime, float ExitTime,  float timeChestOpened, float timeOnPlanet)
    {
        this.PlanetDataList.Add(new PlanetTimeData() { IdPlanet = IdPlanet, numberOfChest = numberOfChest, numberOfPlayer = numberOfPlayer, EnterTime = EnterTime, ExitTime = ExitTime, timeChestOpened = timeChestOpened, timeOnPlanet = timeOnPlanet });
    }
}

//class de datas que l'on veut collecter
[Serializable]
public class PlanetTimeData
{
    public int IdPlanet;
    public int numberOfChest;
    public int numberOfPlayer;
    public float EnterTime;
    public float ExitTime;
    public float timeChestOpened;
    public float timeOnPlanet;
}


