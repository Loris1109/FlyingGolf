/////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////
/////////////////              Lorenz Steiniger-Brach          //////////////
/////////////////                    Flying Golf               //////////////
/////////////////                UIWindEinstellungen           //////////////
/////////////////            Seminarfacharbeit ÖG 19.01.21     //////////////
/////////////////////////////////////////////////////////////////////////////

// Dieses Script sitzt auf Schiebereglern
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWindEinstellungen : MonoBehaviour
{
    
    string facing;
    GameObject effector;//(Ventilator)
    GameManager gameManager;
    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        if (GetComponent<Slider>() != null && effector != null) //zur sicherheit, damit mit kein Nullpointer auftritt
        {
            GetComponent<Slider>().maxValue = effector.transform.parent.GetComponent<ActivateWind>().GetMaxForce();
            //die Oberegrenze des Scheiberegler wird festgelegt
            GetComponent<Slider>().minValue = effector.transform.parent.GetComponent<ActivateWind>().GetMinForce();
            // die Untere Grenze des Schiebereglers wird festgelegt
            GetComponent<Slider>().value = effector.transform.parent.GetComponent<ActivateWind>().GetCurForce();
            //der Startwert wird festgelegt (der Regler wird platziert) 
        }
    }

    public void ChangeForce (float newForce)//Der Input des Schiebereglers (newForce) wird zum ActivateWind Script weitergegeben. 
    {
        if (effector!=null)
        {// der Wert wird nur weitergegeben, wenn er im Intervall zwischen max und min Force ist. (sollte er immer sein).
            if (newForce < effector.transform.parent.GetComponent<ActivateWind>().GetMaxForce() && 
                newForce > effector.transform.parent.GetComponent<ActivateWind>().GetMinForce())
            {
                effector.transform.parent.GetComponent<ActivateWind>().SetCurForce(newForce);
            }
        }
    }
    private void Update()
    {
        if (effector != null)
        {//Wenn sich der Mindestwert verändert hat z.B. weil die Richtung des Ventilators geändert wurde, 
            //werden Max-,Min- und CurForce mit den neuen Werten ausgetauscht. 
            if (effector.transform.parent.GetComponent<ActivateWind>().GetMinForce() != GetComponent<Slider>().minValue)
            {
                GetComponent<Slider>().minValue = effector.transform.parent.GetComponent<ActivateWind>().GetMinForce();
                GetComponent<Slider>().maxValue = effector.transform.parent.GetComponent<ActivateWind>().GetMaxForce();
                GetComponent<Slider>().value = effector.transform.parent.GetComponent<ActivateWind>().GetCurForce();
            }

        }
        
    }
    //der Effector(der Ventilator/rotes Viereck mit Pfeil) wird vom ActivateWind Script übertragen.
    public void SetEffector(GameObject newEffector)
    {
        effector = newEffector;
    }
    
}
