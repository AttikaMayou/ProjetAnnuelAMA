using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonScript : MonoBehaviour
{
    public Button myButton;
    protected GameObject[] toEnable;
    protected GameObject[] toDisable;

    void Start()
    {
        myButton.onClick.AddListener(TaskOnClick);
        
        print("Hola");
    }

    protected void TaskOnClick()
    {
        for (int i = 0; i < toEnable.Length; i++)
        {
            toEnable[i].SetActive(true);
        }
        
        for (int i = 0; i < toDisable.Length; i++)
        {
            toDisable[i].SetActive(false);
        }

    }
}