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
        pools[_content].transform.GetChild(0).transform.SetParent(itemSlot[0].transform);
        itemSlot[0].transform.GetChild(1).gameObject.SetActive(true);
        itemSlot[0].transform.GetChild(1).transform.localScale = new Vector3(1,1,1);
    }
}
