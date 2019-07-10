using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestContentManagerScript : MonoBehaviour
{
    
    public Dictionary<GameObject, ItemSlotInventory> itemAndScripts;
    public List<GameObject> itemSlot;
    public List<GameObject> item;
    public List<ItemSlotInventory> itemSlotInventories;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < item.Count; i++)
        {
            itemAndScripts.Add(item[i], itemSlotInventories[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
