/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////              Lorenz Steiniger-Brach          //////////////
/////////////////                   Flying Golf                //////////////
/////////////////                   StartAndEnd                //////////////
/////////////////            Seminarfacharbeit ÖG 19.01.21     //////////////
/////////////////////////////////////////////////////////////////////////////


// Dieses Script sitzt auf einem Empty GameObjekt (dem GameManager)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAndEnd : MonoBehaviour
{
   
    //wie Funktionen, die von den Knöpfen Home und Next Level angesteuert werden
    public void Qiut()
    {
        SceneManager.LoadScene(0);
    }

    public void Begin()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
