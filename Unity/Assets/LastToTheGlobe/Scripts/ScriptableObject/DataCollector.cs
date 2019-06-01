using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.LastToTheGlobe.Scripts.Avatar;

public class DataCollector : MonoBehaviour
{
    private static DataCollector instance;

    [SerializeField] private KillingDataListScript dataVault;
    [SerializeField] private bool resetData = true;
    //pour utiliser une animation curve
    [SerializeField] AnimationCurve curve;

    /*public void OnEnable()
    {
        this.chart = new AnimationCurve();
        va killCount = 0;
        foreach (var kPos in this.vault.KillData)
        {
            killCount += 1.0f;
            this.chart.AddKey(kPos.PartyTime, killCount);
        }
    }
    */

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

}



