using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class OpenCoHUD : ButtonScript
{
    public GameObject enterPseudo;
    public GameObject mainConnexionMenu;

    void Start () {
        myButton.onClick.AddListener(TaskOnClick);

        toEnable.Append(enterPseudo);
        toDisable.Append(mainConnexionMenu);
    }
}