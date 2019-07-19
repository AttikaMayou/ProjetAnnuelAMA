using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using Assets.LastToTheGlobe.Scripts.Avatar;
using LastToTheGlobe.Scripts.httpRequests;
using UnityEngine.Networking;
using LastToTheGlobe.Scripts.Environment.Planets;
using LastToTheGlobe.Scripts.ScriptableObject;
using Random = System.Random;

public class DataCollector : MonoBehaviour
{
    private static DataCollector instance;

    [SerializeField] private KillingDataListScript dataVault;
    [SerializeField] private PlayerInTime dataVaultPlanet;
    [SerializeField] private BumpersDataScript dataVaultBumper;
    [SerializeField] private bool resetData = true;
    private static bool requestFinished;
    //pour utiliser une animation curve
    [SerializeField] AnimationCurve curve;

    private int _arrayindex = 0;
    private float[] _timeArray = new float[10];
    private int[] _layerArray = new int[10];

    public static DataCollector Instance()
    {
        return instance;
    }

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        if (this.resetData)
        {
            instance.dataVault.ResetData();
            instance.dataVaultPlanet.ResetData();
            instance.dataVaultBumper.ResetData();
        }
    }
    
    public static void RegisterEnnemyKill(MonoBehaviour avatar)
    {
        if (instance != null && instance.dataVault != null) instance.dataVault.AddKillPosEntry(avatar.transform.position);
    }
    public static void RegisterEnnemyKillWithTime(MonoBehaviour avatar)
    {
        if (instance != null && instance.dataVault != null) instance.dataVault.AddKillPosEntry(avatar.transform.position, Time.time);
    }

    public static void RegisterBumper(BumperExposerScript bumper)
    {
        if (instance == null || instance.dataVaultBumper ! == null) return;
        var id = bumper.Id;
        var pos = bumper.BumperTransform.position;
        
        instance.dataVaultBumper.AddBumperData(id, pos);
    }

    //Planet data
    public static void RegisterPlanet(PlanetExposerScript planet)
    {
        if (instance != null && instance.dataVaultPlanet != null)
        {
            var id = UnityEngine.Random.Range(0, 23);
            var randTimeEnter = UnityEngine.Random.Range(10f, 30f);
            var randTimeExit = UnityEngine.Random.Range(20f, 50f);
            var randChestNumber = UnityEngine.Random.Range(1, 5);
            var randTimeChest = UnityEngine.Random.Range(15f, 40f);
            //int numberOfPlayer = UnityEngine.Random.Range(0, 2);
            int numberOfPlayer = 1;
            float timeOnPlanet = randTimeExit - randTimeEnter;

            if (numberOfPlayer == 0)
            {
                randTimeChest = 0;
                randTimeEnter = 0;
                randTimeExit = 0;
                timeOnPlanet = 0;
            }

            instance.dataVaultPlanet.AddidPlanet(id, randChestNumber, numberOfPlayer, randTimeEnter, randTimeExit, randTimeChest, timeOnPlanet);
        }
    }

    public static void RegisterDeathByLayer(MonoBehaviour avatar)
    {
        var randLayer = UnityEngine.Random.Range(0, 4);
        if (instance != null && instance.dataVault != null) instance.dataVault.AddKillPosEntry(avatar.transform.position, Time.time, randLayer);
        instance._timeArray[instance._arrayindex] = Time.time;
        instance._layerArray[instance._arrayindex] = randLayer;
        instance._arrayindex += 1;
        
        if (instance.dataVault.publickillingDataList.Count >= 10)
        {
            instance.StartCoroutine(dataSendRequest());
            Array.Clear(instance._timeArray, 0 , 10);
            Array.Clear(instance._layerArray, 0 , 10);
            instance._arrayindex = 0;

        }
    }

    public static IEnumerator dataSendRequest()
    {
        requestFinished = false;
        WWWForm form = new WWWForm();
        JSONData jsonData = new JSONData();

        jsonData.time = instance._timeArray;
        jsonData.layer = instance._layerArray;
        string JSONtoSend = JsonUtility.ToJson(jsonData);
        form.AddField("data", JSONtoSend);
        UnityWebRequest www = UnityWebRequest.Post("https://ebelder.pythonanywhere.com/data", form);
        yield return www.SendWebRequest();
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
            requestFinished = true;
            yield return null;
        }
        else
        {
                
            Debug.Log(www.downloadHandler.text);
            requestFinished = true;
            instance.dataVault.publickillingDataList.Clear();
        }
    }
}

[Serializable]
public class JSONData
{
    public float[] time;
    public int[] layer;
}



