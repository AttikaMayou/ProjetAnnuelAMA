using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LastToTheGlobe.Scripts.Inventory;
using UnityEngine;
using System.Collections;

public class TESTTPCTOERASE : MonoBehaviour
{
    private bool _nearChest = false;

    private IEnumerator enume;

    private UIChest _actualChest;
    // Start is called before the first frame update
    void Start()
    {
        enume = ChestState().GetEnumerator();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _nearChest)
        {
            enume.MoveNext();
            if (enume.Current.ToString() ==  "Closed") enume = ChestState().GetEnumerator();
            
        }
            
    }
    
    public void CloseToChest(UIChest chest)
    {
        _actualChest = chest;
        _nearChest = true;
        _actualChest.pressE.gameObject.SetActive(true);
        
    }

    public void AwayFromChest(UIChest chest)
    {
        _actualChest.pressE.gameObject.SetActive(false);
        _actualChest.playerInventory.gameObject.SetActive(false);
        _actualChest.chestInventory.gameObject.SetActive(false);
        _actualChest.playerOpenChest = false;
        _actualChest = null;
        _nearChest = false;
        if (enume.Current.ToString() == "Open")
        {
            enume = ChestState().GetEnumerator();
        }
    }

    private IEnumerable ChestState()
    {
        _actualChest.playerInventory.gameObject.SetActive(true);
        _actualChest.chestInventory.gameObject.SetActive(true);
        _actualChest.pressE.gameObject.SetActive(false);
        _actualChest.playerOpenChest = true;
        print("Open");
        yield return "Open";
        
        _actualChest.playerInventory.gameObject.SetActive(false);
        _actualChest.chestInventory.gameObject.SetActive(false);
        _actualChest.playerOpenChest = false;
        print("Closed");
        if (_nearChest)
        {
            _actualChest.pressE.gameObject.SetActive(true);
        }
        else
        {
            _actualChest.pressE.gameObject.SetActive(false);
        }
        
        yield return "Closed";
    }
}
