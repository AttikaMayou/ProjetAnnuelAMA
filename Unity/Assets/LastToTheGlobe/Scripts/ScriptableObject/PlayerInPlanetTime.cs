using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerOnPlanet", menuName = "PlayerOnPlanet", order = 2)]
public class PlayerInTime : ScriptableObject
{
    [SerializeField]
    private List<PlanetTimeData> PlanetDataList;

    public List<PlanetTimeData> publicPlanetDataList => this.PlanetDataList;

    public void ResetData()
    {
        this.PlanetDataList.Clear();
    }

    //fonction de data à collecter
    public void AddidPlanet(int IdPlanet)
    {
        this.PlanetDataList.Add(new PlanetTimeData() { IdPlanet = IdPlanet });
    }
    public void AddTimeEntryCollider(float EnterTime)
    {
        this.PlanetDataList.Add(new PlanetTimeData() {EnterTime = EnterTime });
    }
    public void AddTimeExitCollider(float ExitTime)
    {
        this.PlanetDataList.Add(new PlanetTimeData() { ExitTime = ExitTime });
    }
    public void AddnumberOfChest(int numberOfChest)
    {
        this.PlanetDataList.Add(new PlanetTimeData() { numberOfChest = numberOfChest });
    }
    public void AddtimeChestOpened(float timeChestOpened)
    {
        this.PlanetDataList.Add(new PlanetTimeData() { timeChestOpened = timeChestOpened });
    }
}

//class de datas que l'on veut collecter
[Serializable]
public class PlanetTimeData
{
    public int IdPlanet;
    public int numberOfChest;
    public float EnterTime;
    public float ExitTime;
    public int numberOfPlayer;
    public float timeChestOpened;
}


