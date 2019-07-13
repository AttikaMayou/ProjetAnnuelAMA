using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestContentManagerScript : MonoBehaviour
{
    
    
    public List<GameObject> itemSlot;
    public List<GameObject> pools;
    private int _content;
    
    // Start is called before the first frame update
    public void GenerateChestItem(int seed)
    {
        Random.InitState(seed);
        _content = Random.Range(0, 2);
        print("Seed généré avec succès : "+seed);
        pools[_content].transform.GetChild(0).transform.parent = itemSlot[0].transform;
    }
}
