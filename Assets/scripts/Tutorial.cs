/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////              Lorenz Steiniger-Brach          //////////////
/////////////////                   Flying Golf                //////////////
/////////////////                    Tutorial                  //////////////
/////////////////            Seminarfacharbeit ÖG 19.01.21     //////////////
/////////////////////////////////////////////////////////////////////////////

//Dieses Script sitzt auf einem Empty GameObjekt, welches der "Vater" der Tutorial UI ist
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    
    public GameObject LVLComplete;
    public GameObject gameOver;


    void Update() // Deaktiviert die Tutorial-Texte, wenn das Level gewonnen oder verloren ist.
    {
        if (LVLComplete.activeSelf || gameOver.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
