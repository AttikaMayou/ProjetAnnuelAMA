using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    private static DataCollector instance;

    [SerializeField] private KillingDataListScript dataVault;
    [SerializeField] private bool resetData = true;

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
    /*
    public static void RegisterMinionKill(MinionBehaviour m)
    {
        if (instance != null && instance.dataVault != null) instance.dataVault.AddKillPosEntry(m.transform.position);
    }
    public static void RegisterMinionKillWithTime(MinionBehaviour m)
    {
        if (instance != null && instance.dataVault != null) instance.dataVault.AddKillPosEntry(m.transform.position, Time.time);
    }
    */
}
